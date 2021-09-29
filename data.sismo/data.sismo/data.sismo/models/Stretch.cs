using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Stretch
    {
        public DateTime Date { get; set; }
        public int SurveyId { get; set; }
        public int OperationalFrontId { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public int FrontGroupId { get; set; }
        public decimal InitialStation { get; set; }
        public decimal FinalStation { get; set; }
        public string Line { get; set; }
        public decimal? Km { get; set; }
        public int? TotalRealizedOnlyStations { get; set; }
        public int? TotalRealized { get; set; }
        public int? TotalNotRealized { get; set; }
        public int? TotalPending { get; set; }
        public bool KmRight { get; set; }
        public bool KmLeft { get; set; }

        public virtual FrontGroup FrontGroup { get; set; }
        public virtual FrontGroupLeader FrontGroupLeader { get; set; }
        public virtual OperationalFront OperationalFront { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
