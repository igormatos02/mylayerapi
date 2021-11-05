using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IParameterGroupService
    {
      Task<ParameterGroupModel> GetParameterGroup(int parameterGroupId);
      Task<string> GetSurveyParameterValue(Int32 surveyId, String key);

      Task<List<ParameterGroupModel>> ListParameterGroups();

      Task<List<ParameterModel>> ListParameters();

      Task<List<ParameterGroupModel>> ListParameterGroup();
    }
}
