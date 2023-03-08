﻿using System.Linq.Expressions;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Domain.Contracts
{
    public interface IRepository<T> where T: IEntity
    {
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        Task DeleteManyAsync(IReadOnlyCollection<T> entities);

        Task<int> GetCount(Func<T, bool> predicate);

        Task<T> GetOneById(int id);

        Task<T> GetOneByFilter(Func<T, bool> predicate);

        Task<IList<T>> GetManyByFilter(Func<T, bool> predicate);

        Task<IList<T>> GetAll();
    }
}