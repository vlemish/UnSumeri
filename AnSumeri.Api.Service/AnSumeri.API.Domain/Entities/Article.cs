﻿namespace AnSumeri.API.Domain.Entities
{
    public class Article : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAtTime { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
