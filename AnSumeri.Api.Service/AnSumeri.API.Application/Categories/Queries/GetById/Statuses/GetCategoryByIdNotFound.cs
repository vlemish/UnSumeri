using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Queries.GetById.Statuses
{
    public class GetCategoryByIdNotFound : IOperationStatus
    {
        private readonly int _id;

        public GetCategoryByIdNotFound(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.NotFound;

        public string Message => $"Category with Id = {_id} wasn't found!";
    }
}
