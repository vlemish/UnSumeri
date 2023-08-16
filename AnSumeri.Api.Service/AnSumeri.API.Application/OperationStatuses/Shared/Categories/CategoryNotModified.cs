namespace AnSumeri.API.Application.OperationStatuses.Shared.Categories;

public class CategoryNotModified : IOperationStatus
{
    private readonly int _id;
    private readonly string _userId;
    private readonly string _name;

    public CategoryNotModified(int id, string userId, string name)
    {
        _id = id;
        _userId = userId;
        _name = name;
    }

    public OperationStatusValue Status => OperationStatusValue.NotModified;
    public string Message => $"Category Not Modified where Id = {_id}, userId = {_userId} and name = {_name}";
}
