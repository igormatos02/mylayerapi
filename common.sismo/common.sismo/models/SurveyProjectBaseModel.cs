using System;

namespace common.sismo.models
{
    public class SurveyProjectBaseModel
    {
        public int ProjectBaseId { get; set; }
        public int SurveyId { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
