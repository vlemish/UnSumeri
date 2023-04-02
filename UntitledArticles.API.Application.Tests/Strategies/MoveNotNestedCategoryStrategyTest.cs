using MediatR;
using Moq;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.Statuses;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Strategies;

public class MoveNotNestedCategoryStrategyTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    [Fact]
    public async Task TestMoveNotNestedCategoryStrategy_WhenCategoryExist_ThenMoved()
    {
        int id = 2;
        GetCategoryResponse categoryToMoveResponse =
            new GetCategoryResponse(new GetCategorySuccess(id), CreateTestCategoryResult(id, null));
        
        SetupMocks(id, categoryToMoveResponse);
        
        MoveNotNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await strategy.Move(id, null);
        
        VerifyMoveSuccessMocks(id);
    }
    
    [Fact]
    public async Task TestMoveNotNestedCategoryStrategy_WhenCategoryNotExist_ThenArgumentOutOfRangeException()
    {
        int id = 2;
        GetCategoryResponse categoryToMoveResponse =
            new GetCategoryResponse(new GetCategoryNotFound(id), null);
        
        SetupMocks(id, categoryToMoveResponse);
        
        MoveNotNestedCategoryStrategy strategy = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => strategy.Move(id, null));
        
        VerifyMoveCategoryNotFoundMocks(id);
    }

    private void VerifyMoveCategoryNotFoundMocks(int id)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Never);
    }
    
     private void VerifyMoveSuccessMocks(int id)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once);
        _categoryRepositoryMock
            .Verify(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)),
                Times.Once);
    }

     private void SetupMocks(int id, GetCategoryResponse categoryToMoveResponse)
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryToMoveResponse);

        _categoryRepositoryMock
            .Setup(m => m.UpdateAsync(It.Is<Category>(x => x.Id == id)))
            .Returns(Task.CompletedTask);
    }

    private GetCategoryResult CreateTestCategoryResult(int id, int? parentId) =>
        new()
        {
            Id = id,
            Name = "testCategory",
            ParentId = parentId
        };
}