﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using PostgresqlContext = DataAccess.DataAccess.Context.PostgresqlContext;

namespace DataAccess.DataAccess.Entityframework
{
    public class EfEntityRepositoryBase<TEntity> :IEntityRepository<TEntity> 
        where TEntity:class,IEntity,new()
    {


        protected PostgresqlContext  _context { get; set; }

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
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().SingleOrDefault(filter);
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {

            return filter == null
                ? _context.Set<TEntity>().ToList()
                : _context.Set<TEntity>().Where(filter).ToList();

        }

        public void Update(TEntity entity)
        {

            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();

        }


    }
}
