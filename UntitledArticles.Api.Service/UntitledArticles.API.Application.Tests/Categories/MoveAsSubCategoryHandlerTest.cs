using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

using UntitiledArticles.API.Application.Models.Mediatr;

public class MoveAsSubCategoryHandlerTest
{
    private Mock<ICategoryMoveStrategyFactory> _categoryMoveStrategyFactoryMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ICategoryMoveStrategy> _categoryMoveStrategyMock;

    [Fact]
    public async Task TestMoveAsSubCategoryHandler_WhenCategoriesExist_ThenMoveSuccessStatus()
    {
        int id = 2;
        int parentId = 3;
        MoveAsSubCategory request = new(id, Guid.NewGuid().ToString(), parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.OkNoContent;
        ResultDto<GetCategoryByIdResult> expectedCategoryResponse =
            new(new GetCategoryByIdSuccess(id), GetTestCategoryResult(id));
        ResultDto<GetCategoryByIdResult> expectedParentCategoryResponse =
            new(new GetCategoryByIdSuccess(parentId), GetTestCategoryResult(parentId));

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(_categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.OperationStatus.Status);
        VerifyMoveSuccessMocks(id, parentId);
    }

    [Fact]
    public async Task TestMoveAsSubCategoryHandler_WhenCategoryNotExist_ThenNotFoundStatus()
    {
        int id = 2;
        int parentId = 3;
        MoveAsSubCategory request = new(id, Guid.NewGuid().ToString(), parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;
        ResultDto<GetCategoryByIdResult> expectedCategoryResponse =
            new(new GetCategoryByIdNotFound(id), null);
        ResultDto<GetCategoryByIdResult> expectedParentCategoryResponse = null;

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(_categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.OperationStatus.Status);
        VerifyCategoryNotFoundMocks(id, parentId);
    }

    [Fact]
    public async Task TestMoveAsSubCategoryHandler_WhenParentCategoryNotExist_ThenNotFoundStatus()
    {
        int id = 2;
        int parentId = 3;
        MoveAsSubCategory request = new(id,Guid.NewGuid().ToString(), parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.ParentNotExists;
        ResultDto<GetCategoryByIdResult> expectedCategoryResponse =
            new(new GetCategoryByIdSuccess(id), GetTestCategoryResult(id));
        ResultDto<GetCategoryByIdResult> expectedParentCategoryResponse =
            new(new GetCategoryByIdNotFound(parentId), null);

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(_categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.OperationStatus.Status);
        VerifyParentCategoryNotFoundMocks(id, parentId);
    }

    private GetCategoryByIdResult GetTestCategoryResult(int id) =>
        new() { Name = "testname", Id = id, };

    private void VerifyCategoryNotFoundMocks(int id, int parentId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
                Times.Never());
        _categoryMoveStrategyFactoryMock
            .Verify(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()),
                Times.Never());
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()),
                Times.Never());
    }

    private void VerifyParentCategoryNotFoundMocks(int id, int parentId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryMoveStrategyFactoryMock
            .Verify(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()),
                Times.Never());
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()),
                Times.Never());
    }

    private void VerifyMoveSuccessMocks(int id, int parentId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategoryById>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryMoveStrategyFactoryMock
            .Verify(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()),
                Times.Once());
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()),
                Times.Once());
    }

    private void SetupMocks(int id, int parentId, ResultDto<GetCategoryByIdResult> expectedCategoryResponse,
        ResultDto<GetCategoryByIdResult> expectedParentCategoryResponse)
    {
        _mediatorMock = new();
        _categoryMoveStrategyFactoryMock = new();
        _categoryMoveStrategyMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategoryById>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCategoryResponse);
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategoryById>(x => x.Id == parentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedParentCategoryResponse);
        _categoryMoveStrategyFactoryMock
            .Setup(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()))
            .Returns(_categoryMoveStrategyMock.Object);
        _categoryMoveStrategyMock
            .Setup(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()))
            .Returns(Task.CompletedTask);
    }
}
