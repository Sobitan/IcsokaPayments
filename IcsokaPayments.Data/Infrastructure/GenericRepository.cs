using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace IcsokaPayments.Data.Infrastructure
{
   public class SortExpression<TEntity, TType>
    {
       private Expression<Func<TEntity, TType>> SortProperty;
    }

    public class GenericRepository<TEntity> where TEntity : class
    {
        internal IscokaPaymentEntities Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(IscokaPaymentEntities context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity GetById(object id)
        {
            if (id == null) throw new ArgumentNullException("id");
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            try
            {
                DbSet.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Delete(object id)
        {
            if (id == null) throw new ArgumentNullException("id");

            try
            {
                TEntity entityToDelete = DbSet.Find(id);
                Delete(entityToDelete);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null) throw new ArgumentNullException("entityToDelete");

            try
            {
                if (Context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    DbSet.Attach(entityToDelete);
                }
                DbSet.Remove(entityToDelete);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null) throw new ArgumentNullException("entityToUpdate");

            try
            {
                DbSet.Attach(entityToUpdate);
                Context.Entry(entityToUpdate).State = EntityState.Modified;
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual IQueryable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return DbSet.SqlQuery(query, parameters).AsQueryable();
        }
    }
}

