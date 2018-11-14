using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Repository
{
    public class LogRepository: RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface ILogRepository : IRepository<Log>
    {
        
    }
}