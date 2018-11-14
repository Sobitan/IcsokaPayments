using System.Collections.Generic;
using System.Linq;

namespace IcsokaPayments.Data.Infrastructure
{
    public class PagedResult<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedResult(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;
            Results = source;
            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedResult(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;
            Results = source;
            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedResult(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;
            var enumerable = source as T[] ?? source.ToArray();
            Results = enumerable;
            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(enumerable);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        public IEnumerable<T> Results { get; internal set; }
    }
}
