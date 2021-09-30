using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class TotalDailyProductionModel
    {
        public Int32 ProjectId { get; set; }
        public Int32 SurveyId { get; set; }
        public Int32 OperationalFrontId { get; set; }
        public String Date { get; set; }
        public DateTime DateTime { get; set; }
        public String Line { get; set; }
        public Int32 TotalRealized { get; set; }
        public Int32 TotalNotRealized { get; set; }
        public Decimal TotalKm { get; set; }
        public Decimal AditionalKm { get; set; }
        public Int32 NumberOfChargesInShotPoint { get; set; }
        public Int32 NumberOfFusesInShotPoint { get; set; }
        public List<StretchModel> Stretches { get; set; }
    }
}
