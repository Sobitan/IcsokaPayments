using System;
using IcsokaPayments.Service.Payments;
using IcsokaPayments.Web.Core.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IcsokaPayments.UnitTest
{
    [TestClass]
    public class FlutterWaveUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var setting = new FlutterWaveSetting()
            {
                MerchantKey = "tk_Bg36tT5iby",
                APIKey = "tk_RknixtdDhIumEZU4jTF7",
                UseSandBox = true,
                ValidateOption = "SMS",
                AuthModel = "PIN",
                Country = "NG",
                Currency = "NGN"
            };
      
            var flutterWave = new FlutterWave(setting);
            var request = new ProcessPaymentRequest()
            {
                CreditCardCvv2 = "373",
                CreditCardExpireMonth = 6,
                CreditCardExpireYear = 19,
                CreditCardNumber = "5455844437306780",
                ClientToken = "1111",
                CreditCardType = "VISA",
                AmountTotal = 50,
                Narration = "Test payment",
                Beneficiary =  string.Format("cust{0}", DateTime.Now )
            };
            var tokenize = flutterWave.ProcessPayment(request);

            //var charge = flutterWave.PostProcessPayment(request);

            Assert.IsTrue(tokenize.Success);
        }
    }
}
