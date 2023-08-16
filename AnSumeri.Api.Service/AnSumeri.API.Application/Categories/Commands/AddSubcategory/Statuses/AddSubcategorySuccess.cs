using AnSumeri.API.Application.OperationStatuses;

namespace AnSumeri.API.Application.Categories.Commands.AddSubcategory.Statuses
{
    public class AddSubcategorySuccess : IOperationStatus
    {
        private readonly string _name;
        private readonly int _parentId;

        public AddSubcategorySuccess(string name, int parentId)
        {
            _name = name;
            _parentId = parentId;
        }

        public OperationStatusValue Status => OperationStatusValue.Created;

        public string Message => $"Add Subcategory where Name = {_name} and Parent Id {_parentId} was successfully handled";
    }
}
