using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class ProductionScreenDetailsModel
    {
        public List<FrontGroupModel> Groups { get; set; }
        public List<FrontGroupLeaderModel> Leaders { get; set; }
        public List<string> Lines { get; set; }
        public List<GenericValueModel> StatusList { get; set; }
        public List<DisplacementRuleModel> DisplacementRules { get; set; }
        public List<ReductionRuleModel> ReductionRules { get; set; }
        public List<FrontGroupProductionModel> StretchGroups { get; set; }
        public object QcStatusList { get; set; }
    }
}
