namespace UntitledArticles.API.Domain.Entities
{
    public class CategoryMapping : Entity
    {
        public int Id { get; set; }

        public Category AncestorCategory { get; set; }

        public int AncestoryCategoryId { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
