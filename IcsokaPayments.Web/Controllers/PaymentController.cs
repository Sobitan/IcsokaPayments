using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Service;
using IcsokaPayments.Service.Configurations;
using IcsokaPayments.Service.Logging;
using IcsokaPayments.Service.Payments;
using IcsokaPayments.Web.Models;

namespace IcsokaPayments.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ISettlementProcessingService _orderProcessingService;
        
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ISettlementProcessingService orderProcessingService,
            ISettingService settingService, ILogger logger, IPaymentService paymentService)
        {
            _orderProcessingService = orderProcessingService;
            _settingService = settingService;
            _logger = logger;
            _paymentService = paymentService;
        }


        public static List<SelectListItem> CreditCardTypes
        {
            get
            {
                var creditCardTypes = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Visa", Value = "Visa"},
                    new SelectListItem {Text = "Master Card", Value = "MasterCard"},
                    new SelectListItem {Text = "Discover", Value = "Discover"},
                    new SelectListItem {Text = "Amex", Value = "Amex"}
                };
                return creditCardTypes;
            }
        }

        //
        // GET: /Payment/
        public ActionResult Index()
        {
            var payment = new PaymentModel();
            var settings = _settingService.LoadSetting<PaymentSettings>();
            payment.PaymentMethods = settings.ActivePaymentMethodSystemNames.Select(m => new SelectListItem
            {
                    Selected = (m == payment.SelectedPaymentMethod),
                    Text = m,
                    Value = m
                }).ToList();
            payment.PaymentMethods.Insert(0, new SelectListItem()
            {
                Selected =  string.IsNullOrEmpty(payment.SelectedPaymentMethod),
                Text = "select",
                Value =  string.Empty
            });
            return View(payment);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Pay(PaymentModel model, FormCollection form)
        {
            MakePaymentResult makePaymentResult = null;
            PostProcessPaymentRequest postProcessPaymentRequest = null;
            var confirm = new ConfirmModel();
            try
            {
                var warnings = ValidatePaymentForm(form);
                foreach (var warning in warnings)
                    ModelState.AddModelError("", warning);
                if (!ModelState.IsValid)
                {
                    return View(confirm);
                }
                var processPaymentRequest = new ProcessPaymentRequest()
                {
                    AmountTotal =  model.Amount,
                    PaymentMethodSystemName =  model.SelectedPaymentMethod,
                    Beneficiary =  model.Email,
                      CreditCardType = form["CreditCardType"],
            CreditCardName = form["CardholderName"],
             CreditCardNumber = form["CardNumber"],
            CreditCardExpireMonth = int.Parse(form["ExpireMonth"]),
            CreditCardExpireYear = int.Parse(form["ExpireYear"]),
            CreditCardCvv2 = form["CardCode"],
                    ClientToken = form["PinCode"],
                    Narration = "Demo Payment"
                   
                };
                var extraData = new Dictionary<string, string>();
                makePaymentResult = _orderProcessingService.MakePayment(processPaymentRequest,extraData);
                if (!makePaymentResult.Success)
                {
                    confirm.Warnings.AddRange(makePaymentResult.Errors.Select(x => (x)));
                }
            }
            catch (Exception exception)
            {

                _logger.Warning(exception.Message, exception);
            }

            if (makePaymentResult == null || !makePaymentResult.Success || confirm.Warnings.Any())
            {
                return View(confirm);
            }
            
            try
            {
                postProcessPaymentRequest = new PostProcessPaymentRequest
                {
                    Settlement = new Settlement()
                    {
                        PaymentMethodSystemName = model.SelectedPaymentMethod
                    }
                };
               
                _paymentService.PostProcessPayment(postProcessPaymentRequest);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message,exception);
            }
            if (postProcessPaymentRequest != null && !string.IsNullOrEmpty(postProcessPaymentRequest.RedirectUrl))
            {
                return Redirect(postProcessPaymentRequest.RedirectUrl);
            }
            return RedirectToAction("Completed");
        }


        public ActionResult Completed()
        {

            return View();
        }


        [ChildActionOnly]
        public ActionResult ManualPaymentInfo()
        {
            var model = new ManualPaymentInfoModel();
            // years
            //CC types
            model.CreditCardTypes.Add(new SelectListItem
            {
                Text = "Visa",
                Value = "Visa",
            });
            model.CreditCardTypes.Add(new SelectListItem
            {
                Text = "Master card",
                Value = "MasterCard",
            });
          
            for (int i = 0; i < 15; i++)
            {
                string year =(DateTime.Now.AddYears(i)).ToString("yy");
                model.ExpireYears.Add(new SelectListItem { Text = year, Value = year });
            }

            // months
            for (int i = 1; i <= 12; i++)
            {
                string text = (i < 10) ? "0" + i.ToString() : i.ToString();
                model.ExpireMonths.Add(new SelectListItem { Text = text, Value = i.ToString() });
            }

            
         

            return PartialView(model);
        }

        [NonAction]
        private  IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();

            //validate
            
            var model = new ManualPaymentInfoModel
            {
                CardholderName = form["CardholderName"],
                CardNumber = form["CardNumber"],
                CardCode = form["CardCode"],
                ExpireMonth = form["ExpireMonth"],
                ExpireYear = form["ExpireYear"],
                CreditCardType = form["CreditCardType"],
                PinCode = form["PinCode"]
            };

            if (!TryValidateModel(model))
            {
                warnings.Add("Error");
            }
            return warnings;
        }
    }
}