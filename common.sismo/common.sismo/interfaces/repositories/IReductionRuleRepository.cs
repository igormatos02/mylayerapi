using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IReductionRuleRepository
    {
        Task<ReductionRuleModel> GetReductionRule(int reductionRuleId);
        Task<int> GetReductionFinalHolesQuantity(int reductionRuleId);
        Task<decimal> GetGreatestDistanceBetweenEndHoles(int surveyId);
        Task<List<ReductionRuleModel>> ListReductionRules(int idSurvey);
        Task<List<ReductionRuleModel>> ListReductionRules(int idSurvey, bool isActive);
        Task<List<ReductionRuleModel>> ListAllReductionRules();
        Task<ReductionRuleModel> SaveReductionRules(ReductionRuleModel model);
        Task UpdateReductionRule(ReductionRuleModel model);
    }
}
