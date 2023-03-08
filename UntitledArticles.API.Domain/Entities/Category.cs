﻿using System.ComponentModel.DataAnnotations.Schema;

namespace UntitledArticles.API.Domain.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAtTime { get; } = DateTime.UtcNow;

        public Category Parent { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<Category> SubCategories { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}