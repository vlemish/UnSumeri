namespace AnSumeri.API.Application.OperationStatuses.Shared.Categories;

public class DuplicateCategory : IOperationStatus
{
    private readonly string _name;

    public DuplicateCategory(string name)
    {
        _name = name;
    }

    public OperationStatusValue Status => OperationStatusValue.Duplicate;
    public string Message => $"Category with name = {_name} already exists!";
}
