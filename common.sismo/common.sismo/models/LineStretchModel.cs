using common.sismo.enums;

namespace common.sismo.models
{
    public class LineStretchModel
    {
        public string Line { get; set; }
        public decimal InitialStation { get; set; }
        public decimal FinalStation { get; set; }
        public PreplotPointType LinePointsType { get; set; }
    }
}
