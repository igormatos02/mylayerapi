using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Swath
    {
        public int SwathNumber { get; set; }
        public int SurveyId { get; set; }
        public int PreplotVersionId { get; set; }
        public string Name { get; set; }
        public int ActiveReceiverLinesCount { get; set; }
        public decimal? InitialReceiverStation { get; set; }
        public decimal? FinalReceiverStation { get; set; }
        public string InitialReceiverLine { get; set; }
        public string FinalReceiverLine { get; set; }
        public int TotalReceiverStationPerSwath { get; set; }
        public string InitialShotLine { get; set; }
        public string FinalShotLine { get; set; }
        public decimal InitialShotPoint { get; set; }
        public decimal FinalShotPoint { get; set; }
        public int TotalShotPoint { get; set; }
        public decimal? AreaShotPointKm { get; set; }
        public decimal? AreaReceiverStationKm { get; set; }
        public int? TotalForecastTraces { get; set; }
        public int? OnlineSurveyId { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
