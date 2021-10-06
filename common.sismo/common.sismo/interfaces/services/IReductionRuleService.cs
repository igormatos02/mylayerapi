using common.sismo.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IReductionRuleService
    {
        Task<ReductionRuleModel> GetReductionRule(int reductionRuleId);
        Task<List<ReductionRuleModel>> ListReductionRules(int surveyId);
        Task<List<ReductionRuleModel>> ListAllReductionRules();
        Task<ReductionRuleModel> SaveDisplacementRule(Stream fileStream, ReductionRuleModel model, String fileExtension);
    }
}
