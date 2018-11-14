namespace IcsokaPayments.Domain.Entities
{
    public partial class PaymentMethod : BaseEntity
    {
        /// <summary>
        /// Gets or sets the payment method system name
        /// </summary>
        
        public string PaymentMethodSystemName { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        
        public string FullDescription { get; set; }
    }
}