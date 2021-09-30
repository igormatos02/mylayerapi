using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class DisplacementRuleModel
    {
         public int DisplacementRuleId { get; set; }
        public int SurveyId { get; set; }
        public SurveyModel Survey { get; set; }
        public string Name { get; set; }
        public decimal Azymuth { get; set; }
        public decimal Distance { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DisplacementRuleType DisplacementRuleType { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
