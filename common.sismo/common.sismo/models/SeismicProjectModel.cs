using System;

namespace common.sismo.models
{
    public class SeismicProjectModel
    {
        public int ProjectId { get; set; }
        public int? OnlineProjectId { get; set; }
        public int? OwnerId { get; set; }

        public int? ExplosiveMax { get; set; }
        public int? FuseMax { get; set; }
        public string CR { get; set; }
        public string CRObservation { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string ProjectType { get; set; }
        public bool IsActive { get; set; }
        public string DateIni { get; set; }
        public string DateEnd { get; set; }
        public DateTime LastUpdate { get; set; }

        public Int32 CountryId { get; set; }
        public Int32 StateId { get; set; }
    }
}
