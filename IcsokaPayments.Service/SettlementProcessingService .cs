using System;
using System.Collections.Generic;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Domain.Enums;
using IcsokaPayments.Service.Infrastructure;
using IcsokaPayments.Service.Logging;
using IcsokaPayments.Service.Payments;

namespace IcsokaPayments.Service
{
    public class SettlementProcessingService : ISettlementProcessingService
    {
        private readonly IPaymentService _paymentService;
        private readonly IRepositoryService<Settlement> _repositoryService;
        private readonly ILogger _logger;

        public SettlementProcessingService(IPaymentService paymentService, 
            IRepositoryService<Settlement> repositoryService,
            ILogger logger)
        {
            _paymentService = paymentService;
            _repositoryService = repositoryService;
            _logger = logger;
        }

        public MakePaymentResult MakePayment(ProcessPaymentRequest processPaymentRequest,
            Dictionary<string, string> extraData)
        {
            
            if (processPaymentRequest == null)
                throw new ArgumentNullException("processPaymentRequest");

            if (processPaymentRequest.SettlementGuid == Guid.Empty)
                processPaymentRequest.SettlementGuid = Guid.NewGuid();

            var result = new MakePaymentResult();
            var utcNow = DateTime.UtcNow;
            try
            {
                #region 

                
                


                #endregion

                #region Addresses & pre payment workflow

                // give payment processor the opportunity to fullfill billing address
                var preProcessPaymentResult = _paymentService.PreProcessPayment(processPaymentRequest);

                if (!preProcessPaymentResult.Success)
                {
                    result.Errors.AddRange(preProcessPaymentResult.Errors);
                    result.Errors.Add("Common.Error.PreProcessPayment");
                    return result;
                }


                #endregion

                #region Payment workflow

                // skip payment workflow if order total equals zero
                bool skipPaymentWorkflow = processPaymentRequest.AmountTotal == decimal.Zero;

                // payment workflow
                if (!skipPaymentWorkflow)
                {
                    var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName);
                    if (paymentMethod == null)
                        throw new Exception(("Payment.CouldNotLoadMethod"));
                }
                else
                {
                    processPaymentRequest.PaymentMethodSystemName = "";
                }

                // process payment
                ProcessPaymentResult processPaymentResult = null;
                processPaymentResult = !skipPaymentWorkflow ? _paymentService.ProcessPayment(processPaymentRequest) : new ProcessPaymentResult {NewPaymentStatus = PaymentStatus.Paid};

                #endregion

                if (processPaymentResult != null && processPaymentResult.Success)
                {
                    
                    {
                        #region Save details
                        var initial = new Settlement()
                        {
                            Amount = processPaymentRequest.AmountTotal,
                            PaymentMethodSystemName = processPaymentRequest.PaymentMethodSystemName,
                            AuthorizationTransactionId =  processPaymentResult.AuthorizationTransactionId,
                            AuthorizationTransactionCode = processPaymentResult.AuthorizationTransactionCode,
                            CardCvv2 =  processPaymentRequest.CreditCardCvv2,
                            CardType =  processPaymentRequest.CreditCardType,
                            CardExpirationMonth = processPaymentRequest.CreditCardExpireMonth.ToString("D2"),
                            CardExpirationYear =  processPaymentRequest.CreditCardExpireYear.ToString(),
                            Email = processPaymentRequest.Beneficiary,
                            CardName = processPaymentRequest.CreditCardName,
                            CardNumber = processPaymentRequest.CreditCardNumber,
                            CreatedOnUtc = DateTime.Now,
                            PaidDateUtc = DateTime.Now,
                            AuthorizationTransactionResult = processPaymentResult.AuthorizationTransactionResult,
                            PaymentStatus = processPaymentResult.NewPaymentStatus,
                            UpdatedOnUtc =  DateTime.Now
                        };

                        _repositoryService.Insert(initial);
                        
                        #endregion

                      


                    }
                }
                else
                {
                    result.AddError("Payment.PayingFailed");

                    if (processPaymentResult != null)
                        foreach (var paymentError in processPaymentResult.Errors)
                        {
                            result.AddError(paymentError);
                        }
                }
            }
            catch (Exception exc)
            {
                 _logger.Error(exc.Message, exc);
                result.AddError(exc.Message);
            }

            if (result.Errors.Count > 0)
            {
                _logger.Error(string.Join(" ", result.Errors));
            }

            return result;
        }
  
    }
}

