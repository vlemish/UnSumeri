using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.Statuses;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

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
        MoveAsSubCategory request = new(id, parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.OK;
        GetCategoryResponse expectedCategoryResponse =
            new GetCategoryResponse(new GetCategorySuccess(id), GetTestCategoryResult(id));
        GetCategoryResponse expectedParentCategoryResponse = new(new GetCategorySuccess(parentId), GetTestCategoryResult(parentId));

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(new Mock<ILogger<MoveAsSubCategoryHandler>>().Object,
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        MoveAsSubCategoryResponse actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.Status.Status);
        VerifyMoveSuccessMocks(id, parentId);
    }
    
    [Fact]
    public async Task TestMoveAsSubCategoryHandler_WhenCategoryNotExist_ThenNotFoundStatus()
    {
        int id = 2;
        int parentId = 3;
        MoveAsSubCategory request = new(id, parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;
        GetCategoryResponse expectedCategoryResponse = new GetCategoryResponse(new GetCategoryNotFound(id), null);
        GetCategoryResponse expectedParentCategoryResponse = null;

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(new Mock<ILogger<MoveAsSubCategoryHandler>>().Object,
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        MoveAsSubCategoryResponse actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.Status.Status);
        VerifyCategoryNotFoundMocks(id, parentId);
    }

    [Fact]
    public async Task TestMoveAsSubCategoryHandler_WhenParentCategoryNotExist_ThenNotFoundStatus()
    {
        int id = 2;
        int parentId = 3;
        MoveAsSubCategory request = new(id, parentId);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.ParentNotExists;
        GetCategoryResponse expectedCategoryResponse =
            new GetCategoryResponse(new GetCategorySuccess(id), GetTestCategoryResult(id));
        GetCategoryResponse expectedParentCategoryResponse =
            new GetCategoryResponse(new GetCategoryNotFound(parentId), null);

        SetupMocks(id, parentId, expectedCategoryResponse, expectedParentCategoryResponse);

        MoveAsSubCategoryHandler handler = new(new Mock<ILogger<MoveAsSubCategoryHandler>>().Object,
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);
        MoveAsSubCategoryResponse actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatusValue, actual.Status.Status);
        VerifyParentCategoryNotFoundMocks(id, parentId);
    }

    private GetCategoryResult GetTestCategoryResult(int id) =>
        new()
        {
            Name = "testname",
            Id = id,
        };

    private void VerifyCategoryNotFoundMocks(int id, int parentId)
    {
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
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
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
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
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()),
                Times.Once());
        _mediatorMock
            .Verify(m => m.Send(It.Is<GetCategory>(x => x.Id == parentId), It.IsAny<CancellationToken>()),
                Times.Once());
        _categoryMoveStrategyFactoryMock
            .Verify(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()),
                Times.Once());
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()),
                Times.Once());
    }

    private void SetupMocks(int id, int parentId, GetCategoryResponse expectedCategoryResponse,
        GetCategoryResponse expectedParentCategoryResponse)
    {
        _mediatorMock = new();
        _categoryMoveStrategyFactoryMock = new();
        _categoryMoveStrategyMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategory>(x => x.Id == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCategoryResponse);
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCategory>(x => x.Id == parentId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedParentCategoryResponse);
        _categoryMoveStrategyFactoryMock
            .Setup(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()))
            .Returns(_categoryMoveStrategyMock.Object);
        _categoryMoveStrategyMock
            .Setup(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()))
            .Returns(Task.CompletedTask);
    }
}