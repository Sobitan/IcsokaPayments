using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IcsokaPayments.Data.Infrastructure;

namespace IcsokaPayments.Service.Infrastructure
{
    public abstract class RepositoryService<T> where T : class
    {
        protected IRepository<T> GenericRepository;
        private readonly IUnitOfWork _unitOfWork;

        protected RepositoryService(IRepository<T> genericRepository, IUnitOfWork unitOfWork)
        {
            GenericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        protected RepositoryService()
        {
            _unitOfWork = new UnitOfWork(new DatabaseFactory());
        }

        #region IRepositoryService<T> Members

        public IEnumerable<T> GetAll()
        {
            return GenericRepository.GetAll();
        }

        public T GetById(int id)
        {
            return GenericRepository.GetById(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GenericRepository.GetMany(where);
        }

        public void Update(T entity)
        {
            

            GenericRepository.Update(entity);
            Save();
        }

        public void Insert(T entity)
        {
            GenericRepository.Add(entity);
            Save();
        }

        public void Update(IEnumerable<T> entities)
        {
            

            GenericRepository.Update(entities);
            Save();
        }

        public void Insert(IEnumerable<T> entities)
        {
            GenericRepository.Add(entities);
            Save();
        }
        public void Delete(int id)
        {
            var student = GenericRepository.GetById(id);
            GenericRepository.Delete(student);
            Save();
        }

        public void Delete(IEnumerable<T> entities)
        {
            GenericRepository.Delete(entities);
            _unitOfWork.Commit();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        #endregion
    }
}
