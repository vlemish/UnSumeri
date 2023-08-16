using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Commands.Move.Statuses
{
    public class MoveCategorySuccess : IOperationStatus
    {
        private readonly int _id;
        private readonly int? _parentId;

        public MoveCategorySuccess(int id, int? parentId)
        {
            _id = id;
            _parentId = parentId;
        }

        public OperationStatusValue Status => OperationStatusValue.NoContent;

        public string Message => $"Category where Id = {_id} and Parent Id = {_parentId} was successfully moved!";
    }
}
