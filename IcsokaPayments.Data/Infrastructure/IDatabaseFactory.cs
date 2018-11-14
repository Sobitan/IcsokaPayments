using System;

namespace IcsokaPayments.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        IscokaPaymentEntities Get();
    }
}
