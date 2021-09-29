using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ProjectExplosiveMaterial
    {
        public int EntryId { get; set; }
        public int ProjectExplosiveMaterialTypeId { get; set; }
        public int ProjectId { get; set; }
        public int? SurveyId { get; set; }
        public DateTime Date { get; set; }
        public int? AmountIn { get; set; }
        public int? AmountOut { get; set; }
        public int? AmountUsed { get; set; }
        public string Manufacturer { get; set; }
        public string TrafficGuide { get; set; }
        public string Invoice { get; set; }
        public int? DifferenceAmount { get; set; }
        public DateTime? DifferenceDate { get; set; }
        public string Observation { get; set; }
        public int? EntryType { get; set; }
        public int? Destiny { get; set; }
        public decimal? Volume { get; set; }
        public bool IsActive { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectExplosiveMaterialType ProjectExplosiveMaterialType { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
