using AutoMapper;

using MediatR;

using Moq;

using UntitiledArticles.API.Application.Articles.Commands.Add;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Contracts;
using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Application.Tests.Articles;

using UntitiledArticles.API.Application.Articles.Commands;
using UntitiledArticles.API.Application.Models;
using UntitiledArticles.API.Application.Models.Mediatr;

public class AddArticleHandlerTest
{
    private Mock<IArticleRepository> _articleRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IMediator> _mediatorMock;

    private AddArticleHandler _handler;

    [Fact]
    public async Task TestAddArticleHandler_WhenCategoryNotExist_ThenNotFoundStatus()
    {
        AddArticle request = GetTestAddArticleRequest();
        ResultDto<GetCategoryByIdResult> expectedGetcategoryByIdResponse = new(new GetCategoryByIdNotFound(1), new GetCategoryByIdResult()
        {
            Id = 1,
            Name = "category",
            SubCategories = new List<GetCategoryByIdResult>(),
            Articles = new List<ArticleDto>() { GetDuplicateArticle() },
        });

        SetupMocks(expectedGetcategoryByIdResponse);

        _handler = new(_articleRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

        ResultDto<AddArticleResult> actualResponse = await _handler.Handle(request, default);

        Assert.Equal(OperationStatusValue.NotFound, actualResponse.OperationStatus.Status);
    }

    [Fact]
    public async Task TestAddArticleArticleHandler_WhenArticleAlreadyExist_ThenDuplicateStatus()
    {
        AddArticle request = GetTestAddArticleRequest();
        ResultDto<GetCategoryByIdResult> expectedGetcategoryByIdResponse = new(new GetCategoryByIdSuccess(1), new GetCategoryByIdResult()
        {
            Id = 1,
            Name = "category",
            SubCategories = new List<GetCategoryByIdResult>(),
            Articles = new List<ArticleDto>() { GetDuplicateArticle() },
        });

        SetupMocks(expectedGetcategoryByIdResponse);

        _handler = new(_articleRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

        ResultDto<AddArticleResult> actualResponse = await _handler.Handle(request, default);

        Assert.Equal(OperationStatusValue.Duplicate, actualResponse.OperationStatus.Status);
    }

    [Fact]
    public async Task TestAddArticleHandler_WhenCategoryExistAndArticleUnique_ThenSuccessStatus()
    {
        AddArticle request = GetTestAddArticleRequest();
        ResultDto<GetCategoryByIdResult> expectedGetcategoryByIdResponse = new(new GetCategoryByIdSuccess(1), new GetCategoryByIdResult()
        {
            Id = 1,
            Name = "category",
            SubCategories = new List<GetCategoryByIdResult>(),
            Articles = new List<ArticleDto>(GetUniqueArticles()),
        });

        SetupMocks(expectedGetcategoryByIdResponse);

        _handler = new(_articleRepositoryMock.Object, _mapperMock.Object, _mediatorMock.Object);

        ResultDto<AddArticleResult> actualResponse = await _handler.Handle(request, default);

        Assert.Equal(OperationStatusValue.OK, actualResponse.OperationStatus.Status);
    }

    private void SetupMocks(ResultDto<GetCategoryByIdResult> expectedGetCategoryByIdResponse)
    {
        _articleRepositoryMock = new();
        _mapperMock = new();
        _mediatorMock = new();

        _articleRepositoryMock
            .Setup(m => m.AddAsync(It.IsAny<Article>()))
            .ReturnsAsync(new Article()
            {
                Id = 2,
                CreatedAtTime = new DateTime(2023, 03, 03, 1, 0, 0),
                CategoryId = 1,
                Title = "title",
                Content = "content",
            });

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetCategoryById>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedGetCategoryByIdResponse);
    }

    private AddArticle GetTestAddArticleRequest() =>
        new(1, "title", "content");

    private ArticleDto GetDuplicateArticle() =>
        new(2, "title", "content", new DateTime(2023, 03, 03, 1, 0, 0), 1);

    private List<ArticleDto> GetUniqueArticles() =>
        new()
        {
            new ArticleDto(3, "title2", "content2", new DateTime(2023, 03, 03, 1, 0, 0), 1),
            new ArticleDto(4, "title3", "content3", new DateTime(2023, 03, 03, 1, 0, 0), 1),
        };
}
