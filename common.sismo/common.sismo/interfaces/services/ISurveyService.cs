using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ISurveyService
    {
        Task<SurveyModel> GetSurvey(Int32 surveyId);
        Task<List<SurveyModel>> ListSurveys(bool isActive);
        Task<List<SurveyModel>> ListSurveys(int projectId);
        Task UpdateSurveyStatus(int surveyId, bool isActive);
    }
}
