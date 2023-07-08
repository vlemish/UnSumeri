using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById.Statuses
{
    public class GetCategoryByIdSuccess : IOperationStatus
    {
        private readonly int _id;

        public GetCategoryByIdSuccess(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.OK;

        public string Message => $"Category where Id = {_id} was successfully retrieved";
    }
}