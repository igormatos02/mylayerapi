using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class ProjectExplosiveMaterialModel
    {
        public int EntryId { get; set; }
        public int ProjectExplosiveMaterialTypeId { get; set; }
        public int ProjectId { get; set; }
        public int? SurveyId { get; set; }
        public String Date { get; set; }
        public int? AmountIn { get; set; }
        public int? AmountOut { get; set; }
        public int? AmountUsed { get; set; }
        public string Manufacturer { get; set; }
        public string TrafficGuide { get; set; }
        public string Invoice { get; set; }
        public int? DifferenceAmount { get; set; }
        public String DifferenceDate { get; set; }
        public String Survey { get; set; }
        public String DestinyDescription { get; set; }
        public string Observation { get; set; }
        public string ProjectExplosiveMaterialType { get; set; }
        public int? TotalAmountIn { get; set; }
        public int? TotalAmountOut { get; set; }
        public int? TotalAmountUsed { get; set; }
        public int? TotalAvailableStock { get; set; }
        public int? TotalAvailableField { get; set; }
        public int? TotalDifferenceAmount { get; set; }
        public bool IsActive { get; set; }
        public ExplosiveMaterialEntryType EntryType { get; set; }
        public ExplosiveMaterialDestiny Destiny { get; set; }
        public decimal? Volume { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
