using System.Reflection.Metadata.Ecma335;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using AnSumeri.API.Application.Categories.Commands.Add;
using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Tests.Categories;

using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using AnSumeri.API.Application.Categories.Queries.FindOne;
using AnSumeri.API.Application.Categories.Queries.FindOne.Statuses;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;

public class AddCategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepository;
    private Mock<IMediator> _mediatorMock;
    private Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly DateTime _testDateTime = new(1999, 5, 26, 1, 1, 1);

    [Theory]
    [InlineData("testName")]
    [InlineData("t")]
    public void TestAddCategoryValidator_WhenNameNotNullOrEmpty_ThenValid(string name)
    {
        int expectedErrorsCount = 0;
        AddCategory addCategory = new(name, Guid.NewGuid().ToString());

        AddCategoryValidator validator = new();
        ValidationResult validationResult = validator.Validate(addCategory);

        Assert.True(validationResult.IsValid);
        Assert.Equal(expectedErrorsCount, validationResult.Errors.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestAddCategoryValidator_WhenNameNullOrEmpty_ThenValid(string name)
    {
        int expectedErrorsCount = 1;
        AddCategory addCategory = new(name, Guid.NewGuid().ToString());

        AddCategoryValidator validator = new();
        ValidationResult validationResult = validator.Validate(addCategory);

        Assert.False(validationResult.IsValid);
        Assert.Equal(expectedErrorsCount, validationResult.Errors.Count);
    }

    [Fact]
    public async Task TestAddCategoryHandler_WhenCategoryAdded_ThenSuccessStatus()
    {
        string name = "test_name";
        AddCategory request = new AddCategory(name, Guid.NewGuid().ToString());
        OperationStatusValue expectedOperationStatus = OperationStatusValue.Created;
        Category addedCategory = new Category() { Id = 2, Articles = null, Name = name };

        SetupMocks(addedCategory);

        AddCategoryHandler handler = new(_categoryRepository.Object, this._mediatorMock.Object,
            _dateTimeProviderMock.Object);

        ResultDto<AddCategoryResult> actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);
        Assert.Equal(actual.Payload.Id, addedCategory.Id);

        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryRepository
            .Verify(m => m.AddAsync(It.IsAny<Category>()), Times.Once());
    }

    [Fact]
    public async Task TestAddCategoryHandler_WhenCategoryAlreadyExists_ThenDuplicateStatus()
    {
        string name = "test_name";
        AddCategory request = new AddCategory(name, Guid.NewGuid().ToString());
        OperationStatusValue expectedOperationStatus = OperationStatusValue.Duplicate;

        this.SetupDuplicateMocks();

        AddCategoryHandler handler = new(_categoryRepository.Object, this._mediatorMock.Object,
            _dateTimeProviderMock.Object);

        ResultDto<AddCategoryResult> actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);

        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryRepository
            .Verify(m => m.AddAsync(It.IsAny<Category>()), Times.Never());
    }

    private void SetupMocks(Category addedCategory)
    {
        _categoryRepository = new();
        _categoryRepository
            .Setup(m => m.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(addedCategory);
        _mediatorMock = new();
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto<FindOneByFilterResult>(new CategoryNoContent(), null));
        _dateTimeProviderMock = new();
        _dateTimeProviderMock.Setup(m => m.Current)
            .Returns(_testDateTime);
    }

    private void SetupDuplicateMocks()
    {
        SetupMocks(null);
        this._mediatorMock
            .Setup(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto<FindOneByFilterResult>(new FindOneByFilterSuccess(),
                new FindOneByFilterResult() { Name = "name", Id = 2, }));
    }
}
