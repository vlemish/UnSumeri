namespace UntitledArticles.API.Application.Tests.Articles;

using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Moq;
using UntitiledArticles.API.Application.Articles.Commands.Delete;
using UntitiledArticles.API.Application.Articles.Queries.GetOneById;
using UntitiledArticles.API.Application.Articles.Queries.GetOneById.Statuses;
using UntitiledArticles.API.Application.Models;
using UntitiledArticles.API.Application.Models.Mediatr;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitiledArticles.API.Application.OperationStatuses.Shared.Articles;

public class DeleteArticleHandlerTest
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IArticleRepository> _articleRepositoryMock;

    private DeleteArticleHandler _handler;

    [Fact]
    public async Task TestDeleteArticleHandler_WhenArticleExist_ThenSuccess()
    {
        int id = 2;
        DeleteArticle request = new(id);
        ResultDto<ArticleDto> expetectedArticleResultDto = new ResultDto<ArticleDto>(new GetOneArticleByIdSuccess(id),
            new ArticleDto(id, "title", "content", DateTime.UtcNow, 3));

        SetupMocks(expetectedArticleResultDto, id);

        this._handler = new(this._articleRepositoryMock.Object, this._mediatorMock.Object);

        ResultDto result = await this._handler.Handle(request, default);

        Assert.Equal(OperationStatusValue.OK, result.OperationStatus.Status);
        VerifyMocks(Times.Once(), Times.Once());
    }

    [Fact]
    public async Task TestDeleteArticleHandler_WhenArticleNotExist_ThenNotFound()
    {
        int id = 2;
        DeleteArticle request = new(id);
        ResultDto<ArticleDto> expetectedArticleResultDto = new ResultDto<ArticleDto>(new ArticleNotFound(id),
            null);

        SetupMocks(expetectedArticleResultDto, id);

        this._handler = new(this._articleRepositoryMock.Object, this._mediatorMock.Object);

        ResultDto result = await this._handler.Handle(request, default);

        Assert.Equal(OperationStatusValue.NotFound, result.OperationStatus.Status);
        VerifyMocks(Times.Never(), Times.Once());
    }

    private void VerifyMocks(Times expectedArticleRepositoryTimesCalled, Times expectedMediatorTimesCalled)
    {
        this._mediatorMock
            .Verify(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()),
                expectedMediatorTimesCalled);
        this._articleRepositoryMock
            .Verify(m => m.DeleteAsync(It.IsAny<Article>()), expectedArticleRepositoryTimesCalled);
    }

    private void SetupMocks(ResultDto<ArticleDto> expectedGetOneArticleByIdResponse, int id)
    {
        this._mediatorMock = new();
        this._articleRepositoryMock = new();

        this._mediatorMock
            .Setup(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetOneArticleByIdResponse);
        this._articleRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Article>()))
            .ReturnsAsync(id);
    }
}
