using System.Collections.Generic;

namespace IcsokaPayments.Domain.Entities
{
    public class FeeSharing
    {
        public int Id { get; set; }
        public int FeeId { get; set; }
        public int MerchantId { get; set; }
        public double Percentage { get; set; }
        public FeeSharing()
        {
            Merchants = new HashSet<Merchant>();
        }

        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Merchant> Merchants { get; set; }
    }
}
