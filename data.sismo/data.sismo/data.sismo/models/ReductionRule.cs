using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ReductionRule
    {
        public ReductionRule()
        {
            PointProductions = new HashSet<PointProduction>();
        }

        public int ReductionRuleId { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int FinalHolesQuantity { get; set; }
        public decimal DistanceBetweenHoles { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? OnlineSurveyId { get; set; }
        public int? OnlineReductionRuleId { get; set; }
        public bool? Uploaded { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
    }
}
