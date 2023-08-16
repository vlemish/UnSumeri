using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using AnSumeri.API.Application.Categories.Commands.Move;
using AnSumeri.API.Application.Categories.Commands.Move.Statuses;
using AnSumeri.API.Application.Categories.Commands.MoveAsRoot;
using AnSumeri.API.Application.Categories.Commands.MoveAsSubCategory;
using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Tests.Categories;

using AnSumeri.API.Application.Models.Mediatr;

public class MoveHandlerTest
{
    private Mock<IMediator> _mediatorMock;

    [Fact]
    public async Task TestMoveHandler_WhenMoveToIdNull_ThenCallMoveAsRootCommand()
    {
        int id = 2;
        int? moveToId = null;
        MoveCategory moveCategoryRequest = new MoveCategory(id, Guid.NewGuid().ToString(), moveToId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NoContent;

        SetupMocks(id, moveToId);

        MoveCategoryHandler handler =
            new MoveCategoryHandler(_mediatorMock.Object);
        ResultDto actual = await handler.Handle(moveCategoryRequest, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.OperationStatus.Status);
        VerifyMocks(id, moveToId);
    }

    [Fact]
    public async Task TestMoveHandler_WhenMoveToIdPresent_ThenCallMoveAsRootCommand()
    {
        int id = 2;
        int? moveToId = 3;
        MoveCategory moveCategoryRequest = new MoveCategory(id,Guid.NewGuid().ToString(), moveToId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NoContent;

        SetupMocks(id, moveToId);

        MoveCategoryHandler handler =
            new MoveCategoryHandler(_mediatorMock.Object);
        ResultDto actual = await handler.Handle(moveCategoryRequest, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.OperationStatus.Status);
        VerifyMocks(id, moveToId);
    }

    private void VerifyMocks(int id, int? moveToId)
    {
        Times expectedMoveAsRootTimesCalled = moveToId is null
            ? Times.Once()
            : Times.Never();
        Times expectedMoveAsSubCategoryTimesCalled = moveToId is null
            ? Times.Never()
            : Times.Once();

        _mediatorMock
            .Verify(m => m.Send(It.Is<MoveAsRoot>(x => x.Id == id), It.IsAny<CancellationToken>()),
                expectedMoveAsRootTimesCalled);
        _mediatorMock
            .Verify(m => m.Send(It.Is<MoveAsSubCategory>(x => x.Id == id && x.MoveToId == moveToId),
                It.IsAny<CancellationToken>()), expectedMoveAsSubCategoryTimesCalled);
    }

    private void SetupMocks(int id, int? moveToId)
    {
        _mediatorMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<MoveAsRoot>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto(new MoveCategorySuccess(id, null)));
        _mediatorMock
            .Setup(m => m.Send(It.Is<MoveAsSubCategory>(x => x.Id == id && x.MoveToId == moveToId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultDto(new MoveCategorySuccess(id, moveToId)));
    }
}
