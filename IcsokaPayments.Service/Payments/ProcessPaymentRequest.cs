using System;
using System.Collections.Generic;

namespace IcsokaPayments.Service.Payments
{
    public class ProcessPaymentRequest
    {
        public ProcessPaymentRequest()
        {
            this.CustomValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets a InstitutionId identifier
        /// </summary>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets a Settlement identifier
        /// </summary>
        public int SettlementId { get; set; }

        public Guid SettlementGuid { get; set; }

        /// <summary>
        /// Gets or sets an settlement amount
        /// </summary>
        public decimal AmountTotal { get; set; }

        /// <summary>
        /// /// <summary>
        /// Gets or sets a payment method identifier
        /// </summary>
        /// </summary>
        public string PaymentMethodSystemName { get; set; }

        #region Payment method specific properties

        public string FileName { get; set; }

        public string DebitAccountName { get; set; }

        public string DebitAccountNumber { get; set; }

        public string DebitAccountSortCode { get; set; }

        public string AuthorizationTransactionId { get; set; }


        public string AuthorizationTransactionCode { get; set; }


        public string AuthorizationTransactionResult { get; set; }

        public string ClientToken { get; set; }

        public string Narration { get; set; }

        public string Beneficiary { get; set; }

        public string BeneficiaryAccountNumber { get; set; }

        public string BeneficiarySortCode { get; set; }


        /// <summary>
        /// Gets or sets a credit card type (Visa, Master Card, etc...)
        /// </summary>
        public string CreditCardType { get; set; }

        /// <summary>
        /// Gets or sets a credit card owner name
        /// </summary>
        public string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets a credit card number
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets a credit card expire year
        /// </summary>
        public int CreditCardExpireYear { get; set; }

        /// <summary>
        /// Gets or sets a credit card expire month
        /// </summary>
        public int CreditCardExpireMonth { get; set; }

        /// <summary>
        /// Gets or sets a credit card CVV2 (Card Verification Value)
        /// </summary>
        public string CreditCardCvv2 { get; set; }

        #endregion
        /// <summary>
        /// You can store any custom value in this property
        /// </summary>
        public Dictionary<string, object> CustomValues { get; set; }

        
    }
}