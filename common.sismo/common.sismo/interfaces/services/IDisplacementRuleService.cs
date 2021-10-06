using common.sismo.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IDisplacementRuleService
    {
        Task<DisplacementRuleModel> GetDisplacementRule(int displacementRuleId);
        Task<List<DisplacementRuleModel>> ListDisplacementRules(int surveyId, bool onlyActives, int type);
        Task<List<DisplacementRuleModel>> ListAllDisplacementRules();
        Task<DisplacementRuleModel> SaveDisplacementRule(Stream fileStream, DisplacementRuleModel model, String fileExtension);
    }
}
