using MediatR;
using Moq;
using AnSumeri.API.Application.Models.Factories;
using AnSumeri.API.Application.Models.Strategies;
using AnSumeri.API.Domain.Contracts;

namespace AnSumeri.API.Application.Tests.Factories;

public class CategoryMoveStrategyFactoryTest
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IMediator> _mediatorMock;

    [Theory]
    [InlineData(2, 3)]
    [InlineData(2,  null)]
    public void
        TestCreateCategoryMoveStrategy_WhenCategoryParentNotSameAsMoveToId_ThenMoveNotNestedCategoryStrategyCreated(
            int categoryToMoveId, int? destinationParentId)
    {
        SetupMocks();
        CategoryMoveStrategyFactory factory = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        ICategoryMoveStrategy actual = factory.CreateCategoryMoveStrategy(categoryToMoveId, destinationParentId);

        Assert.NotNull(actual);
        Assert.Equal(typeof(MoveNotNestedCategoryStrategy), actual.GetType());
        Assert.NotEqual(typeof(MoveNestedCategoryStrategy), actual.GetType());
    }

    [Fact]
    public void
        TestCreateCategoryMoveStrategy_WhenCategoryParentSameAsMoveToId_ThenMoveNestedCategoryStrategyCreated()
    {
        int categoryToMoveId = 2;
        int destinationParentId = 2;

        SetupMocks();
        CategoryMoveStrategyFactory factory = new(_categoryRepositoryMock.Object, _mediatorMock.Object);
        ICategoryMoveStrategy actual = factory.CreateCategoryMoveStrategy(categoryToMoveId, destinationParentId);

        Assert.NotNull(actual);
        Assert.Equal(typeof(MoveNestedCategoryStrategy), actual.GetType());
        Assert.NotEqual(typeof(MoveNotNestedCategoryStrategy), actual.GetType());
    }

    private void SetupMocks()
    {
        _categoryRepositoryMock = new();
        _mediatorMock = new();
    }
}
