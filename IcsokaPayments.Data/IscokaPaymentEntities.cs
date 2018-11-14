using System.Data.Entity;
using IcsokaPayments.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IcsokaPayments.Data
{
    public class IscokaPaymentEntities : IdentityDbContext<User>
    {
     public IscokaPaymentEntities()
            : this("name=DefaultConnection")
        {
            
        }

     public IscokaPaymentEntities(string nameOrConnectionString)
         : base(nameOrConnectionString)
        {

        }


     public IDbSet<Settlement> Settlements { get; set; }
     public IDbSet<Setting> Settings { get; set; }
    // public IDbSet<SettlementHistory> SettlementHistories { get; set; }
     public IDbSet<Log> Logs { get; set; }

     public IDbSet<Bank> Banks { get; set; }

     public IDbSet<Fee> Fees { get; set; }
     public IDbSet<FeeSharing> FeeSharings { get; set; }

     public IDbSet<Merchant> Merchants { get; set; }

     public IDbSet<MerchantBank> MerchantBanks { get; set; }
     public IDbSet<PaymentsStatus> PaymentsStatuses { get; set; }
     public IDbSet<MerchantPaymentGateway> MerchantPaymentGateways { get; set; }
     public IDbSet<PaymentGateway> PaymentGateways { get; set; }
     public IDbSet<Transaction> Transactions { get; set; }

     public IDbSet<TransactionLog> TransactionLogs { get; set; }

     public virtual void Commit()
     {
         SaveChanges();
     }
    }
}