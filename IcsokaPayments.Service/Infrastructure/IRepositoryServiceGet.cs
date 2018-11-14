using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IcsokaPayments.Service.Infrastructure
{
    public interface IRepositoryServiceGet<T> : IRepositoryService<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}