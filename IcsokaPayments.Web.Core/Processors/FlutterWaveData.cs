// ReSharper disable InconsistentNaming
namespace IcsokaPayments.Web.Core.Processors
{
    public class FlutterWaveData
    {
        public string responsecode { get; set; }
        public string avsresponsemessage { get; set; }
        public string avsresponsecode { get; set; }
        public string responsemessage { get; set; }
        public string otptransactionidentifier { get; set; }
        public string transactionreference { get; set; }
        public string responsehtml { get; set; }
        public string responsetoken { get; set; }
    }
}