using MediatR;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.Update;
using UntitiledArticles.API.Application.Categories.Commands.Update.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.FindMany;
using UntitiledArticles.API.Application.Categories.Queries.FindMany.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.FindOne;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitiledArticles.API.Application.OperationStatuses.Shared.Categories;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

public class UpdateCategoryHandlerTest
{
    private Mock<IMediator> _mediatorMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategoryValid_ThenSuccess()
    {
        var testData = GetTestData();
        UpdateCategory request = new(testData.id, testData.userId, testData.name);

        ResultDto<FindManyByFilterResult> expectedFilterResult = new(
            new FindManyByFilterSuccess(),
            new()
            {
                Categories = new List<FindOneByFilterResult>()
                {
                    new() { Name = "prev_name", Id = testData.id, UserId = testData.userId },
                    new() { Name = "newName", Id = testData.id + 1, UserId = testData.userId },
                }
            });
        SetupMocks(expectedFilterResult);

        UpdateCategoryHandler handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(OperationStatusValue.NoContent, actual.OperationStatus.Status);

        VerifySuccessMocks();
    }

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategoryNotExists_ThenNotFoundResult()
    {
        var testData = GetTestData();
        UpdateCategory request = new(testData.id, testData.userId, testData.name);

        ResultDto<FindManyByFilterResult> expectedFilterResult = new(new CategoryNoContent(), null);
        SetupMocks(expectedFilterResult);

        UpdateCategoryHandler handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedFilterResult.OperationStatus.Status, actual.OperationStatus.Status);

        VerifyValidationFailedMocks();
    }

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategoryDuplicateName_ThenDuplicateResult()
    {
        var testData = GetTestData();
        UpdateCategory request = new(testData.id, testData.userId, testData.name);

        ResultDto<FindManyByFilterResult> expectedFilterResult = new(new DuplicateCategory(testData.name),
            new()
            {
                Categories = new List<FindOneByFilterResult>()
                {
                    new() { Name = "previous_name", Id = testData.id, UserId = testData.userId },
                    new() { Name = testData.name, Id = testData.id + 1, UserId = testData.userId },
                }
            });
        SetupMocks(expectedFilterResult);

        UpdateCategoryHandler handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedFilterResult.OperationStatus.Status, actual.OperationStatus.Status);

        VerifyValidationFailedMocks();
    }

    [Fact]
    public async Task TestUpdateCategoryHandler_WhenCategorySameName_ThenNotModifiedResult()
    {
        var testData = GetTestData();
        UpdateCategory request = new(testData.id, testData.userId, testData.name);

        ResultDto<FindManyByFilterResult> expectedFilterResult = new(
            new CategoryNotModified(testData.id, testData.userId, testData.name),
            new()
            {
                Categories = new List<FindOneByFilterResult>()
                {
                    new() { Name = testData.name, Id = testData.id, UserId = testData.userId },
                    new() { Name = "newName", Id = testData.id + 1, UserId = testData.userId },
                }
            });
        SetupMocks(expectedFilterResult);

        UpdateCategoryHandler handler = new(_categoryRepositoryMock.Object, _mediatorMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedFilterResult.OperationStatus.Status, actual.OperationStatus.Status);

        VerifyValidationFailedMocks();
    }

    private void SetupMocks(ResultDto<FindManyByFilterResult> expectedFilterResult)
    {
        _mediatorMock = new();
        _categoryRepositoryMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<FindManyByFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedFilterResult);
        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);
    }

    private void VerifyValidationFailedMocks()
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<FindManyByFilter>(), It.IsAny<CancellationToken>()), Times.Once());
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.IsAny<Category>()), Times.Never());
    }

    private void VerifySuccessMocks()
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<FindManyByFilter>(), It.IsAny<CancellationToken>()), Times.Once());
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.IsAny<Category>()), Times.Once());
    }

    private (int id, string userId, string name) GetTestData() =>
        new(2, "userId", "category_name");
}
