using AnSumeri.API.Application.Articles.Commands.Update;
using AnSumeri.API.Application.Articles.Commands.Update.Statuses;
using AnSumeri.API.Application.Articles.Queries.GetOneById;
using AnSumeri.API.Application.Models;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Application.OperationStatuses.Shared.Articles;
using AnSumeri.API.Domain.Contracts;
using AnSumeri.API.Domain.Entities;
using MediatR;
using Moq;

namespace AnSumeri.API.Application.Tests.Articles;

public class UpdateArticleHandlerTest
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IArticleRepository> _articleRepositoryMock;

    [Theory]
    [InlineData("UpdatedTitle", "UpdatedContent")]
    [InlineData("UpdatedTitle", "Content")]
    [InlineData("Title", "UpdatedContent")]
    public async Task TestUpdateArticleHandler_WhenArticleExistsAndUpdated_ThenSuccess(string title, string content)
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();

        OperationStatusValue expectedOperationStatus = OperationStatusValue.NoContent;
        ResultDto<ArticleDto> expectedResultDto =
            new ResultDto<ArticleDto>(new UpdateArticleSuccess(id), CreateTestArticleDto(id));
        UpdateArticle request = new(id, userId, title, content);

        SetupMocks(expectedResultDto);

        UpdateArticleHandler handler = new(_mediatorMock.Object, _articleRepositoryMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);
        VerifyMocks(Times.Once());
    }

    [Fact]
    public async Task TestUpdateArticleHandler_WhenArticleNotExist_ThenNotFound()
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();
        string title = "UpdatedTitle";
        string content = "UpdatedContent";

        OperationStatusValue expectedOperationStatus = OperationStatusValue.NotFound;
        ResultDto<ArticleDto> expectedResultDto =
            new ResultDto<ArticleDto>(new ArticleNotFound(id), null);
        UpdateArticle request = new(id, userId, title, content);

        SetupMocks(expectedResultDto);

        UpdateArticleHandler handler = new(_mediatorMock.Object, _articleRepositoryMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);
        VerifyMocks(Times.Never());
    }

    [Theory]
    [InlineData("Title", "Content")]
    public async Task TestUpdateArticleHandler_WhenArticleExistsAndNotUpdated_ThenArticleNotModified(string title, string content)
    {
        int id = 2;
        string userId = Guid.NewGuid().ToString();

        OperationStatusValue expectedOperationStatus = OperationStatusValue.NotModified;
        ResultDto<ArticleDto> expectedResultDto =
            new ResultDto<ArticleDto>(new ArticleNotChanged(id), CreateTestArticleDto(id));
        UpdateArticle request = new(id, userId, title, content);

        SetupMocks(expectedResultDto);

        UpdateArticleHandler handler = new(_mediatorMock.Object, _articleRepositoryMock.Object);

        ResultDto actual = await handler.Handle(request, default);

        Assert.NotNull(actual);
        Assert.Equal(expectedOperationStatus, actual.OperationStatus.Status);
        VerifyMocks(Times.Never());
    }

    private void SetupMocks(ResultDto<ArticleDto> expectedGetArticleResult)
    {
        _mediatorMock = new();
        _articleRepositoryMock = new();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetArticleResult);
        _articleRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Article>()))
            .Returns(Task.CompletedTask);
    }

    private void VerifyMocks(Times articleRepositoryTimesCalled)
    {
        _mediatorMock
            .Verify(m => m.Send(It.IsAny<GetOneArticleById>(), It.IsAny<CancellationToken>()), Times.Once());
        _articleRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<Article>()))
            .Returns(Task.CompletedTask);
    }

    private ArticleDto CreateTestArticleDto(int id) =>
        new(id, "Title", "Content", DateTime.Now, 3);
}
