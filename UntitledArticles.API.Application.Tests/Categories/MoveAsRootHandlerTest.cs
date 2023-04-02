﻿using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using UntitiledArticles.API.Application.Categories.Commands.MoveAsRoot;
using UntitiledArticles.API.Application.Categories.Queries;
using UntitiledArticles.API.Application.Categories.Queries.Statuses;
using UntitiledArticles.API.Application.Models.Factories;
using UntitiledArticles.API.Application.Models.Strategies;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Categories;

public class MoveAsRootHandlerTest
{
    private Mock<ICategoryMoveStrategyFactory> _categoryMoveStrategyFactoryMock;
    private Mock<IMediator> _mediatorMock;
    private Mock<ICategoryMoveStrategy> _categoryMoveStrategyMock;

    [Fact]
    public async Task TestMoveAsRootHandler_WhenParticipantNotFound_ThenNotFoundStatus()
    {
        int id = 2;
        MoveAsRoot request = new(id);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;
        GetCategoryResponse expectedGetCategoryResponse = new(new GetCategoryNotFound(id), null);

        SetupMocks(id, expectedGetCategoryResponse);

        MoveAsRootHandler handler = new(new Mock<ILogger<MoveAsRootHandler>>().Object,
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);

        MoveAsRootResponse actualResponse = await handler.Handle(request, default);

        Assert.NotNull(actualResponse);
        Assert.Equal(expectedOperationStatusValue, actualResponse.Status.Status);
        VerifyMocks(mediatorTimesCalled: Times.Once(), categoryMoveStrategyFactoryTimesCalled: Times.Never(),
            categoryMoveStrategyTimesCalled: Times.Never());
    }

    [Fact]
    public async Task TestMoveAsRoot_WhenParticipantFound_ThenSuccessStatus()
    {
        int id = 2;
        MoveAsRoot request = new(id);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.OK;
        GetCategoryResponse expectedGetCategoryResponse = new(new GetCategorySuccess(id),
            new GetCategoryResult() { Name = "name", Id = id, ParentId = 3 });

        SetupMocks(id, expectedGetCategoryResponse);

        MoveAsRootHandler handler = new(new Mock<ILogger<MoveAsRootHandler>>().Object,
            _categoryMoveStrategyFactoryMock.Object, _mediatorMock.Object);

        MoveAsRootResponse actualResponse = await handler.Handle(request, default);

        Assert.NotNull(actualResponse);
        Assert.Equal(expectedOperationStatusValue, actualResponse.Status.Status);
        VerifyMocks(mediatorTimesCalled: Times.Once(), categoryMoveStrategyFactoryTimesCalled: Times.Once(),
            categoryMoveStrategyTimesCalled: Times.Once());
    }

    private void VerifyMocks(Times mediatorTimesCalled, Times categoryMoveStrategyFactoryTimesCalled,
        Times categoryMoveStrategyTimesCalled)
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<GetCategory>(),
                It.IsAny<CancellationToken>()), mediatorTimesCalled);
        _categoryMoveStrategyFactoryMock
            .Verify(m =>
                    m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()),
                categoryMoveStrategyFactoryTimesCalled);
        _categoryMoveStrategyMock
            .Verify(m => m.Move(It.IsAny<int>(), It.IsAny<int?>()), categoryMoveStrategyTimesCalled);
    }

    private void SetupMocks(int id, GetCategoryResponse expectedGetCategoryResponse)
    {
        _mediatorMock = new();
        _categoryMoveStrategyFactoryMock = new();
        _categoryMoveStrategyMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategory>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetCategoryResponse);
        _categoryMoveStrategyFactoryMock
            .Setup(m => m.CreateCategoryMoveStrategy(It.IsAny<Category>(), It.IsAny<int?>()))
            .Returns(_categoryMoveStrategyMock.Object);
        _categoryMoveStrategyMock
            .Setup(m => m.Move(It.Is<int>(x => x == id), It.IsAny<int?>()))
            .Returns(Task.CompletedTask);
    }
}