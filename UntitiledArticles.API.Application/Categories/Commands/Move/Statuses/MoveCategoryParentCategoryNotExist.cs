using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Categories.Commands.Move.Statuses
{
    public class MoveCategoryParentCategoryNotExist : IOperationStatus
    {
        private readonly int _id;
        private readonly int? _parentId;

        public MoveCategoryParentCategoryNotExist(int id, int? parentId)
        {
            _id = id;
            _parentId = parentId;
        }

        public OperationStatusValue Status => OperationStatusValue.ParentNotExists;

        public string Message => $"Category where Id = {_id} can't be moved because parent with Id = {_parentId} doesn't exist!";
    }
}
