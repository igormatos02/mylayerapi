namespace common.sismo.models
{
    public class SRSModel
    {
        public string SRSName { get; set; }
        public int SRSId { get; set; }
        public int? CentralMeridian { get; set; }
        public string WKT { get; set; }
        public bool IsActive { get; set; }
    }
}
