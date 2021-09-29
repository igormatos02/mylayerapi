using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class OperationalFront
    {
        public OperationalFront()
        {
            FrontGroupLeaders = new HashSet<FrontGroupLeader>();
            FrontGroups = new HashSet<FrontGroup>();
            InversePreviousOperationalFront = new HashSet<OperationalFront>();
            PlannedStretches = new HashSet<PlannedStretch>();
            PointProductions = new HashSet<PointProduction>();
            Stretches = new HashSet<Stretch>();
            SurveyOperationalFronts = new HashSet<SurveyOperationalFront>();
        }

        public int OperationalFrontId { get; set; }
        public int OperationalFrontType { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int? PreviousOperationalFrontId { get; set; }
        public string OperationalFrontColor { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? OwnerId { get; set; }
        public int? OnlineOperationalFrontId { get; set; }
        public int? OnlinePreviousOperationalFrontId { get; set; }
        public int? OnlineProjectd { get; set; }

        public virtual OperationalFront PreviousOperationalFront { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<FrontGroupLeader> FrontGroupLeaders { get; set; }
        public virtual ICollection<FrontGroup> FrontGroups { get; set; }
        public virtual ICollection<OperationalFront> InversePreviousOperationalFront { get; set; }
        public virtual ICollection<PlannedStretch> PlannedStretches { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
        public virtual ICollection<Stretch> Stretches { get; set; }
        public virtual ICollection<SurveyOperationalFront> SurveyOperationalFronts { get; set; }
    }
}
