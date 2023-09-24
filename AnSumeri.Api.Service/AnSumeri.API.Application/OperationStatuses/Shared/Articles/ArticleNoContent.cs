namespace AnSumeri.API.Application.OperationStatuses.Shared.Articles;

public class ArticleNoContent: IOperationStatus
{
    public OperationStatusValue Status => OperationStatusValue.NoContent;
    public string Message => "There were no articles found for specified filter";
}
