using System.Collections.Generic;
using IcsokaPayments.Service.Configurations;

namespace IcsokaPayments.Service.Payments
{
    public class PaymentSettings : ISettings
    {
        public PaymentSettings()
        {
            ActivePaymentMethodSystemNames = new List<string>();
        }

        /// <summary>
        /// Gets or sets a system names of active payment methods
        /// </summary>
        public List<string> ActivePaymentMethodSystemNames { get; set; }

        
    }
}