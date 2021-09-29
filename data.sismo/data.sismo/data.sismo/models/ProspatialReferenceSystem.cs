using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ProspatialReferenceSystem
    {
        public ProspatialReferenceSystem()
        {
            SurveyCoordinateSystems = new HashSet<SurveyCoordinateSystem>();
        }

        public int SpatialReferenceId { get; set; }
        public string WellKnownText { get; set; }
        public int? CentralMeridian { get; set; }
        public string Geogcs { get; set; }

        public virtual ICollection<SurveyCoordinateSystem> SurveyCoordinateSystems { get; set; }
    }
}
