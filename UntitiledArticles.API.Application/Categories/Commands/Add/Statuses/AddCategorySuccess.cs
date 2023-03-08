using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UntitiledArticles.API.Application.OperationStatuses;

using UntitledArticles.API.Domain.Entities;

namespace UntitiledArticles.API.Application.Categories.Commands.Add.Statuses
{
    public class AddCategorySuccess : IOperationStatus
    {
        private readonly Category _category;

        public AddCategorySuccess(Category category)
        {
            _category = category;
        }

        public OperationStatusValue Status => OperationStatusValue.OK;

        public string Message => $"Category where Id = {_category.Id} and Name = {_category.Name} was succesfully added!";
    }
}
