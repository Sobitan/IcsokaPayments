using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Service.Payments
{
    public class PostProcessPaymentRequest
    {
        public Settlement Settlement { get; set; }
        public string ClientToken { get; set; }

        public string AuthorizationTransactionId { get; set; }
        public string RedirectUrl { get; set; }
    }
}