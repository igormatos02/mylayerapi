using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ProjectExplosiveMaterialType
    {
        public ProjectExplosiveMaterialType()
        {
            ProjectExplosiveMaterials = new HashSet<ProjectExplosiveMaterial>();
        }

        public int ProjectExplosiveMaterialTypeId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public decimal Volume { get; set; }
        public string Unity { get; set; }
        public bool IsActive { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectExplosiveMaterial> ProjectExplosiveMaterials { get; set; }
    }
}
