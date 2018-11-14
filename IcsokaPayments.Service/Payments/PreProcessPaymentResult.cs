using System.Collections.Generic;

namespace IcsokaPayments.Service.Payments
{
  
    public class PreProcessPaymentResult
    {
        public PreProcessPaymentResult()
        {
            Errors = new List<string>();
        }

        public IList<string> Errors { get; set; }

        public bool Success
        {
            get { return Errors.Count == 0; }
        }

        public string AuthorizationTransactionCode { get; set; }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}