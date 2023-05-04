using UntitledArticles.API.Domain.Enums;

namespace UntitledArticles.API.Service.Contracts.Requests;

public class GetAllCategoriesRequest
{
    public int? Offset { get; set; } 

    public int? Skip { get; set; }

    public OrderByOption? OrderByOption { get; set; }
}