using System.Collections.Generic;

namespace IcsokaPayments.Service.Payments
{
    public interface IPaymentMethodProvider
    {
        IList<IPaymentMethod> GetPayments();
        IPaymentMethod GetPayment(string systemName);
    }
}