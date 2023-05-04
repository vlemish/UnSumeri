using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;

using UntitledArticles.API.Domain.Enums;

namespace UntitledArticles.API.Infrastructure.Extensions
{
    internal static class QueryExtensions
    {
        internal static IIncludableQueryable<TInput, ICollection<TInput>> IncludeSelfReferencingCollectionWithDepth<TInput>(this DbSet<TInput> dbSet,
            Expression<Func<TInput, ICollection<TInput>>> navigationInclude,
            int depth)
            where TInput : class
        {
            var includableQueryale = dbSet.Include(navigationInclude);
            for (int i = 0; i < depth - 1; i++)
            {
                includableQueryale = includableQueryale.ThenInclude(navigationInclude);
            }

            return includableQueryale;
        }

        //internal static IIncludableQueryable<TInput, ICollection<TInput>> IncludeSelfReferencingCollectionWithDepth<TInput>(this IQueryable<TInput> input,
        //    Expression<Func<TInput, ICollection<TInput>>> navigationInclude,
        //    int depth)
        //    where TInput : class
        //{
        //    var includableQueryale = input.Include(navigationInclude);
        //    for (int i = 0; i < depth; i++)
        //    {
        //        includableQueryale.ThenInclude(navigationInclude);
        //    }

        //    return includableQueryale;
        //}


        internal static IOrderedQueryable<T> Sort<T, TKey>(this DbSet<T> dbSet, Expression<Func<T, TKey>> keySelector,
            OrderByOption orderByOption)
            where T : class
        {
            if (orderByOption == OrderByOption.ASC)
            {
                return dbSet.OrderBy(keySelector);
            }

            return dbSet.OrderBy(keySelector);
        }
    }
}
