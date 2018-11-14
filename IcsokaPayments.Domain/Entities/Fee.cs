namespace IcsokaPayments.Domain.Entities
{
    public class Fee
    {
        public int Id { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public string Flat { get; set; }
        public double Percentage { get; set; }
        public Fee()
        {
           
        }
    }
}
