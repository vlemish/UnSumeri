using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Articles.Commands.Add.Statuses
{
    public class AddArticleSuccessStatus : IOperationStatus
    {
        private readonly int _id;

        public AddArticleSuccessStatus(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.Created;

        public string Message => $"Successfully added article. Id = {_id}";
    }
}
