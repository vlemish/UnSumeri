using UntitiledArticles.API.Application.OperationStatuses;

namespace UntitiledArticles.API.Application.Articles.Commands.Add.Statuses
{
    public class AddArticleCategoryNotExist : IOperationStatus
    {
        private readonly int _categoryId;

        public AddArticleCategoryNotExist(int categoryId)
        {
            _categoryId = categoryId;
        }

        public OperationStatusValue Status => OperationStatusValue.NotFound;

        public string Message => $"Couldn't add article where Category Id = {_categoryId}. Category doesn't exist!";
    }
}