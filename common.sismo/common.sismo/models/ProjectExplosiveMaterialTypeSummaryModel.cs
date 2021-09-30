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
}
