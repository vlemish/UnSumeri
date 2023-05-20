using MediatR;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

using Models.Mediatr;
using UntitledArticles.API.Domain.Contracts;

public record GetAllCategories : IRequest<ResultDto<IPaginatedResult<GetAllCategoriesResult>>>
{
    public GetAllCategories(LoadOptions loadOptions, OrderByOption? orderByOption, int? depth)
    {
        LoadOptions = loadOptions;
        OrderByOption = orderByOption ?? UntitledArticles.API.Domain.Enums.OrderByOption.ASC;
        Depth = depth ?? 2;
    }

    public LoadOptions LoadOptions { get; init; } = new();

    public OrderByOption OrderByOption { get; init; }

    public int Depth {get; init; }
}
