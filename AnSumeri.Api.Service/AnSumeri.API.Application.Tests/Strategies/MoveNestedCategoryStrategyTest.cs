using MediatR;

using Moq;

using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.Categories.Queries.GetById.Statuses;
using AnSumeri.API.Application.Models.Strategies;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Tests.Strategies;

using AnSumeri.API.Application.Models.Mediatr;

public class MoveNestedCategoryStrategyTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    [Fact]
    public async Task TestMoveNestedCategoryStrategy_WhenCategoryExist_ThenMoved()
    {
        int id = 2;
        int moveToCategoryId = 3;
        string userId = Guid.NewGuid().ToString();
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            new(new GetCategoryByIdSuccess(id), CreateTestCategoryResult(id, moveToCategoryId));
        ResultDto<GetCategoryByIdResult> parentCategoryResponse =
            new(new GetCategoryByIdSuccess(moveToCategoryId), CreateTestCategoryResult(moveToCategoryId, null));

        SetupMocks(id, moveToCategoryId, categoryToMoveResponse, parentCategoryResponse);

        MoveNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await strategy.Move(id, userId, moveToCategoryId);

        VerifyMoveSuccessMocks(id, moveToCategoryId);
    }

    [Fact]
    public async Task TestMoveNestedCategoryStrategy_WhenCategoryNotExist_ThenArgumentOutOfRangeException()
    {
        int id = 2;
        int moveToCategoryId = 3;
        string userId = Guid.NewGuid().ToString();
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            new(new GetCategoryByIdSuccess(id), CreateTestCategoryResult(id, moveToCategoryId));
        ResultDto<GetCategoryByIdResult> parentCategoryResponse =
            new(new GetCategoryByIdSuccess(moveToCategoryId), CreateTestCategoryResult(moveToCategoryId, null));

        SetupMocks(id, moveToCategoryId, categoryToMoveResponse, parentCategoryResponse);

        MoveNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await strategy.Move(id, userId, moveToCategoryId);

        VerifyMoveSuccessMocks(id, moveToCategoryId);
    }

    [Fact]
    public async Task TestMoveNestedCategoryStrategy_WhenBothCategoriesNotExist_ThenMoved()
    {
        int id = 2;
        int moveToCategoryId = 3;
        string userId = Guid.NewGuid().ToString();
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            new(new GetCategoryByIdNotFound(id), null);
        ResultDto<GetCategoryByIdResult> parentCategoryResponse =
            new(new GetCategoryByIdSuccess(id), null);

        SetupMocks(id, moveToCategoryId, categoryToMoveResponse, parentCategoryResponse);

        MoveNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => strategy.Move(id, userId, moveToCategoryId));

        VerifyMoveCategoriesNotFoundMocks(id, moveToCategoryId);
    }

    private void VerifyMoveCategoriesNotFoundMocks(int id, int moveToCategoryId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == moveToCategoryId), It.IsAny<CancellationToken>()),
                Times.Once);

        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Never);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == moveToCategoryId)),
                Times.Never);
    }

    private void VerifyMoveSuccessMocks(int id, int moveToCategoryId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == moveToCategoryId), It.IsAny<CancellationToken>()),
                Times.Once);

        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Once);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == moveToCategoryId)),
                Times.Once);
    }

    private void SetupMocks(int id, int parentId, ResultDto<GetCategoryByIdResult> categoryToMoveResponse,
        ResultDto<GetCategoryByIdResult> parentCategoryResponse)
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryToMoveResponse);
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategoryById>(x => x.Id == parentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(parentCategoryResponse);

        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)))
            .Returns(Task.CompletedTask);
        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.Is<Category>(x => x.Id == parentId)))
            .Returns(Task.CompletedTask);
    }

    private GetCategoryByIdResult CreateTestCategoryResult(int id, int? parentId) =>
        new()
        {
            Id = id,
            Name = "testCategory",
            ParentId = parentId
        };
}
