﻿using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using AnSumeri.API.Application.Categories.Commands.AddSubcategory;
using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Tests.Categories;

using MediatR;
using AnSumeri.API.Application.Categories.Queries.FindOne;
using AnSumeri.API.Application.Categories.Queries.FindOne.Statuses;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses.Shared.Categories;

public class AddSubcategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly DateTime _testDateTime = new(1999, 5, 26, 1, 1, 1);

    [Theory]
    [InlineData("a", 2)]
    [InlineData("test_data", 2)]
    public void TestAddSubcategoryValidator_WhenNameNotNullAndIdPositive_ThenSuccess(string name, int parentId)
    {
        int expectedErrorCount = 0;
        AddSubcategory addSubcategory = new(name, Guid.NewGuid().ToString(), parentId);
        AddSubcategoryValidator validator = new AddSubcategoryValidator();

        ValidationResult result = validator.Validate(addSubcategory);

        Assert.True(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
    }

    [Theory]
    [InlineData("", 2, 1)]
    [InlineData(null, 2, 1)]
    [InlineData("asd", -2, 1)]
    [InlineData(null, -2, 2)]
    public void TestAddSubcategoryValidator_WhenNameAndIdNotValid_ThenSuccess(string name, int parentId,
        int expectedErrorCount)
    {
        AddSubcategory addSubcategory = new(name, Guid.NewGuid().ToString(), parentId);
        AddSubcategoryValidator validator = new AddSubcategoryValidator();

        ValidationResult result = validator.Validate(addSubcategory);

        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorCount, result.Errors.Count);
    }

    [Fact]
    public async Task TestAddSubcategory_WhenNoExceptions_ThenSuccessStatus()
    {
        int id = 2;
        string name = "testCategory";
        int parentId = 3;
        OperationStatusValue expectedOperationStatus = OperationStatusValue.Created;
        AddSubcategory request = new(name, Guid.NewGuid().ToString(), parentId);
        Category category = new() { Id = 2, Name = name, ParentId = parentId };

        SetupMocks(category);

        AddSubcategoryHandler handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object,
            _dateTimeProviderMock.Object);

        ResultDto<AddSubcategoryResult> result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Equal(category.Id, result.Payload.Id);

        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryRepositoryMock
            .Verify(m => m.AddAsync(It.IsAny<Category>()), Times.Once());
    }

    [Fact]
    public async Task TestAddSubcategoryHandler_WhenCategoryAlreadyExists_ThenDuplicateStatus()
    {
        int id = 2;
        string name = "testCategory";
        int parentId = 3;
        OperationStatusValue expectedOperationStatus = OperationStatusValue.Duplicate;
        AddSubcategory request = new(name, Guid.NewGuid().ToString(), parentId);

        this.SetupDuplicateMocks();

        AddSubcategoryHandler handler = new(_categoryRepositoryMock.Object, this._mediatorMock.Object,
            _dateTimeProviderMock.Object);

        ResultDto<AddSubcategoryResult> actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);

        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()),
                Times.Once());
        this._categoryRepositoryMock
            .Verify(m => m.AddAsync(It.IsAny<Category>()), Times.Never());
    }

    private void SetupMocks(Category addedCategory)
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();

        _categoryRepositoryMock
            .Setup(m => m.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(addedCategory);
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
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<FindOneByFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto<FindOneByFilterResult>(new FindOneByFilterSuccess(),
                new FindOneByFilterResult() { Name = "name", Id = 2, }));
    }
}
