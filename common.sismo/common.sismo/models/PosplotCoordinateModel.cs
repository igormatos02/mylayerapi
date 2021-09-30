using NetTopologySuite.Geometries;
using System;

namespace common.sismo.models
{
    public class PosplotCoordinateModel
    {
        public DateTime RegistrationTime { get; set; }
        public DateTime? PosplotDate { get; set; }
        public String Date { get; set; }

        public string RegistrationTimeString { get; set; }
        public Geometry Coordinate { get; set; }
        public Geometry PosplotCoordinate { get; set; }
        public Geometry PreplotCoordinate { get; set; }
        public string PreplotCoordinateX { get; set; }
        public string PreplotCoordinateY { get; set; }
        public string PosplotCoordinateX { get; set; }
        public string PosplotCoordinateY { get; set; }
        public string PosplotCoordinateZ { get; set; }
        public string AdjacentPointCoordinateZ { get; set; }
        public decimal AdjacentPointNumber { get; set; }
        public double LocationError { get; set; }
        public double AltimetryVariationMeters { get; set; }
        public double AltimetryVariationDegrees { get; set; }
        public string ReceiversArrangement { get; set; }
        public int SurveyId { get; set; }
        public int OperationalFrontId { get; set; }
        public string CreatorUserLogin { get; set; }
        public bool IsActive { get; set; }
        public Int32? StatusId { get; set; }
        public String Status { get; set; }
        public string TopographyDescription { get; set; }
        public string Type { get; set; }
        public Int32 PreplotPointType { get; set; }
        public string Line { get; set; }
        public decimal StationNumber { get; set; }
        public string LinePreplot { get; set; }
        public decimal? StationNumberPreplot { get; set; }
        public int? PreplotPointId { get; set; }
        public String DeltaN { get; set; }
        public String DeltaE { get; set; }
        public String Distance { get; set; }
        public String DisplacementRule { get; set; }
        public String LocationDistance { get; set; }
        public String AltimetryVariationMetersStr { get; set; }
        public String AltimetryVariationDegreesStr { get; set; }
    }
}
