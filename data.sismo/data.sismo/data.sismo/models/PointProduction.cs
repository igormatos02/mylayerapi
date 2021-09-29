using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class PointProduction
    {
        public PointProduction()
        {
            Holes = new HashSet<Hole>();
            SeismicRegisters = new HashSet<SeismicRegister>();
        }

        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int WorkNumber { get; set; }
        public int PreplotPointType { get; set; }
        public int OperationalFrontId { get; set; }
        public string Observation { get; set; }
        public int? ReductionRuleId { get; set; }
        public int? Ffid { get; set; }
        public string LastEditorUserLogin { get; set; }
        public int? DisplacementRuleId { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public int FrontGroupId { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public DateTime Date { get; set; }

        public virtual DisplacementRule DisplacementRule { get; set; }
        public virtual FrontGroup FrontGroup { get; set; }
        public virtual FrontGroupLeader FrontGroupLeader { get; set; }
        public virtual OperationalFront OperationalFront { get; set; }
        public virtual PreplotPoint PreplotPoint { get; set; }
        public virtual PreplotVersion PreplotVersion { get; set; }
        public virtual ReductionRule ReductionRule { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<Hole> Holes { get; set; }
        public virtual ICollection<SeismicRegister> SeismicRegisters { get; set; }
    }
}
