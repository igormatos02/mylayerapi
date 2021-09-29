using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class ParameterGroup
    {
        public ParameterGroup()
        {
            Parameters = new HashSet<Parameter>();
        }

        public int ParameterGroupId { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
