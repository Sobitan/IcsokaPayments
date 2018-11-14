namespace IcsokaPayments.Domain.Entities
{
    public class MerchantBank
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public int BankId { get; set; }
        public string AccountNo { get; set; }
        public MerchantBank()
        {

        }

    }
}
