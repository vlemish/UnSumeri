namespace UntitledArticles.API.Domain.Entities
{
    public class Article : IEntity
    {
        public int Id { get; }

        public string Content { get; }

        public DateTime CreatedAtTime { get; set; }
    }
}
