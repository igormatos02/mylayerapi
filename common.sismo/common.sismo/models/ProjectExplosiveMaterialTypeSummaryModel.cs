using System.Collections.Generic;

namespace common.sismo.models
{
    public class ProjectExplosiveMaterialTypeSummaryModel
    {
        public int? TotalTypeMaterialsIn { get; set; }
        public int? TotalAmountAvailableStock { get; set; }
        public int? TotalAmountAvailableField { get; set; }
        public decimal? TotalMaterialVolumeAvailableStock { get; set; }
        public int? TotalMaterialsTypeUse { get; set; }

        public decimal? TotalMaterialsIn { get; set; }
        public decimal? TotalMaterialsVolumeOutDestroyed { get; set; }
        public decimal? TotalMaterialsVolumeOutTransfered { get; set; }
        public ProjectExplosiveMaterialTypeModel MaterialType { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProjectExplosiveMaterialDataModel
    {
        public List<ProjectExplosiveMaterialModel> Materials { get; set; }
        public decimal TotalVolumeKg { get; set; }
        public decimal TotalVolumeM { get; set; }
        public decimal TotalProjectVolumeKg { get; set; }
        public decimal TotalProjectVolumeM { get; set; }
        public decimal TotalSurveyVolumeKg { get; set; }
        public decimal TotalSurveyVolumeM { get; set; }
    }

    public class ProjectExplosiveMaterialTypeSummaryDataModel
    {
        public List<ProjectExplosiveMaterialTypeModel> MaterialTypes { get; set; }
        public List<ProjectExplosiveMaterialTypeSummaryModel> ProjectExplosiveMaterialTypeSummaryList { get; set; }
        public List<string> DatesToReturn { get; set; }
        public DecimalChartSerie ExplosiveStock { get; set; }
        public DecimalChartSerie FuseStock { get; set; }
        public DecimalChartSerie ExplosiveUsed { get; set; }
        public DecimalChartSerie FuseUsed { get; set; }
    }
}
