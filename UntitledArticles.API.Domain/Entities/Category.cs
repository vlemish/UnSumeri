using System.ComponentModel.DataAnnotations.Schema;

namespace UntitledArticles.API.Domain.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAtTime { get; set; }

        public int ParentCategoryId { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        [NotMapped]
        public CategoryMapping CategoryMapping { get; set; }
    }
}
