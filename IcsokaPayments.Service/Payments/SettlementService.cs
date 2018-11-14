using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Data.Repository;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Service.Infrastructure;

namespace IcsokaPayments.Service.Payments
{
    public class SettlementService : RepositoryService<Settlement>, IRepositoryService<Settlement>
    {
        public SettlementService(ISettlementRepository settlementRepository, IUnitOfWork unitOfWork)
            : base(settlementRepository, unitOfWork)
        {
        }
    }
}