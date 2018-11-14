using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IcsokaPayments.Service.Infrastructure
{
    public interface IRepositoryService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        void Update(T entity);
        void Insert(T entity);
        void Delete(int id);
        void Save();
        void Update(IEnumerable<T> entities);
        void Insert(IEnumerable<T> entities);

        void Delete(IEnumerable<T> entities);
    }
}
