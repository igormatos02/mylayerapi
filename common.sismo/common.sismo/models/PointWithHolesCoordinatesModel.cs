using System.Collections.Generic;

namespace common.sismo.models
{
    public class PointWithHolesCoordinatesModel
    {
        public PointWithHolesCoordinatesModel()
        {
            HolesCoordinates = new List<HoleCoordinateModel>();
        }
        public string Line { get; set; }
        public decimal StationNumber { get; set; }
        public int? PreplotPointId { get; set; }
        public string DisplacementRule { get; set; }
        public string DisplacementRuleImagePath { get; set; }
        public string ReductionRule { get; set; }
        public string ReductionRuleImagePath { get; set; }
        public string ChargingFrontObservation { get; set; }
        public string PosplotCoordinateX { get; set; }
        public string PosplotCoordinateY { get; set; }
        public string PosplotCoordinateZ { get; set; }
        public string PosplotRegistrationTime { get; set; }
        public IList<HoleCoordinateModel> HolesCoordinates { get; set; }
        public string Alert { get; set; }
    }
}
