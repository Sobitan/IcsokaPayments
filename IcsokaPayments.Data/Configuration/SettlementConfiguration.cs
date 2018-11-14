using System.Data.Entity.ModelConfiguration;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Configuration
{
    public class SettlementConfiguration:EntityTypeConfiguration<Settlement>
    {
        public SettlementConfiguration()
        {
            Ignore(x => x.PaymentStatus);
           
            
        }
    }
}