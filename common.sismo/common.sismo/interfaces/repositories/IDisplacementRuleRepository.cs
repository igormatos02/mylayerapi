using common.sismo.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IDisplacementRuleRepository
    {
        Task<DisplacementRuleModel> GetDisplacementRule(int displacementRuelId);
        Task<List<DisplacementRuleModel>> ListProjects(int surveyId, bool onlyActives, int type);
        Task<List<DisplacementRuleModel>> ListProjects();
        Task<DisplacementRuleModel> SaveDisplacementRules(DisplacementRuleModel model);
        Task UpdateDisplacementRule(DisplacementRuleModel model);
    }
}
