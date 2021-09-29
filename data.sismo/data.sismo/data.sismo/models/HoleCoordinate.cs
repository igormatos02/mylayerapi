using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class HoleCoordinate
    {
        public int? PreplotPointId { get; set; }
        public int SurveyId { get; set; }
        public int? HoleNumber { get; set; }
        public DateTime AcquisitionTime { get; set; }
        public int? CreatorUserId { get; set; }
        public string Line { get; set; }
        public int FileId { get; set; }
        public decimal? StationNumber { get; set; }
        public int HoleCoordinateId { get; set; }

        public virtual HolesCoordinatesFile File { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
