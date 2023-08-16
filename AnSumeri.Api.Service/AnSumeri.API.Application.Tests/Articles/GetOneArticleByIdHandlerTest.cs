namespace AnSumeri.API.Application.Tests.Articles;

using System.Linq.Expressions;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Moq;
using Service.Mappings;
using AnSumeri.API.Application.Articles.Queries.GetOneById;
using AnSumeri.API.Application.Models;
using AnSumeri.API.Application.Models.Mediatr;
using AnSumeri.API.Application.OperationStatuses;

public class GetOneArticleByIdHandlerTest
{
    private Mock<IArticleRepository> _articleRepositoryMock;
    private IMapper _mapper;

    private GetOneArticleByIdHandler _handler;

    [Fact]
    public async Task TestGetOneArticleByIdHandler_WhenArticleExists_ThenSuccess()
    {
        int id = 1;
        Article expectedArticle = this.GetTestArticle(id);
        OperationStatusValue expectedOperationStatus = OperationStatusValue.OK;

        SetupMocks(expectedArticle);

        this._handler = new(this._articleRepositoryMock.Object, this._mapper);

        ResultDto<ArticleDto> result = await this._handler.Handle(new GetOneArticleById(id, Guid.NewGuid().ToString()), default);

        Assert.NotNull(result);
        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Equal(id, result.Payload.Id);
    }

    [Fact]
    public async Task TestGetOneArticleByIdHandler_WhenArticleNotExists_ThenNotFound()
    {
        int id = 1;
        Article expectedArticle = this.GetTestArticle(id);
        OperationStatusValue expectedOperationStatus = OperationStatusValue.NotFound;

        SetupMocks(null);

        this._handler = new(this._articleRepositoryMock.Object, this._mapper);

        ResultDto<ArticleDto> result = await this._handler.Handle(new GetOneArticleById(id, Guid.NewGuid().ToString()), default);

        Assert.NotNull(result);
        Assert.Equal(expectedOperationStatus, result.OperationStatus.Status);
        Assert.Null(result.Payload);
    }

    private void SetupMocks(Article expectedArticle)
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new CategoryMappings())));
        this._articleRepositoryMock = new();
        this._articleRepositoryMock
            .Setup(m => m.GetOneByFilter(It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync(expectedArticle);
    }

    private Article GetTestArticle(int id) =>
        new()
        {
            Id = id, CategoryId = 2, Title = "TestTitle", Content = "TestContent",
        };
}
