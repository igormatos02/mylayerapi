using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Project
    {
        public Project()
        {
            OperationalFronts = new HashSet<OperationalFront>();
            ProjectBases = new HashSet<ProjectBase>();
            ProjectExplosiveMaterialTypes = new HashSet<ProjectExplosiveMaterialType>();
            ProjectExplosiveMaterials = new HashSet<ProjectExplosiveMaterial>();
            Surveys = new HashSet<Survey>();
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string ProjectType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateIni { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Cr { get; set; }
        public int? ExplosiveMax { get; set; }
        public int? FuseMax { get; set; }
        public string Crobservation { get; set; }
        public int? ProjectBusinessType { get; set; }
        public int? OwnerId { get; set; }
        public int? OnlineProjectId { get; set; }

        public virtual ICollection<OperationalFront> OperationalFronts { get; set; }
        public virtual ICollection<ProjectBase> ProjectBases { get; set; }
        public virtual ICollection<ProjectExplosiveMaterialType> ProjectExplosiveMaterialTypes { get; set; }
        public virtual ICollection<ProjectExplosiveMaterial> ProjectExplosiveMaterials { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
