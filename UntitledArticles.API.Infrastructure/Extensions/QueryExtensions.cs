using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;

namespace UntitledArticles.API.Infrastructure.Extensions
{
    internal static class QueryExtensions
    {
        internal static IIncludableQueryable<TInput, ICollection<TInput>> IncludeSelfReferencingCollectionWithDepth<TInput>(this DbSet<TInput> dbSet,
            Expression<Func<TInput, ICollection<TInput>>> navigationInclude,
            int depth)
            where TInput: class
        {
            var includableQueryale = dbSet.Include(navigationInclude);
            for (int i = 0; i < depth - 1; i++)
            {
                includableQueryale = includableQueryale.ThenInclude(navigationInclude);
            }

            return includableQueryale;
        }
    }
}
