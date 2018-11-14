using System.Collections.Generic;

namespace IcsokaPayments.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string paymentReference { get; set; }
        public string TransactionReferrence { get; set; }
        public double Amount { get; set; }
        public int FeeSharing { get; set; }
        public int MerchantId { get; set; }
        public int PaymentGatewayId { get; set; }
        public bool Status { get; set; }
        public bool Verified { get; set; }
        public Transaction()
        {
            Merchants = new HashSet<Merchant>();
            PaymentGateways = new HashSet<PaymentGateway>();
        }

        public virtual ICollection<Merchant> Merchants { get; set; }
        public virtual ICollection<PaymentGateway> PaymentGateways { get; set; }
    }
}
