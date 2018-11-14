using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Service.Payments
{
    public class RefundPaymentRequest
    {
        /// <summary>
        /// Gets or sets an settlement
        /// </summary>
        public Settlement Settlement { get; set; }

        /// <summary>
        /// Gets or sets an amount
        /// </summary>
        public decimal AmountToRefund { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it's a partial refund; otherwize, full refund
        /// </summary>
        public bool IsPartialRefund { get; set; }
    }
}