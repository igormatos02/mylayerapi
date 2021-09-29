using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class DisplacementRule
    {
        public DisplacementRule()
        {
            PointProductions = new HashSet<PointProduction>();
        }

        public int DisplacementRuleId { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public decimal Azymuth { get; set; }
        public decimal Distance { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public int? DisplacementRuleType { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? OnlineSurveyId { get; set; }
        public int? OnlineDisplacementRuleId { get; set; }
        public bool? Uploaded { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
    }
}
