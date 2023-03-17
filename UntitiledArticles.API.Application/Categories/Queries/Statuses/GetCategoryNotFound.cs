using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.Statuses
{
    public class GetCategoryNotFound : IOperationStatus
    {
        private readonly int _id;

        public GetCategoryNotFound(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.NotFound;

        public string Message => $"Category with Id = {_id} wasn't found!";
    }
}
