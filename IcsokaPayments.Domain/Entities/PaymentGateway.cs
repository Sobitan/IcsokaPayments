namespace IcsokaPayments.Domain.Entities
{
    public class PaymentGateway
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string ConfirmationAPI { get; set; }
        public PaymentGateway()
        {

        }
    }
}
