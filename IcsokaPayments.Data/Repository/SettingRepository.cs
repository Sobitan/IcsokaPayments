using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Repository
{
    public class  SettingRepository:RepositoryBase<Setting>, ISettingRepository
    {
        public SettingRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }


    public interface ISettingRepository : IRepository<Setting>
    {
        
    }
}