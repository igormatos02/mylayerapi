using System;

namespace common.sismo.models
{
    public class PointOfInterestModel
    {
        public string CoordinateX { get; set; }
        public string CoordinateY { get; set; }
        public int PointOfInterestId { get; set; }
        public String Description { get; set; }
        public String Text { get; set; }
        public int? SurveyId { get; set; }
        public int? Type { get; set; }
        public String Responsable { get; set; }
        public String ImagePath { get; set; }
        public String Date { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
