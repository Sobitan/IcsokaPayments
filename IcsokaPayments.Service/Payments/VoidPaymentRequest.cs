using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Service.Payments
{
    public class VoidPaymentRequest
    {
        /// <summary>
        /// Gets or sets a Settlement
        /// </summary>
        public Settlement Settlement { get; set; }
    }
}