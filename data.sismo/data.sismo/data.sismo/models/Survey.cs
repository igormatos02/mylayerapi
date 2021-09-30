using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Survey
    {
        public Survey()
        {
            DisplacementRules = new HashSet<DisplacementRule>();
            HoleCoordinates = new HashSet<HoleCoordinate>();
            Holes = new HashSet<Hole>();
            Lines = new HashSet<Line>();
            PlannedStretches = new HashSet<PlannedStretch>();
            PointProductions = new HashSet<PointProduction>();
            PosplotCoordinates = new HashSet<PosplotCoordinate>();
            PreplotPoints = new HashSet<PreplotPoint>();
            ProjectExplosiveMaterials = new HashSet<ProjectExplosiveMaterial>();
            ReductionRules = new HashSet<ReductionRule>();
            Stretches = new HashSet<Stretch>();
            SurveyCoordinateSystems = new HashSet<SurveyCoordinateSystem>();
            SurveyOperationalFronts = new HashSet<SurveyOperationalFront>();
            SurveyParameters = new HashSet<SurveyParameter>();
            SurveyPointsOfInterests = new HashSet<SurveyPointsOfInterest>();
            SurveyProjectBases = new HashSet<SurveyProjectBase>();
            Swaths = new HashSet<Swath>();
            TerrainChargeControls = new HashSet<TerrainChargeControl>();
        }

        public int SurveyId { get; set; }
        public string Name { get; set; }
        public int CoordinateSystem { get; set; }
        public string Dimension { get; set; }
        public string LastEditorUserLogin { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateIni { get; set; }
        public int? HolesPerShotPoint { get; set; }
        public string HolesArrangementImagePath { get; set; }
        public decimal? HolesDepth { get; set; }
        public DateTime? DateEnd { get; set; }
        public string HolesArrangementDescription { get; set; }
        public string GpsdatabaseName { get; set; }
        public int DatumId { get; set; }
        public string PolygonColor { get; set; }
        public short? BaseMapId { get; set; }
        public decimal? HolesBufferSizeFactor { get; set; }
        public decimal? DistanceBetweenHoles { get; set; }
        public DateTime LastUpdate { get; set; }
        public int? OwnerId { get; set; }
        public int? OnlineSurveyId { get; set; }
        public int? OnlineProjectId { get; set; }
        public int? TotalPoints { get; set; }
        public int? TotalPointsEr { get; set; }
        public int? TotalPointsPt { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<DisplacementRule> DisplacementRules { get; set; }
        public virtual ICollection<HoleCoordinate> HoleCoordinates { get; set; }
        public virtual ICollection<Hole> Holes { get; set; }
        public virtual ICollection<Line> Lines { get; set; }
        public virtual ICollection<PlannedStretch> PlannedStretches { get; set; }
        public virtual ICollection<PointProduction> PointProductions { get; set; }
        public virtual ICollection<PosplotCoordinate> PosplotCoordinates { get; set; }
        public virtual ICollection<PreplotPoint> PreplotPoints { get; set; }
        public virtual ICollection<ProjectExplosiveMaterial> ProjectExplosiveMaterials { get; set; }
        public virtual ICollection<ReductionRule> ReductionRules { get; set; }
        public virtual ICollection<Stretch> Stretches { get; set; }
        public virtual Geometry Polygon { get; set; }
        public virtual ICollection<SurveyCoordinateSystem> SurveyCoordinateSystems { get; set; }
        public virtual ICollection<SurveyOperationalFront> SurveyOperationalFronts { get; set; }
        public virtual ICollection<SurveyParameter> SurveyParameters { get; set; }
        public virtual ICollection<SurveyPointsOfInterest> SurveyPointsOfInterests { get; set; }
        public virtual ICollection<SurveyProjectBase> SurveyProjectBases { get; set; }
        public virtual ICollection<Swath> Swaths { get; set; }
        public virtual ICollection<TerrainChargeControl> TerrainChargeControls { get; set; }
    }
}
