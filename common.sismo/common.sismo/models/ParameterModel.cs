using System;

namespace common.sismo.models
{
    public class ParameterModel
    {
        public int ParameterId { get; set; }
        public int ParameterGroupId { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParameterType { get; set; }
        public string Format { get; set; }
        public string ParameterOwner { get; set; }
        public string VarType { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
