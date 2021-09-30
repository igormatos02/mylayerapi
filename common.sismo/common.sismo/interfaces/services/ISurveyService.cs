using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ISurveyService
    {
        Task<SurveyModel> GetSurvey(Int32 surveyId);
    }
}
