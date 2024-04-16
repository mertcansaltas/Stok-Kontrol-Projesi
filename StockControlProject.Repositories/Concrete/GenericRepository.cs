using Microsoft.EntityFrameworkCore;
using StockControlProject.Entities.Entities;
using StockControlProject.Repositories.Abstract;
using StockControlProject.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockControlProject.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StockControlContext _context;

        public GenericRepository(StockControlContext context)
        {
            _context = context;
        }

        public bool Activate(int id)
        {
            T item = GetById(id);
            item.isActive = true;
            return Update(item);
        }

        public bool Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var item in items)
                    {
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();
                    return Save() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any(exp);

        public List<T> GetActive() => _context.Set<T>().Where(x => x.isActive == true).ToList();

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.isActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(exp);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp);

        public T GetById(int id) => _context.Set<T>().Find(id);


        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.Id == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().Where(exp).ToList();

        public bool Remove(T entity)
        {
            entity.isActive = false;
            return Update(entity);
        }

        public bool Remove(int id)
        {
            try
            {
                T item = GetById(id);
                return Remove(item);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(exp);
                    int counter = 0;
                    foreach (var item in collection)
                    {
                        item.isActive = false;
                        bool operationResult = Update(item);
                        counter++;
                    }
                    if (collection.Count == counter) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public bool Update(T entity)
        {
            try
            {
                entity.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(entity);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
