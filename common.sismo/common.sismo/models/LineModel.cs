using common.sismo.enums;

namespace common.sismo.models
{
    public class LineModel
    {
        public int SurveyId { get; set; }
        public string LineName { get; set; }
        public decimal TotalKm { get; set; }
        public int TotalPoints { get; set; }
        public int PreplotVersionId { get; set; }
        public decimal InitialStation { get; set; }
        public decimal FinalStation { get; set; }
        public PreplotPointType LinePointsType { get; set; }
        public string LineTypeName { get; set; }
        public bool IsActive { get; set; }
        public LineGrowthDirection? GrowthDirection { get; set; }
        public int TotalRealizedPT { get; set; }
        public int TotalRealizedER { get; set; }
        public decimal TotalKmRealized { get; set; }
        public int TotalRealized { get; set; }
        public int TotalNotRealized { get; set; }
        public decimal RemainingKm { get; set; }
        public int RemainingPoints { get; set; }
    }
}
