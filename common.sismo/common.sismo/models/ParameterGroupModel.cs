using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class ParameterGroupModel
    {
        public int ParameterGroupId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<ParameterModel> Parameters { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
