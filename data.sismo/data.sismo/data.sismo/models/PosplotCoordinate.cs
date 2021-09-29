using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class PosplotCoordinate
    {
        public DateTime RegistrationTime { get; set; }
        public string CreatorUserLogin { get; set; }
        public string TopographyDescription { get; set; }
        public int SurveyId { get; set; }
        public int OperationalFrontId { get; set; }
        public string Line { get; set; }
        public decimal StationNumber { get; set; }
        public int? PreplotPointId { get; set; }
        public string LinePreplot { get; set; }
        public decimal? StationNumberPreplot { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
