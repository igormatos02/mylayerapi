using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Hole
    {
        public decimal Depth { get; set; }
        public int? ChargesType { get; set; }
        public int? FusesType { get; set; }
        public decimal? NumberOfCharges { get; set; }
        public decimal? NumberOfFuses { get; set; }
        public int HoleNumber { get; set; }
        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int WorkNumber { get; set; }
        public int PreplotPointType { get; set; }
        public int OperationalFrontId { get; set; }
        public int? FrontGroupId { get; set; }
        public DateTime? Date { get; set; }

        public virtual PointProduction PointProduction { get; set; }
        public virtual PreplotPoint PreplotPoint { get; set; }
        public virtual PreplotVersion PreplotVersion { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
