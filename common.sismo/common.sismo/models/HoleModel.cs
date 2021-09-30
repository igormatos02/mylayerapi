using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class HoleModel
    {
        public decimal Depth { get; set; }
        public int? ChargesTypeId { get; set; }
        public int? FusesTypeId { get; set; }
        public decimal? NumberOfCharges { get; set; }
        public decimal? NumberOfFuses { get; set; }
        public int HoleNumber { get; set; }
        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int WorkNumber { get; set; }
        public PreplotPointType PreplotPointType { get; set; }
        public int OperationalFrontId { get; set; }
        public int? FrontGroupId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Date { get; set; }
    }
}
