using System.Collections.Generic;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Service.Payments
{
    public class ProcessPaymentResult
    {
        private PaymentStatus _newPaymentStatus = PaymentStatus.Pending;
        public IList<string> Errors { get; set; }

        public ProcessPaymentResult()
        {
            Errors = new List<string>();
        }

        public bool Success
        {
            get { return Errors.Count == 0; }
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }


        /// <summary>
        /// Gets or sets an AVS result
        /// </summary>
        public string AvsResult { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction identifier
        /// </summary>
        public string AuthorizationTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction code
        /// </summary>
        public string AuthorizationTransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the authorization transaction result
        /// </summary>
        public string AuthorizationTransactionResult { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction identifier
        /// </summary>
        public string CaptureTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the capture transaction result
        /// </summary>
        public string CaptureTransactionResult { get; set; }


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
    }
}