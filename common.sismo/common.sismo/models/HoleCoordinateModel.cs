using NetTopologySuite.Geometries;
using System;

namespace common.sismo.models
{
    public class HoleCoordinateModel
    {
        public int HoleCoordinateId { get; set; }
        public int? PreplotPointId { get; set; }
        public int SurveyId { get; set; }
        public Geometry Coordinate { get; set; }
        public string CoordinateX { get; set; }
        public string CoordinateY { get; set; }
        public string CoordinateZ { get; set; }
        public int? HoleNumber { get; set; }
        public DateTime AcquisitionTime { get; set; }
        public string AcquisitionTimeString { get; set; }
        public int? CreatorUserId { get; set; }
        public string CreatorUserLogin { get; set; }
        public string Line { get; set; }
        public decimal? StationNumber { get; set; }
        public decimal? DistanceToPosplotPoint { get; set; }
        public int FileId { get; set; }
        public bool IsActive { get; set; }
    }
}
