using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ProjectBase
    {
        public ProjectBase()
        {
            ProjectBaseImages = new HashSet<ProjectBaseImage>();
            SurveyProjectBases = new HashSet<SurveyProjectBase>();
        }

        public int BaseId { get; set; }
        public string Description { get; set; }
        public string ImageFolder { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsChecked { get; set; }
        public string BaseName { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectBaseImage> ProjectBaseImages { get; set; }
        public virtual ICollection<SurveyProjectBase> SurveyProjectBases { get; set; }
    }
}
