using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ISurveyParameterService
    {
        Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId);

        Task SaveSurveyParameters(List<ParameterModel> listDto);
    }
}
