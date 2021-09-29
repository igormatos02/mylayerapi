using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class PreplotLastVersionView
    {
        public long Gid { get; set; }
        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int PreplotPointType { get; set; }
        public string Line { get; set; }
        public decimal StationNumber { get; set; }
        public int? LandId { get; set; }
        public bool IsActive { get; set; }
    }
}
