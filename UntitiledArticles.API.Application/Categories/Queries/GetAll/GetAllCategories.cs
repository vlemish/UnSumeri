using MediatR;
using UntitledArticles.API.Domain.Enums;
using UntitledArticles.API.Domain.Pagination;

namespace UntitiledArticles.API.Application.Categories.Queries.GetAll;

public record GetAllCategories : IRequest<GetAllCategoriesResponse>
{
    public GetAllCategories(LoadOptions loadOptions, OrderByOption? orderByOption)
    {
        LoadOptions = loadOptions;
        OrderByOption = orderByOption ?? UntitledArticles.API.Domain.Enums.OrderByOption.ASC;
    }
    
    public LoadOptions LoadOptions { get; init; } = new();

    public OrderByOption OrderByOption { get; init; }
}