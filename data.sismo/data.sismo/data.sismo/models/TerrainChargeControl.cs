using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class TerrainChargeControl
    {
        public int TerrainChargeControlId { get; set; }
        public int SurveyId { get; set; }
        public DateTime Date { get; set; }
        public bool? IsFinished { get; set; }
        public int? SwathNumber { get; set; }
        public string OriginalShotLine { get; set; }
        public decimal? OriginalShotPoint { get; set; }
        public string Hole1 { get; set; }
        public string Hole2 { get; set; }
        public string Hole3 { get; set; }
        public string Hole4 { get; set; }
        public string Hole5 { get; set; }
        public string Hole6 { get; set; }
        public string FieldResponsable { get; set; }
        public DateTime? RecoveryDate { get; set; }
        public string RecoveryResponsable { get; set; }
        public string SafetyTechnician { get; set; }
        public string ProvidenceQsms { get; set; }
        public string Reazon { get; set; }
        public string Displacement { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
