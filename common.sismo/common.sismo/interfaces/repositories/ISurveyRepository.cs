using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISurveyRepository
    {
        Task<SurveyModel> GetSurvey(Int32 surveyId);
    }
}
