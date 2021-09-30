using System;

namespace common.sismo.models
{
    public class PointDetailModel
    {
        public DateTime ProductionDate { get; set; }
        public String ProductionDateStr { get; set; }
        public Int32 Status { get; set; }
        public String StatusName { get; set; }
        public String Line { get; set; }
        public Decimal StationNumber { get; set; }
        public String Reason { get; set; }
        public Int32 PreplotPointId { get; set; }
        public String OperationalFrontName { get; set; }
        public String OperationalFrontColor { get; set; }
        public String FrontGroupName { get; set; }
        public String FrontGroupLeaderName { get; set; }
        public Int32 HasDisplacementRule { get; set; }
        public String DisplacementRuleImage { get; set; }
        public Int32 HasReductionRule { get; set; }
        public String ReductionRuleImage { get; set; }
    }
}
