using System.Data.Entity.ModelConfiguration;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Data.Configuration
{
    public class LogConfiguration:EntityTypeConfiguration<Log>
    {
        public LogConfiguration()
        {
            HasOptional(p => p.User).WithMany().HasForeignKey(x => x.UserId);
            Ignore(x => x.LogLevel);
            
        }
    }
}