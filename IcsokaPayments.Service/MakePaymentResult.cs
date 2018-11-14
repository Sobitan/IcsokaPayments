using System.Collections.Generic;
using IcsokaPayments.Domain.Entities;

namespace IcsokaPayments.Service
{
    public class MakePaymentResult
    {
          public List<string> Errors { get; set; }

        public MakePaymentResult() 
        {
            this.Errors = new List<string>();
        }

        public bool Success
        {
            get { return (this.Errors.Count == 0); }
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        
        
        public Settlement Settlement { get; set; }
    }
}