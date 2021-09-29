using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class PreplotVersion
    {
        public PreplotVersion()
        {
            Holes = new HashSet<Hole>();
            PointProductions = new HashSet<PointProduction>();
            PreplotPoints = new HashSet<PreplotPoint>();
        }

        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatorUserLogin { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Hole> Holes { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
        public virtual ICollection<PreplotPoint> PreplotPoints { get; set; }
    }
}
