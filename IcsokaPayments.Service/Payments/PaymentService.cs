using System;
using System.Collections.Generic;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Domain.Enums;

namespace IcsokaPayments.Service.Payments
{
    public class PaymentService:IPaymentService
    {
        private readonly PaymentSettings _paymentSettings;
        private readonly IPaymentMethodProvider _paymentMethodProvider;

        public PaymentService(PaymentSettings paymentSettings,
            IPaymentMethodProvider paymentMethodProvider)
        {
            _paymentSettings = paymentSettings;
            _paymentMethodProvider = paymentMethodProvider;
        }

        public IList<IPaymentMethod> LoadActivePaymentMethods()
        {
            return _paymentMethodProvider.GetPayments();
        }

        public IPaymentMethod LoadPaymentMethodBySystemName(string systemName)
        {
            return _paymentMethodProvider.GetPayment(systemName);
        }

        public IList<IPaymentMethod> LoadAllPaymentMethods()
        {
            return _paymentMethodProvider.GetPayments();
        }

        public PreProcessPaymentResult PreProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            if (processPaymentRequest.AmountTotal == decimal.Zero)
            {
                var result = new PreProcessPaymentResult();
                return result;
            }
            var paymentMethod = LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");

            return paymentMethod.PreProcessPayment(processPaymentRequest);
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            if (processPaymentRequest.AmountTotal == decimal.Zero)
            {
                var result = new ProcessPaymentResult
                {
                    NewPaymentStatus = PaymentStatus.Authorized
                };
                return result;
            }


            var paymentMethod = LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName);
            if (paymentMethod == null)
                throw new Exception("Payment method couldn't be loaded");
            return paymentMethod.ProcessPayment(processPaymentRequest);
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            if (!string.IsNullOrEmpty(postProcessPaymentRequest.Settlement.PaymentMethodSystemName))
            {
                var paymentMethod = LoadPaymentMethodBySystemName(postProcessPaymentRequest.Settlement.PaymentMethodSystemName);
                if (paymentMethod == null)
                    throw new Exception("Payment method couldn't be loaded");

                paymentMethod.PostProcessPayment(postProcessPaymentRequest);
            }
        }

        public bool CanRePostProcessPayment(Settlement settlement)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportCapture(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportPartiallyRefund(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportRefund(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public bool SupportVoid(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public RecurringPaymentType GetRecurringPaymentType(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            throw new System.NotImplementedException();
        }

        public PaymentMethodType GetPaymentMethodType(string paymentMethodSystemName)
        {
            throw new System.NotImplementedException();
        }

        public string GetMaskedCreditCardNumber(string creditCardNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}