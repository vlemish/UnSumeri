namespace UntitledArticles.API.Domain.Entities
{
    public class Article : Entity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Category Category { get; set; }
    }
}
