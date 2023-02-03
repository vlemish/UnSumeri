namespace UntitledArticles.API.Domain.Entities
{
    public class Category : Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CategoryMapping> CategoryMappings { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
