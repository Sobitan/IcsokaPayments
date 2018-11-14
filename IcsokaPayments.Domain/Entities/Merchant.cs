namespace IcsokaPayments.Domain.Entities
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Domain { get; set; }
        public string CallBackURL { get; set; }
        public Merchant()
        {

        }
    }
}
