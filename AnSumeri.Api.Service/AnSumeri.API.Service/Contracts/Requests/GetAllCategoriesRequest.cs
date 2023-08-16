using AnSumeri.API.Domain.Enums;

namespace AnSumeri.API.Service.Contracts.Requests;

public class GetAllCategoriesRequest
{
    public int? Offset { get; set; }

    public int? Skip { get; set; }

    public int? Depth { get; set; } = 2;

    public OrderByOption? OrderByOption { get; set; }
}
