using System.Collections.Generic;

namespace common.sismo.models
{
    public class SurveyDetailedModel
    {
        public SurveyModel Survey { get; set; }
        public List<SRSModel> SRS { get; set; }
        public List<ParameterGroupModel> ParameterGroups { get; set; }
        public List<OperationalFrontModel> OperationalFronts { get; set; }
        public List<ProjectBaseModel> ProjectBases { get; set; }
        public List<ReductionRuleModel> ReductionRules { get; set; }
        public List<DisplacementRuleModel> DisplacementRules { get; set; }
        public bool IsActive { get; set; }
    }
}
