namespace AnSumeri.API.Application.Articles.Queries.GetOneById;

using AutoMapper;
using MediatR;
using Models;
using Models.Mediatr;
using OperationStatuses.Shared.Articles;
using Statuses;
using Domain.Contracts;
using Domain.Entities;

public class GetOneArticleByIdHandler : IRequestHandler<GetOneArticleById, ResultDto<ArticleDto>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public GetOneArticleByIdHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto<ArticleDto>> Handle(GetOneArticleById request, CancellationToken cancellationToken)
    {
        Article article = await this._articleRepository.GetOneByFilter(a => a.Id == request.Id && request.UserId == request.UserId);
        if (article is null)
        {
            return ReportNotFound(request);
        }

        ArticleDto articleDto = this._mapper.Map<ArticleDto>(article);
        return ReportSuccess(request, articleDto);
    }

    private ResultDto<ArticleDto> ReportSuccess(GetOneArticleById request, ArticleDto articleDto) =>
        new(new GetOneArticleByIdSuccess(request.Id), articleDto);

    private ResultDto<ArticleDto> ReportNotFound(GetOneArticleById request) =>
        new(new ArticleNotFound(request.Id), null);
}
