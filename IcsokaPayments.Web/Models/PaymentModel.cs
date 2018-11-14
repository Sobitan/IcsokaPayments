using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IcsokaPayments.Web.Models
{
    public class PaymentModel
    {
       public PaymentModel()
        {
         PaymentMethods = new List<SelectListItem>();   
        }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, Display(Name = "Payment Method")]
        public string SelectedPaymentMethod { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
        
    }
}