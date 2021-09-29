using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SurveyPointsOfInterest
    {
        public int PointOfInterestId { get; set; }
        public int? SurveyId { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public string Responsable { get; set; }
        public DateTime? Date { get; set; }
        public int? Type { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
