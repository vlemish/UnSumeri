using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.Statuses
{
    public class GetCategorySuccess : IOperationStatus
    {
        private readonly int _id;

        public GetCategorySuccess(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.OK;

        public string Message => $"Category where Id = {_id} was successfully retrieved";
    }
}