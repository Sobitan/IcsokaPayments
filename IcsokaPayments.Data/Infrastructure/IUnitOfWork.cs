namespace IcsokaPayments.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
