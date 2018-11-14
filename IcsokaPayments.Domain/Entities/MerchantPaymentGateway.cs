using System.Collections.Generic;

namespace IcsokaPayments.Domain.Entities
{
    public class MerchantPaymentGateway
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int PaymentGatewayId { get; set; }
        public string Name { get; set; }

        public MerchantPaymentGateway()
        {
            Merchants = new HashSet<Merchant>();
            PaymentGateways = new HashSet<PaymentGateway>();
        }

        public virtual ICollection<Merchant> Merchants { get; set; }
        public virtual ICollection<PaymentGateway> PaymentGateways { get; set; }
    }
}
