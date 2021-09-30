using System;

namespace common.sismo.models
{
    public class LandStretchModel
    {
        public int LandId { get; set; }
        public int SurveyId { get; set; }
        public String Line { get; set; }
        public decimal InitialStation { get; set; }
        public decimal FinalStation { get; set; }
        public decimal? IndemnityValue { get; set; }
        public int CultivationId { get; set; }
        public int? StationsCount { get; set; }
        public decimal? Km { get; set; }
    }
}
