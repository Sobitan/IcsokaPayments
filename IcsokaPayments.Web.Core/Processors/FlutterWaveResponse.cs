namespace IcsokaPayments.Web.Core.Processors
{
    public class FlutterWaveResponse<T>
    {
        public  T Data { get; set; }

        public string Status { get; set; }
    }
}