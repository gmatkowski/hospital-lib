using Hospital.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Hospital.Abstracts
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _dataContext;

        public BaseRepository(DbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int Count()
        {
            return _dataContext.Set<T>().Count();
        }

        public bool Exists(T entity)
        {
            return _dataContext.Set<T>().Local.Any(e => e == entity);
        }

        public void Delete(T entity)
        {
            if (!this.Exists(entity)) { 
                _dataContext.Set<T>().Attach(entity);
            }
            _dataContext.Set<T>().Remove(entity);
            _dataContext.SaveChanges();
        }

        public T Get(int id)
        {
            return _dataContext.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            _dataContext.SaveChanges();
        }

        public IList<T> List()
        {
            return _dataContext.Set<T>().ToList();
        }

        public IList<T> List(Expression<Func<T, bool>> expression)
        {
            return _dataContext.Set<T>().Where(expression).ToList();
        }

        public void Update(T entity)
        {
            if (!this.Exists(entity))
            {
                _dataContext.Set<T>().Attach(entity);
            }
            _dataContext.Entry<T>(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public void Clear()
        {
            _dataContext.Set<T>().Local.Clear();
        }
    }
}
