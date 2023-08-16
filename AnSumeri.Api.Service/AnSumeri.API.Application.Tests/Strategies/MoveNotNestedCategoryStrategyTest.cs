using MediatR;

using Moq;

using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.Categories.Queries.GetById.Statuses;
using AnSumeri.API.Application.Models.Strategies;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Tests.Strategies;

using AnSumeri.API.Application.Models.Mediatr;

public class MoveNotNestedCategoryStrategyTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    [Fact]
    public async Task TestMoveNotNestedCategoryStrategy_WhenCategoryExist_ThenMoved()
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            new(new GetCategoryByIdSuccess(id), CreateTestCategoryResult(id, null));

        SetupMocks(id, categoryToMoveResponse);

        MoveNotNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await strategy.Move(id, userId,null);

        VerifyMoveSuccessMocks(id);
    }

    [Fact]
    public async Task TestMoveNotNestedCategoryStrategy_WhenCategoryNotExist_ThenArgumentOutOfRangeException()
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();
        ResultDto<GetCategoryByIdResult> categoryToMoveResponse =
            new(new GetCategoryByIdNotFound(id), null);

        SetupMocks(id, categoryToMoveResponse);

        MoveNotNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => strategy.Move(id, userId, null));

        VerifyMoveCategoryNotFoundMocks(id);
    }

    private void VerifyMoveCategoryNotFoundMocks(int id)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Never);
    }

     private void VerifyMoveSuccessMocks(int id)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Once);
    }

     private void SetupMocks(int id, ResultDto<GetCategoryByIdResult> categoryToMoveResponse)
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryToMoveResponse);

        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)))
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
