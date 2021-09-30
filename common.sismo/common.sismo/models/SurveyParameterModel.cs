using System;

namespace common.sismo.models
{
    public class SurveyParameterModel
    {
        public int SurveyId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
        public String Key { get; set; }
    }
}
