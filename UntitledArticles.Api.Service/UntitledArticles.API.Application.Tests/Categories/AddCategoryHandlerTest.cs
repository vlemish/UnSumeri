using System.Reflection.Metadata.Ecma335;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

using UntitiledArticles.API.Application.Models.Mediatr;

public class AddCategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepository;

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

        AddCategoryHandler handler = new(new Mock<ILogger<AddCategoryHandler>>().Object, _categoryRepository.Object);

        ResultDto<AddCategoryResult> actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);
        Assert.Equal(actual.Payload.Id, addedCategory.Id);

        _categoryRepository
            .Verify(m => m.AddAsync(It.IsAny<Category>()), Times.Once());
    }

    private void SetupMocks(Category addedCategory)
    {
        _categoryRepository = new();
        _categoryRepository
            .Setup(m => m.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(addedCategory);
    }
}
