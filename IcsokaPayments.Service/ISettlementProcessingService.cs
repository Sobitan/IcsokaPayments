using System.Collections.Generic;
using IcsokaPayments.Service.Payments;

namespace IcsokaPayments.Service
{
    public interface ISettlementProcessingService
    {
        MakePaymentResult MakePayment(ProcessPaymentRequest processPaymentRequest, Dictionary<string, string> extraData);
    }
}