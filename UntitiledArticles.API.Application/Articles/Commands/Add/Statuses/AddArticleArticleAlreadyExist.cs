using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Articles.Commands.Add.Statuses
{
    public class AddArticleArticleAlreadyExist : IOperationStatus
    {
        public OperationStatusValue Status => OperationStatusValue.Duplicate;

        public string Message => $"Couldn't add article. Article already exist!";
    }
}