using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

using UntitiledArticles.API.Application.Models.Mediatr;

public class AbbSubcategoryHandlerTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;

    [Theory]
    [InlineData("a", 2)]
    [InlineData("test_data", 2)]
    public void TestAddSubcategoryValidator_WhenNameNotNullAndIdPositive_ThenSuccess(string name, int parentId)
    {
        int expectedErrorCount = 0;
        AddSubcategory addSubcategory = new(name, parentId);
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
    public void TestAddSubcategoryValidator_WhenNameAndIdNotValid_ThenSuccess(string name, int parentId, int expectedErrorCount)
    {
        AddSubcategory addSubcategory = new(name, parentId);
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
        OperationStatusValue expectedOperationStatus = OperationStatusValue.OK;
        AddSubcategory request = new(name, parentId);
        Category category = new()
        {
            Id = 2,
            Name = name,
            ParentId = parentId
        };

        SetupMocks(category);

        AddSubcategoryHandler handler = new(_categoryRepositoryMock.Object);

        ResultDto<AddSubcategoryResult> result = await handler.Handle(request, default);

        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Equal(category.Id, result.Payload.Id);

        _categoryRepositoryMock
            .Verify(m=> m.AddAsync(It.IsAny<Category>()), Times.Once());
    }

    private void SetupMocks(Category addedCategory)
    {
        _categoryRepositoryMock = new();
        _categoryRepositoryMock
            .Setup(m => m.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(addedCategory);
    }
}
