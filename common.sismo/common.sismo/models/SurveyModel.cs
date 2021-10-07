using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class SurveyModel
    {
        public SurveyModel()
        {
            SurveyId = 0;
            Project = new SeismicProjectModel();
            Parameters = new List<ParameterModel>();
            OperationalFrontsIds = new List<int>();
        }
        public int SurveyId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public int CoordinateSystem { get; set; }
        public int CoordinateSystemId { get; set; }
        public int DatumId { get; set; }
        public int ZoneId { get; set; }
        public string Dimension { get; set; }
        public string LastEditorUser { get; set; }
        public string DateIni { get; set; }
        public string DateEnd { get; set; }
        public bool IsActive { get; set; }
        public string PolygonWKT { get; set; }
        public string PolygonColor { get; set; }
        public List<ParameterModel> Parameters { get; set; }
        public SeismicProjectModel Project { get; set; }
        public List<OperationalFrontModel> OperationalFronts { get; set; }
        public List<ProjectBaseModel> ProjectBases { get; set; }
        public List<int> OperationalFrontsIds { get; set; }
        public List<int> ProjectBasesIds { get; set; }
        public int? HolesPerShotPoint { get; set; }
        public decimal? HolesDepth { get; set; }
        public double CenterMapX { get; set; }
        public double CenterMapY { get; set; }
        public string HolesArrangementImagePath { get; set; }
        public string HolesArrangementDescription { get; set; }
        public string GPSDatabaseName { get; set; }
        public int BaseMapId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public DateTime LastUpdate { get; set; }
       
        public int ProfileId { get; set; }
        public decimal DistanceBetweenHoles { get; set; }
        public String SRSName { get; set; }
        public String CoordinateX { get; set; }
        public String CoordinateY { get; set; }
    }
}
