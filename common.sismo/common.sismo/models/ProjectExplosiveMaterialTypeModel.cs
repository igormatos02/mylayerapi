namespace common.sismo.models
{
    public class ProjectExplosiveMaterialTypeModel
    {
        public int ProjectExplosiveMaterialTypeId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public decimal Volume { get; set; }
        public string Unity { get; set; }
        public int RemainingPoints { get; set; }
        public bool IsActive { get; set; }
    }
}
