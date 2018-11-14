using System.Collections.Generic;

namespace IcsokaPayments.Domain.Entities
{
    public class PaymentsStatus
    {
        public int Id { get; set; }
        public int PaymentGatewayId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public PaymentsStatus()
        {
            PaymentGateways = new HashSet<PaymentGateway>();
        }
        public virtual ICollection<PaymentGateway> PaymentGateways { get; set; }
    }
}
