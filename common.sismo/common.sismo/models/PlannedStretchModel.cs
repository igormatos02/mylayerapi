using NetTopologySuite.Geometries;
using System;

namespace common.sismo.models
{
    public class PlannedStretchModel
    {
        public string ExecutionDateString { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string PlanningDateTimeString { get; set; }
        public DateTime PlanningDateTime { get; set; }
        public int SurveyId { get; set; }
        public string FrontGroupName { get; set; }
        public int? FrontGroupId { get; set; }
        public int OperationalFrontId { get; set; }
        public string OperationalFrontName { get; set; }
        public string FrontGroupLeaderName { get; set; }
        public int? FrontGroupLeaderId { get; set; }
        public String Line { get; set; }
        public Decimal InitialStation { get; set; }
        public Decimal FinalStation { get; set; }
        public int TotalStations { get; set; }
        public decimal? Km2 { get; set; }
        public decimal? Km { get; set; }
        public Boolean? KmLeft { get; set; }
        public Boolean? KmRight { get; set; }
        public Geometry StretchLineGeometry { get; set; }
        public int PlannerUserId { get; set; }
        public bool NotRealized { get; set; }
        public bool IsActive { get; set; }
    }
}
