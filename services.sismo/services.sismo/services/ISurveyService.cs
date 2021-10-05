using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class SurveyService : ISurveyService
    {
        public readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository; 
        }

        public async Task<SurveyModel> GetSurvey(int surveyId)
        {
            try { 
                return await _surveyRepository.GetSurvey(surveyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
