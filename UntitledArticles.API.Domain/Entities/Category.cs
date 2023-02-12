namespace UntitledArticles.API.Domain.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAtTime { get; set; }

        public ICollection<CategoryMapping> CategoryMappings { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
