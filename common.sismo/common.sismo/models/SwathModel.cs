using System;

namespace common.sismo.models
{
    public class SwathModel
    {
        public Int32 SwathNumber { get; set; }
        public Int32 SurveyId { get; set; }
        public Int32 PreplotVersionId { get; set; }
        public String Name { get; set; }
        public Int32 ActiveReceiverLinesCount { get; set; }
        public string InitialReceiverLine { get; set; }
        public string FinalReceiverLine { get; set; }
        public string InitialShotLine { get; set; }
        public string FinalShotLine { get; set; }
        public Int32 TotalReceiverStationPerSwath { get; set; }
        public decimal InitialShotPoint { get; set; }
        public decimal? FinalReceiverStation { get; set; }
        public decimal? InitialReceiverStation { get; set; }
        public decimal FinalShotPoint { get; set; }
        public Int32 TotalShotPoint { get; set; }
        public decimal? AreaShotPointKm { get; set; }
        public decimal? AreaReceiverStationKm { get; set; }
        public int? TotalForecastTraces { get; set; }
    }
}
