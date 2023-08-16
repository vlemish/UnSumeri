using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using AnSumeri.API.Application.Categories.Commands.MoveAsRoot;
using AnSumeri.API.Application.Categories.Queries.GetById;
using AnSumeri.API.Application.Categories.Queries.GetById.Statuses;
using AnSumeri.API.Application.Models.Factories;
using AnSumeri.API.Application.Models.Strategies;
using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Tests.Categories;

using AnSumeri.API.Application.Models.Mediatr;

public class MoveAsRootHandlerTest
{
    private Mock<ICategoryMoveStrategyFactory> _categoryMoveStrategyFactoryMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ICategoryMoveStrategy> _categoryMoveStrategyMock;

    [Fact]
    public async Task TestMoveAsRootHandler_WhenParticipantNotFound_ThenNotFoundStatus()
    {
        int id = 2;
        MoveAsRoot request = new(id, Guid.NewGuid().ToString());
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;
        ResultDto<GetCategoryByIdResult> expectedGetCategoryResponse = new(new GetCategoryByIdNotFound(id), null);

        SetupMocks(id, expectedGetCategoryResponse);

        MoveAsRootHandler handler = new(_categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);

        ResultDto actualResponse = await handler.Handle(request, default);

        Assert.NotNull(actualResponse);
        Assert.Equal(expectedOperationStatusValue, actualResponse.OperationStatus.Status);
        VerifyMocks(mediatorTimesCalled: Times.Once(), categoryMoveStrategyFactoryTimesCalled: Times.Never(),
            categoryMoveStrategyTimesCalled: Times.Never());
    }

    [Fact]
    public async Task TestMoveAsRoot_WhenParticipantFound_ThenSuccessStatus()
    {
        int id = 2;
        MoveAsRoot request = new(id, Guid.NewGuid().ToString());
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NoContent;
        ResultDto<GetCategoryByIdResult> expectedGetCategoryResponse = new(new GetCategoryByIdSuccess(id),
            new GetCategoryByIdResult() { Name = "name", Id = id, ParentId = 3 });

        SetupMocks(id, expectedGetCategoryResponse);

        MoveAsRootHandler handler = new(
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);

        ResultDto actualResponse = await handler.Handle(request, default);

        Assert.NotNull(actualResponse);
        Assert.Equal(expectedOperationStatusValue, actualResponse.OperationStatus.Status);
        VerifyMocks(mediatorTimesCalled: Times.Once(), categoryMoveStrategyFactoryTimesCalled: Times.Once(),
            categoryMoveStrategyTimesCalled: Times.Once());
    }

    private void SetupMocks(int id, ResultDto<GetCategoryByIdResult> expectedGetCategoryResponse)
    {
        _mediatorMock = new();
        _categoryMoveStrategyFactoryMock = new();
        _categoryMoveStrategyMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetCategoryResponse);
        _categoryMoveStrategyFactoryMock
            .Setup(m => m.CreateCategoryMoveStrategy(It.IsAny<int>(), It.IsAny<int?>()))
            .Returns(_categoryMoveStrategyMock.Object);
        _categoryMoveStrategyMock
            .Setup(m => m.Move(It.Is<int>(x => x == id), It.IsAny<string>(), It.IsAny<int?>()))
            .Returns(Task.CompletedTask);
    }

    private void VerifyMocks(Times mediatorTimesCalled, Times categoryMoveStrategyFactoryTimesCalled,
        Times categoryMoveStrategyTimesCalled)
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<GetCategoryById>(),
                It.IsAny<CancellationToken>()), mediatorTimesCalled);
        _categoryMoveStrategyFactoryMock
            .Verify(m =>
                    m.CreateCategoryMoveStrategy(It.IsAny<int>(), It.IsAny<int?>()),
                categoryMoveStrategyFactoryTimesCalled);
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>()), categoryMoveStrategyTimesCalled);
    }
}
