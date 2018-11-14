using System;
using System.Collections.Generic;
using System.Linq;

namespace IcsokaPayments.Service.Payments
{
    public class PaymentMethodProvider:IPaymentMethodProvider
    {
        private readonly IDictionary<string, Func<IPaymentMethod>> _factories;
        public PaymentMethodProvider(IDictionary<string, Func<IPaymentMethod>> factories)
        {
            _factories = factories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public IPaymentMethod GetPayment(string systemName)
        {
            var factory = _factories[systemName];
            var payMethod = factory == null ? null : factory();
            return payMethod;
        }

        public IList<IPaymentMethod> GetPayments()
        {
            return _factories.Values.Select(f => f()).ToList();
        }
    }
}