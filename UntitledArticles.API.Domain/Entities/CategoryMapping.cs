using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledArticles.API.Domain.Entities
{
    public class CategoryMapping : IEntity
    {
        public int Id { get; set; }

        public Category AncestorCategory { get; set; }

        public Category SubCategory { get; set; }
    }
}
