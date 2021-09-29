using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ProjectBaseImage
    {
        public int ProjectBaseId { get; set; }
        public string Image { get; set; }

        public virtual ProjectBase ProjectBase { get; set; }
    }
}
