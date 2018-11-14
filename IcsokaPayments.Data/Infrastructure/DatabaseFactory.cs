namespace IcsokaPayments.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private IscokaPaymentEntities _dataContext;

        public IscokaPaymentEntities Get()
        {
            return _dataContext ?? (_dataContext = new IscokaPaymentEntities());
        }

        protected override void DisposeCore(bool disose)
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }

        
    }
}
