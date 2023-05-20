using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Articles.Commands.Add.Statuses
{
    public class AddArticleSuccessStatus : IOperationStatus
    {
        private readonly int _id;

        public AddArticleSuccessStatus(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.OK;

        public string Message => $"Successfully added article. Id = {_id}";
    }
}