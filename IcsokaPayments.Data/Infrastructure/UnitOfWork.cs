namespace IcsokaPayments.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private IscokaPaymentEntities  _dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        protected IscokaPaymentEntities  DataContext
        {
            get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}
