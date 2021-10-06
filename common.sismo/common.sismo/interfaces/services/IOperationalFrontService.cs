using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IOperationalFrontService
    {
        Task<OperationalFrontModel> GetOperationalFront(Int32 operationalFrontId);
        Task<List<List<OperationalFrontProductionModel>>> GetOperationalFrontProduction(int surveyId, string date);
        Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId);
        Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId);
        Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId, int operationalFrontType);
        Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId, int opFrontIdToExclude);
        Task<object> ListOperationalFrontTypes();
        Task<OperationalFrontModel> SaveOperationalFront(OperationalFrontModel operationalFront);
        Task<OperationalFrontModel> DeleteOperationalFront(OperationalFrontModel operationalFront);
    }
}
