using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IcsokaPayments.Web.Models
{
    public abstract class PaymentInfoModelBase 
    {
        public int Id { get; set; }
        public string DescriptionText { get; set; }
    }


    public class ManualPaymentInfoModel : PaymentInfoModelBase
    {
        public ManualPaymentInfoModel()
        {
            CreditCardTypes = new List<SelectListItem>();
            ExpireMonths = new List<SelectListItem>();
            ExpireYears = new List<SelectListItem>();
        }

        [DisplayName("CreditCard Type")]
        [AllowHtml, Required]
        public string CreditCardType { get; set; }
        [DisplayName("Select Credit Card")]
        public IList<SelectListItem> CreditCardTypes { get; set; }

        [DisplayName("CardholderName")]
        [AllowHtml,Required]
        public string CardholderName { get; set; }

        [DisplayName("CardNumber")]
        [AllowHtml,Required,CreditCard]
        public string CardNumber { get; set; }

        [DisplayName("ExpirationDate")]
        [AllowHtml, Required]
        public string ExpireMonth { get; set; }
        [DisplayName("ExpirationDate")]
        [AllowHtml, Required]
        public string ExpireYear { get; set; }
        public IList<SelectListItem> ExpireMonths { get; set; }
        public IList<SelectListItem> ExpireYears { get; set; }

        [DisplayName("Card Code(CVV)")]
        [AllowHtml]
        [Required, RegularExpression(@"^[0-9]{3,4}$")]
        public string CardCode { get; set; }
        [Required, Display(Name = "Pin")]
        public string PinCode { get; set; }
    }
}