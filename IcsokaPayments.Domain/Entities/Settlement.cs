using System;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Domain.Entities
{
    public class Settlement: BaseEntity
    {
        
        public string UserIp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether storing of credit card number is allowed
        /// </summary>
        
        public bool AllowStoringCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card type
        /// </summary>
        
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the card name
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Gets or sets the card number
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the masked credit card number
        /// </summary>
        public string MaskedCreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card CVV2
        /// </summary>
        public string CardCvv2 { get; set; }

        /// <summary>
        /// Gets or sets the card expiration month
        /// </summary>
        public string CardExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the card expiration year
        /// </summary>
        public string CardExpirationYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether storing of credit card number is allowed
        /// </summary>
        public bool AllowStoringDirectDebit { get; set; }

        /// <summary>
        /// Gets or sets the direct debit account holder
        /// </summary>
        public string DirectDebitAccountHolder { get; set; }

        /// <summary>
        /// Gets or sets the direct debit account number
        /// </summary>
        public string DirectDebitAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the direct debit bank code
        /// </summary>
        public string DirectDebitBankCode { get; set; }

        /// <summary>
        /// Gets or sets the direct debit bank name
        /// </summary>
        public string DirectDebitBankName { get; set; }

        /// <summary>
        /// Gets or sets the direct debit bic
        /// </summary>
        public string DirectDebitBVN { get; set; }

        /// <summary>
        /// Gets or sets the direct debit country
        /// </summary>
        public string DirectDebitCountry { get; set; }

        /// <summary>
        /// Gets or sets the direct debit iban
        /// </summary>
        public string DirectDebitIban { get; set; }


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
        /// Gets or sets the subscription transaction identifier
        /// </summary>
        
        public string SubscriptionTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the paid date and time
        /// </summary>
        
        public DateTime? PaidDateUtc { get; set; }


       
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time when order was updated
        /// </summary>
       
        public DateTime UpdatedOnUtc { get; set; }

        public string PaymentMethodSystemName { get; set; }


        /// <summary>
        /// Gets or sets the payment status
        /// </summary>
        public Enums.PaymentStatus PaymentStatus
        {
            get
            {
                return (Enums.PaymentStatus)PaymentStatusId;
            }
            set
            {
                PaymentStatusId = (int)value;
            }
        }

        public int PaymentStatusId { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
    }
}