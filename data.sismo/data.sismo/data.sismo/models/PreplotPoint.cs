using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class PreplotPoint
    {
        public PreplotPoint()
        {
            Holes = new HashSet<Hole>();
            PointProductions = new HashSet<PointProduction>();
        }

        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int PreplotPointType { get; set; }
        public string Line { get; set; }
        public decimal StationNumber { get; set; }
        public int? LandId { get; set; }
        public bool IsActive { get; set; }
        public bool? Uploaded { get; set; }

        public virtual PreplotVersion PreplotVersion { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<Hole> Holes { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
    }
}
