using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Routing;
using IcsokaPayments.Domain.Enums;
using IcsokaPayments.Service.Payments;
using Newtonsoft.Json;

namespace IcsokaPayments.Web.Core.Processors
{
    public class FlutterWave:  PaymentMethodBase
    {

        private readonly FlutterWaveSetting _flutterWaveSetting;

        public FlutterWave(FlutterWaveSetting flutterWaveSetting)
        {
            _flutterWaveSetting = flutterWaveSetting;
        }

       

        public override ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            var request = GetHttpClient();
            var strObj = JsonConvert.SerializeObject(new FlutterWaveRequest()
            {
                authmodel = Encrypt(_flutterWaveSetting.AuthModel, _flutterWaveSetting.APIKey),
                cardno = Encrypt(processPaymentRequest.CreditCardNumber, _flutterWaveSetting.APIKey),
                currency = Encrypt(_flutterWaveSetting.Currency, _flutterWaveSetting.APIKey),
                country = Encrypt(_flutterWaveSetting.Country, _flutterWaveSetting.APIKey),
                cvv = Encrypt(processPaymentRequest.CreditCardCvv2, _flutterWaveSetting.APIKey),
                pin = Encrypt(processPaymentRequest.ClientToken, _flutterWaveSetting.APIKey),
                cardtype = Encrypt(processPaymentRequest.CreditCardType, _flutterWaveSetting.APIKey),
                expirymonth = Encrypt(processPaymentRequest.CreditCardExpireMonth.ToString("D2"), _flutterWaveSetting.APIKey),
                expiryyear = Encrypt(processPaymentRequest.CreditCardExpireYear.ToString(), _flutterWaveSetting.APIKey),
                merchantid = _flutterWaveSetting.MerchantKey,
                narration = Encrypt(processPaymentRequest.Narration, _flutterWaveSetting.APIKey),
                amount = Encrypt(processPaymentRequest.AmountTotal.ToString("##.###"), _flutterWaveSetting.APIKey),
                custid = Encrypt(processPaymentRequest.Beneficiary, _flutterWaveSetting.APIKey)
            });
            var content = new StringContent(strObj, Encoding.UTF8,
                                    "application/json");
            try
            {
                var httpResponseMessage = request.PostAsync("/pwc/rest/card/mvva/pay", content).Result;
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var stringResponse = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<dynamic>(stringResponse);
                    if ((string)response.status == "success")
                    {
                        var r = JsonConvert.DeserializeObject<FlutterWaveResponse<FlutterWaveData>>(stringResponse);
                        if (r.Data.responsecode == "00")
                        {
                            result.AuthorizationTransactionId = r.Data.transactionreference;
                            result.AuthorizationTransactionCode = string.Format("{0},{1}", r.Data.responsecode,  r.Data.responsemessage);
                            result.AuthorizationTransactionResult = string.Format("Approved ({0}: {1})", r.Data.transactionreference, r.Data.responsetoken);
                            result.AvsResult = r.Data.avsresponsecode;
                            result.NewPaymentStatus = PaymentStatus.Paid;
                        }
                        else
                        {
                            result.AddError(string.Format("Declined ({0}: {1})", processPaymentRequest.CreditCardNumber, r.Status));
                        }
                    }
                    else
                    {
                        result.AddError((string)response.data);
                    }
                }
                else
                {
                    result.AddError("FlutterWave unknown error");
                }
            }
            catch (Exception exception)
            {

                Debug.WriteLine(exception);
                result.AddError(exception.Message);
            }

            return result;
        }

       

       

        private string Encrypt(string source, string key)
        {
            var desCryptoProvider = new TripleDESCryptoServiceProvider();
            var hashMd5Provider = new MD5CryptoServiceProvider();

            var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            var byteBuff = Encoding.UTF8.GetBytes(source);

            var encoded =
                Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return encoded;
        }


        private static string Decrypt(string encodedText, string key)
        {
            var desCryptoProvider = new TripleDESCryptoServiceProvider();
            var hashMd5Provider = new MD5CryptoServiceProvider();

            var byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            var byteBuff = Convert.FromBase64String(encodedText);

            var plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;
        }


        private string GetFlutterWaveUrl()
        {
            return _flutterWaveSetting.UseSandBox ? "http://staging1flutterwave.co:8080" : "https://prod1flutterwave.co:8181";
        }

        private HttpClient GetHttpClient()
        {
            var url = string.Format("{0}", GetFlutterWaveUrl());
            var client = new HttpClient()
            {
                BaseAddress = new Uri(url),
                
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromHours(1);
            return  client;
        }

        public override Type GetControllerType()
        {
            return typeof(FlutterWaveController);
        }

        public override void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            throw new NotImplementedException();
        }

        public override void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaymentManual";
            routeValues = new RouteValueDictionary { { "Namespaces", "Payments.Manual.Controllers" }, { "area", null } };
        }
    }
}