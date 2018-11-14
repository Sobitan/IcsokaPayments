using IcsokaPayments.Service.Configurations;

namespace IcsokaPayments.Web.Core.Processors
{
    public class FlutterWaveSetting:ISettings
    {
        public string MerchantKey { get; set; }
        public string APIKey { get; set; }
        public bool UseSandBox { get; set; }
        public string ValidateOption { get; set; }

        public string AuthModel { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
    }
}