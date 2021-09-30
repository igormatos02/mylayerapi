using System;

namespace common.sismo.models
{
    public class ReductionRuleModel
    {
        public int ReductionRuleId { get; set; }
        public int SurveyId { get; set; }
        public SurveyModel Survey { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int FinalHolesQuantity { get; set; }
        public decimal DistanceBetweenHoles { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
