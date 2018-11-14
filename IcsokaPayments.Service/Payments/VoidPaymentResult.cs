using System.Collections.Generic;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Service.Payments
{
    public class VoidPaymentResult
    {
        private PaymentStatus _newPaymentStatus = PaymentStatus.Pending;
        public IList<string> Errors { get; set; }

        public VoidPaymentResult()
        {
            Errors = new List<string>();
        }

        public bool Success
        {
            get { return (this.Errors.Count == 0); }
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        #region Properties

        /// <summary>
        /// Gets or sets a payment status after processing
        /// </summary>
        public PaymentStatus NewPaymentStatus
        {
            get
            {
                return _newPaymentStatus;
            }
            set
            {
                _newPaymentStatus = value;
            }
        }

        #endregion
    }
}