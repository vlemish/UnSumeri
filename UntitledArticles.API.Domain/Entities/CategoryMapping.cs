using System.Collections.ObjectModel;

namespace UntitledArticles.API.Domain.Entities
{
    public class CategoryMapping
    {
        public Category CurrentCategory { get; set; }

        public ReadOnlyCollection<Category> SubCategories { get; set; }
    }
}
