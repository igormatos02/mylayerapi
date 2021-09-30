using System;

namespace common.sismo.models
{
    public class TerrainChargeControlModel
    {
        public Int32 TerrainChargeControlId { get; set; }
        public Int32 SurveyId { get; set; }
        public String Date { get; set; }
        public Boolean? IsFinished { get; set; }
        public Int32? SwathNumber { get; set; }
        public String OriginalShotLine { get; set; }
        public Decimal? OriginalShotPoint { get; set; }
        public String Hole1 { get; set; }
        public String Hole2 { get; set; }
        public String Hole3 { get; set; }
        public String Hole4 { get; set; }
        public String Hole5 { get; set; }
        public String Hole6 { get; set; }
        public String Displacement { get; set; }
        public String FieldResponsable { get; set; }
        public String RecoveryDate { get; set; }
        public String RecoveryResponsable { get; set; }
        public String SafetyTechnician { get; set; }
        public String ProvidenceQSMS { get; set; }
        public String Reazon { get; set; }
        public bool IsActive { get; set; }
    }
}
