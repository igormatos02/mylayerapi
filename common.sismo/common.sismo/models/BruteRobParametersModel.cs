using System.Collections.Generic;

namespace common.sismo.models
{
    public class BruteRobParametersModel
    {
        public int SurveyId { get; set; }
        public int OperationalFrontId { get; set; }
        public int FrontGroupId { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public string Date { get; set; }
        public List<LineStretchModel> LineStretches { get; set; }
        public bool IsActive { get; set; }
    }
}
