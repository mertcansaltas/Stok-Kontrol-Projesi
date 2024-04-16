using StockControlProject.Entities.Entities;
using StockControlProject.Repositories.Abstract;
using StockControlProject.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Service.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> _repository;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public bool Activate(int id)
        {
            if (id == 0 || GetById(id) == null)
            {
                return false;
            }
            return _repository.Activate(id);
        }

        public bool Add(T entity)
        {
            return _repository.Add(entity);
        }

        public bool Add(List<T> items)
        {
            return _repository.Add(items);  
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _repository.Any(exp);
        }

        public List<T> GetActive()
        {
            return _repository.GetActive();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetActive(includes);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetAll(includes);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetAll(exp, includes);
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetByDefault(exp);
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetById(id, includes);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetDefault(exp);
        }

        public bool Remove(T entity)
        {
            if (entity == null) return false;
            return _repository.Remove(entity);
        }

        public bool Remove(int id)
        {
            if (id <= 0) return false;
            return _repository.Remove(id);
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            return _repository.RemoveAll(exp);
        }

        public int Save()
        {
            return _repository.Save();
        }

        public bool Update(T entity)
        {
            if(entity == null) return false;
            return _repository.Update(entity);
        }
    }
}
