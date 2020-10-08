using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using PostgresqlContext = DataAccess.DataAccess.Context.PostgresqlContext;

namespace DataAccess.DataAccess.Entityframework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
         where TEntity : class, IEntity, new()
    {
        protected PostgresqlContext _context { get; set; }

        public EfEntityRepositoryBase(PostgresqlContext context)
        {
            this._context = context;
        }
        public void Add(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();

        }

        public void Delete(TEntity entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _context.SaveChanges();

        }

        public int Delete(int id, string table)
        {

            string query = string.Format("DELETE FROM {0} WHERE id={1}", table, id);
            var rows = _context.Database.ExecuteSqlCommand(query, id);
            return rows;

        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().AsNoTracking().SingleOrDefault(filter);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().AsNoTracking().ToList()
                : _context.Set<TEntity>().Where(filter).AsNoTracking().ToList();

        }


        public List<TEntity> GetPaginatedList(Expression<Func<TEntity, bool>> filter,
            out int total, int pageIndex, int pageSize,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            int skipCount = (pageIndex) * pageSize;
            var _resetSet = filter != null
                 ? _context.Set<TEntity>().Where<TEntity>(filter)
                 : _context.Set<TEntity>();
            total = _resetSet.Count();
            if (orderBy != null)
            {
                _resetSet = orderBy(_resetSet);
            }
            _resetSet = skipCount == 0 ? _resetSet.Take(pageSize) :
            _resetSet.Skip(skipCount).Take(pageSize);

            return _resetSet.ToList();

        }

        public void Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Count();


        }


        public List<TEntity> GetPaginatedListInclude(Expression<Func<TEntity, bool>> filter,
            out int total, int pageIndex, int pageSize,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            int skipCount = (pageIndex) * pageSize;

            var _resetSet = filter != null
                 ? _context.Set<TEntity>().Where<TEntity>(filter)
                 : _context.Set<TEntity>();
            total = _resetSet.Count();

            if (orderBy != null)
            {
                _resetSet = orderBy(_resetSet);
            }
            _resetSet = skipCount == 0 ? _resetSet.Take(pageSize) :
            _resetSet.Skip(skipCount).Take(pageSize);


            foreach (var includeProperty in includeProperties)
            {
                _resetSet = _resetSet.Include(includeProperty);
            }


            return _resetSet.ToList();

        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;


        }

        public List<TEntity> GetListQuery(string query)
        {
            return query == null
                ? _context.Set<TEntity>().ToList()
                : _context.Set<TEntity>().FromSqlRaw(query).ToList();

        }


    }
}
