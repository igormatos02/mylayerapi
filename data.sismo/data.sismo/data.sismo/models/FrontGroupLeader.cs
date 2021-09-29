using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class FrontGroupLeader
    {
        public FrontGroupLeader()
        {
            PlannedStretches = new HashSet<PlannedStretch>();
            PointProductions = new HashSet<PointProduction>();
            Stretches = new HashSet<Stretch>();
        }

        public int FrontGroupLeaderId { get; set; }
        public int OperationalFrontId { get; set; }
        public string TeamLeaderUserLogin { get; set; }
        public string Name { get; set; }
        public string LastEditorUserLogin { get; set; }
        public bool IsActive { get; set; }
        public int? OnlineFrontGroupLeaderId { get; set; }
        public int? OnlineOperationalFrontId { get; set; }
        public bool? Uploaded { get; set; }

        public virtual OperationalFront OperationalFront { get; set; }
        public virtual ICollection<PlannedStretch> PlannedStretches { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
        public virtual ICollection<Stretch> Stretches { get; set; }
    }
}
