// ReSharper disable InconsistentNaming
namespace IcsokaPayments.Web.Core.Processors
{
    public class FlutterWaveRequest
    {
       
        public string amount { get; set; }
        public string authmodel { get; set; }
        public string cardno { get; set; }
        public string currency { get; set; }
        public string custid { get; set; }
        public string country { get; set; }
        public string cvv { get; set; }
        public string pin { get; set; }
        public string bvn { get; set; }
        public string cardtype { get; set; }
        public string expirymonth { get; set; }
        public string expiryyear { get; set; }
        public string merchantid { get; set; }
        public string narration { get; set; }
        public string responseurl { get; set; }
    }
}