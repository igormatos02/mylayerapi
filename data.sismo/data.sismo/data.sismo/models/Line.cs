using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Line
    {
        public int SurveyId { get; set; }
        public string LineName { get; set; }
        public decimal TotalKm { get; set; }
        public int TotalPoints { get; set; }
        public int PreplotVersionId { get; set; }
        public decimal InitialStation { get; set; }
        public decimal FinalStation { get; set; }
        public int LinePointsType { get; set; }
        public int? GrowthDirection { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
