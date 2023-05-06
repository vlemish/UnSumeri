using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Update.Statuses
{
    public class UpdateCategoryNotFound : IOperationStatus
    {
        private readonly int _id;

        public UpdateCategoryNotFound(int id)
        {
            _id = id;
        }

        public OperationStatusValue Status => OperationStatusValue.NotFound;

        public string Message => $"Couldn't update the category where id = {_id}: Category wasn't found!";
    }
}