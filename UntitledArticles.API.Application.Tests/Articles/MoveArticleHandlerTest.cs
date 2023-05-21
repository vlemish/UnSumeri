namespace UntitledArticles.API.Application.Tests.Articles;

using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;
using MediatR;
using Moq;
using UntitiledArticles.API.Application.Articles.Commands.Move;
using UntitiledArticles.API.Application.Articles.Queries.GetOneById;
using UntitiledArticles.API.Application.Articles.Queries.GetOneById.Statuses;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;
using UntitiledArticles.API.Application.Models;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;

public class MoveArticleHandlerTest
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IArticleRepository> _articleRepositoryMock;

    private MoveArticleHandler _handler;

    [Fact]
    public async Task TestMoveArticleHandler_WhenCategoryAndArticleExist_ThenSuccess()
    {
        int id = 1;
        int categoryId = 1;

        int categoryMoveToId = 2;

        ResultDto<GetCategoryByIdResult> expectedGetCategoryByIdResult = new(new GetCategoryByIdSuccess(categoryId),
            CreateTestGetCategoryByIdResult(categoryId));
        ResultDto<ArticleDto> expectedGetOneArticleByIdResult =
            new(new GetOneArticleByIdSuccess(id), CreateTestArticleDto(id, categoryId));
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.OK;

        SetupMocks(expectedGetCategoryByIdResult, expectedGetOneArticleByIdResult);

        this._handler = new(this._mediatorMock.Object, this._articleRepositoryMock.Object);

        ResultDto result = await this._handler.Handle(new MoveArticle(id, categoryMoveToId), default);

        Assert.Equal(expectedOperationStatusValue, result.OperationStatus.Status);
        VerifyMocks(expectedGetCategoryByIdTimesCalled: Times.Once(),
            expectedGetOneArticleByIdTimesCalled: Times.Once(), expectedArticleRepositoryTimesCalled: Times.Once());
    }

    [Fact]
    public async Task TestMoveArticleHandler_WhenArticleNotExist_ThenNotFound()
    {
        int id = 1;
        int categoryId = 1;

        int categoryMoveToId = 2;

        ResultDto<ArticleDto> expectedGetOneArticleByIdResult =
            new(new GetOneByIdArticleNotFound(id), null);
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;

        SetupMocks(null, expectedGetOneArticleByIdResult);

        this._handler = new(this._mediatorMock.Object, this._articleRepositoryMock.Object);

        ResultDto result = await this._handler.Handle(new MoveArticle(id, categoryMoveToId), default);

        Assert.Equal(expectedOperationStatusValue, result.OperationStatus.Status);
        VerifyMocks(expectedGetCategoryByIdTimesCalled: Times.Never(),
            expectedGetOneArticleByIdTimesCalled: Times.Once(), expectedArticleRepositoryTimesCalled: Times.Never());
    }

    [Fact]
    public async Task TestMoveArticleHandler_WhenCategoryToMoveToNotExist_ThenNotFound()
    {
        int id = 1;
        int categoryId = 1;

        int categoryMoveToId = 2;

        ResultDto<GetCategoryByIdResult> expectedGetCategoryByIdResult = new(new GetCategoryByIdNotFound(categoryId),
            null);
        ResultDto<ArticleDto> expectedGetOneArticleByIdResult =
            new(new GetOneArticleByIdSuccess(id), CreateTestArticleDto(id, categoryId));
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotFound;

        SetupMocks(expectedGetCategoryByIdResult, expectedGetOneArticleByIdResult);

        this._handler = new(this._mediatorMock.Object, this._articleRepositoryMock.Object);

        ResultDto result = await this._handler.Handle(new MoveArticle(id, categoryMoveToId), default);

        Assert.Equal(expectedOperationStatusValue, result.OperationStatus.Status);
        VerifyMocks(expectedGetCategoryByIdTimesCalled: Times.Once(),
            expectedGetOneArticleByIdTimesCalled: Times.Once(), expectedArticleRepositoryTimesCalled: Times.Never());
    }

    [Fact]
    public async Task TestMoveArticleHandler_WhenCategoryToMoveSameAsCurrentCategory_ThenNotModified()
    {
        int id = 1;
        int categoryId = 1;

        int categoryMoveToId = 1;

        ResultDto<GetCategoryByIdResult> expectedGetCategoryByIdResult = new(new GetCategoryByIdNotFound(categoryId),
            null);
        ResultDto<ArticleDto> expectedGetOneArticleByIdResult =
            new(new GetOneArticleByIdSuccess(id), CreateTestArticleDto(id, categoryId));
        OperationStatusValue expectedOperationStatusValue = OperationStatusValue.NotModified;

        SetupMocks(expectedGetCategoryByIdResult, expectedGetOneArticleByIdResult);

        this._handler = new(this._mediatorMock.Object, this._articleRepositoryMock.Object);

        ResultDto result = await this._handler.Handle(new MoveArticle(id, categoryMoveToId), default);

        Assert.Equal(expectedOperationStatusValue, result.OperationStatus.Status);
        VerifyMocks(expectedGetCategoryByIdTimesCalled: Times.Never(),
            expectedGetOneArticleByIdTimesCalled: Times.Once(), expectedArticleRepositoryTimesCalled: Times.Never());
    }

    private void VerifyMocks(Times expectedGetCategoryByIdTimesCalled, Times expectedGetOneArticleByIdTimesCalled, Times expectedArticleRepositoryTimesCalled)
    {
        this._mediatorMock.Verify(m => m.Send(It.IsAny<GetCategoryById>(), It.IsAny<CancellationToken>()),
            expectedGetCategoryByIdTimesCalled);
        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()),
                expectedGetOneArticleByIdTimesCalled);
        this._articleRepositoryMock
            .Verify(m => m.UpdateAsync(It.IsAny<Article>()), expectedArticleRepositoryTimesCalled);
    }

    private void SetupMocks(ResultDto<GetCategoryByIdResult> expectedGetCategoryByIdResult,
        ResultDto<ArticleDto> expectedGetOneArticleByIdResult)
    {
        this._mediatorMock = new();
        this._articleRepositoryMock = new();

        this._mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoryById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetCategoryByIdResult);
        this._mediatorMock
            .Setup(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetOneArticleByIdResult);

        this._articleRepositoryMock
            .Setup(m => m.UpdateAsync(It.IsAny<Article>()))
            .Returns(Task.CompletedTask);
    }

    private ArticleDto CreateTestArticleDto(int id, int categoryId) =>
        new(id, "TestTitle", "TestContent", DateTime.UtcNow, categoryId);

    private GetCategoryByIdResult CreateTestGetCategoryByIdResult(int categoryId) =>
        new() { Id = categoryId, Name = "TestCategory", ParentId = 2, };
}
