using MediatR;
using AnSumeri.API.Domain.Enums;
using AnSumeri.API.Domain.Pagination;

namespace AnSumeri.API.Application.Categories.Queries.GetAll;

using Models.Mediatr;
using Domain.Contracts;

public record GetAllCategories : IRequest<ResultDto<IPaginatedResult<GetAllCategoriesResult>>>
{
    public GetAllCategories(LoadOptions loadOptions, string userId, OrderByOption? orderByOption, int? depth)
    {
        LoadOptions = loadOptions;
        OrderByOption = orderByOption ?? Domain.Enums.OrderByOption.ASC;
        Depth = depth ?? 2;
        UserId = userId;
    }

    public LoadOptions LoadOptions { get; init; } = new();

    public OrderByOption OrderByOption { get; init; }

    public int Depth {get; init; }

    public string UserId { get; set; }
}
