using AnSumeri.API.Application.OperationStatuses;
using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Application.Categories.Commands.Add.Statuses
{
    public class AddCategorySuccess : IOperationStatus
    {
        private readonly Category _category;

        public AddCategorySuccess(Category category)
        {
            _category = category;
        }

        public OperationStatusValue Status => OperationStatusValue.Created;

        public string Message => $"Category where Id = {_category.Id} and Name = {_category.Name} was succesfully added!";
    }
}
