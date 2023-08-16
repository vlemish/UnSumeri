namespace AnSumeri.API.Application.OperationStatuses.Shared.Categories;

public class CategoryNoContent : IOperationStatus
{
    public OperationStatusValue Status => OperationStatusValue.NoContent;
    public string Message => "There were no categories found for specified filter";
}
