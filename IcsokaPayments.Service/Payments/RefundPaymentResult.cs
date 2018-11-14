using System.Collections.Generic;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Service.Payments
{
    public class RefundPaymentResult
    {

        private Domain.Enums.PaymentStatus _newPaymentStatus = IcsokaPayments.Domain.Enums.PaymentStatus.Pending;
        public IList<string> Errors { get; set; }

        public RefundPaymentResult()
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

        #region Properties

        /// <summary>
        /// Gets or sets a payment status after processing
        /// </summary>
        public IcsokaPayments.Domain.Enums.PaymentStatus NewPaymentStatus
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