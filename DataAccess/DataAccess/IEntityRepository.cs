using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;

namespace DataAccess.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter = null);

        List<T> GetList(Expression<Func<T, bool>> filter = null);

        List<T> GetListQuery(string query);

        List<T> GetPaginatedList(Expression<Func<T, bool>> filter, out int total, int pageIndex, int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        List<T> GetPaginatedListInclude(Expression<Func<T, bool>> filter, out int total, int pageIndex, int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        int Delete(int id, string table);

        int Count(Expression<Func<T, bool>> filter = null);
    }
}