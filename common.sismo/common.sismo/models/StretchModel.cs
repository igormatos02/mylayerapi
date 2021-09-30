using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class StretchModel
    {
        public StretchModel()
        {
            Points = new List<decimal>();
        }
        public StretchModel(string line, int stretchIndex, bool isFromDb)
        {
            Points = new List<decimal>();
            Index = stretchIndex;
            Line = line;
            IsFromDb = isFromDb;
            IsLocked = true;
        }
        public string DateString { get; set; }
        public DateTime Date { get; set; }
        public int SurveyId { get; set; }
        public string FrontGroupName { get; set; }
        public int FrontGroupId { get; set; }
        public int OperationalFrontId { get; set; }
        public string FrontGroupLeaderName { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public int Index { get; set; }
        public String Line { get; set; }
        public Decimal InitialStation { get; set; }
        public Decimal FinalStation { get; set; }
        public Decimal InitialStationBkp { get; set; }
        public Decimal FinalStationBkp { get; set; }
        public List<Decimal> Points { get; set; }
        public int PointsCount { get; set; }
        public Boolean IsFromDb { get; set; }
        public Boolean IsLocked { get; set; }
        public int? RealizedCount { get; set; }
        public int? RealizedOnlyStationsCount { get; set; }
        public int? NotRealizedCount { get; set; }
        //public int? PendingCount { get; set; }
        public decimal? TotalKm { get; set; }
        public Boolean KmLeft { get; set; }
        public Boolean KmRight { get; set; }
        public Int32 NumberOfChargesInShotPoint { get; set; }
        public Int32 NumberOfFusesInShotPoint { get; set; }
    }
}
