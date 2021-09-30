using System;

namespace common.sismo.models
{
    public class SurveyOperationalFrontModel
    {
        public int OperationalFrontId { get; set; }
        public int SurveyId { get; set; }

        public Boolean IsActive { get; set; }
    }
}
