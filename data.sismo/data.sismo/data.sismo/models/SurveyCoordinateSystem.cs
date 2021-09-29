using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SurveyCoordinateSystem
    {
        public int SurveyId { get; set; }
        public int SpatialReferenceId { get; set; }

        public virtual ProspatialReferenceSystem SpatialReference { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
