namespace UntitiledArticles.API.Application.OperationStatuses.Shared.Categories;

public class CategoryNotFound : IOperationStatus
{
    private readonly int _categoryId;

    public CategoryNotFound(int categoryId)
    {
        _categoryId = categoryId;
    }

    public OperationStatusValue Status => OperationStatusValue.NotFound;

    public string Message => $"Couldn't add article where Category Id = {_categoryId}. Category doesn't exist!";
}
