using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Repository
{
    public class SettlementRepository : RepositoryBase<Settlement>, ISettlementRepository
    {
        public SettlementRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface ISettlementRepository:IRepository<Settlement>
    {
    }
}