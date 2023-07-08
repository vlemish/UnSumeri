using MediatR;
using Moq;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Factories;

public class CategoryMoveStrategyFactoryTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    [Theory]
    [InlineData(2, 3, null)]
    [InlineData(2, 3, 4)]
    [InlineData(2, null, 4)]
    public void
        TestCreateCategoryMoveStrategy_WhenCategoryParentNotSameAsMoveToId_ThenMoveNotNestedCategoryStrategyCreated(
            int id, int? parentId, int? moveToId)
    {
        Category categoryToMove = CreateTestCategory(id, parentId);

        SetupMocks();
        CategoryMoveStrategyFactory factory = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        ICategoryMoveStrategy actual = factory.CreateCategoryMoveStrategy(categoryToMove, moveToId);

        Assert.NotNull(actual);
        Assert.Equal(typeof(MoveNotNestedCategoryStrategy), actual.GetType());
        Assert.NotEqual(typeof(MoveNestedCategoryStrategy), actual.GetType());
    }

    [Fact]
    public void
        TestCreateCategoryMoveStrategy_WhenCategoryParentSameAsMoveToId_ThenMoveNestedCategoryStrategyCreated()
    {
        int id = 2;
        int parentId = 3;
        int moveToId = parentId;

        Category categoryToMove = CreateTestCategory(id, parentId);

        SetupMocks();
        CategoryMoveStrategyFactory factory = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        ICategoryMoveStrategy actual = factory.CreateCategoryMoveStrategy(categoryToMove, moveToId);

        Assert.NotNull(actual);
        Assert.Equal(typeof(MoveNestedCategoryStrategy), actual.GetType());
        Assert.NotEqual(typeof(MoveNotNestedCategoryStrategy), actual.GetType());
    }

    private Category CreateTestCategory(int id, int? parentId) =>
        new()
        {
            Id = id,
            Name = "testCategory",
            ParentId = parentId,
        };

    private void SetupMocks()
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();
    }
}