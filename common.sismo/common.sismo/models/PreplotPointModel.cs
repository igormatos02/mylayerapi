using common.sismo.enums;
using NetTopologySuite.Geometries;

namespace common.sismo.models
{
    public class PreplotPointModel
    {
        public int PreplotVersionId { get; set; }
        public PreplotVersionModel PreplotVersion { get; set; }
        public string LineName { get; set; }
        public SurveyModel Survey { get; set; }
        public int SurveyId { get; set; }
        public decimal StationNumber { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public Geometry PreplotCoordinate { get; set; }
        public string PreplotCoordinateX { get; set; }
        public string PreplotCoordinateY { get; set; }
        public int CoordinateSystem { get; set; }
        public int PreplotPointId { get; set; }
        public bool IsActive { get; set; }
        public PreplotPointType PreplotPointType { get; set; }
        public string TypeName { get; set; }
        public int? LandId { get; set; }
    }
}
