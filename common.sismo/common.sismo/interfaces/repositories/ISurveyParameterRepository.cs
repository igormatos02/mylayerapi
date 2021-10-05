using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISurveyParameterRepository
    {
        Task<List<SurveyParameterModel>> GetSurveyParameters(int surveyId);
        Task<SurveyParameterModel> GetSurveyParameter(int surveyId, int parameterId);
        Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId);
        Task<SurveyParameterModel> AddSurveyParameter(SurveyParameterModel model);
        Task UpdateSurveyParameter(SurveyParameterModel model);

    }
}
