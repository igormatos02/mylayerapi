using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System;
using System.Collections.Generic;
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

        public async Task<List<SurveyModel>> ListSurveys(bool isActive)
        {
            try
            {
                return await _surveyRepository.ListSurveys();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SurveyModel>> ListSurveys(int projectId)
        {
            try
            {
                return await _surveyRepository.ListProjectSurveys( projectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateSurveyStatus(int surveyId, bool isActive)
        {
            try
            {
                var existing = await _surveyRepository.GetSurvey(surveyId);
                if (existing != null)
                {
                    existing.IsActive = isActive;
                    await _surveyRepository.UpdateSurvey(existing);
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
