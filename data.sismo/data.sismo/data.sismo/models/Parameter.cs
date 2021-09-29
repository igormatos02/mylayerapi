using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class Parameter
    {
        public Parameter()
        {
            SurveyParameters = new HashSet<SurveyParameter>();
        }

        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParameterType { get; set; }
        public bool IsActive { get; set; }
        public string Format { get; set; }
        public int ParameterGroupId { get; set; }
        public string ParameterOwner { get; set; }
        public string VarType { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ParameterGroup ParameterGroup { get; set; }
        public virtual ICollection<SurveyParameter> SurveyParameters { get; set; }
    }
}
