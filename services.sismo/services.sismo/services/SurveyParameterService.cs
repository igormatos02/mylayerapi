using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class SurveyParameterService : ISurveyParameterService
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ISurveyParameterRepository _surveyParameterRepository;
        private readonly IConfiguration _configuration;

        public SurveyParameterService(
            IParameterRepository parameterRepository,
            ISurveyParameterRepository surveyParameterRepository,
            IConfiguration configuration)
        {
            _parameterRepository = parameterRepository;
            _surveyParameterRepository = surveyParameterRepository;
            _configuration = configuration;
        }
        public async Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId)
        {
            try
            {
                return await _surveyParameterRepository.ListSurveyParameters(surveyId);
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task SaveSurveyParameters(List<ParameterModel> listDto)
        {
           
            if (listDto == null || listDto.Count == 0)
            {  
                throw new Exception("O objeto da lista está nulo ou vazio");
            }

          
            try
            {
                var temp = listDto.Where(t => t.Value != null && t.Value != "" && t.SurveyId != 0);
                foreach (var item in temp)
                {
                    ParameterModel existingParameter = await _parameterRepository.GetParameter(item.ParameterId);
                    SurveyParameterModel existingSurveyParameter = await _surveyParameterRepository.GetSurveyParameter(item.SurveyId, item.ParameterId);
                    if (existingParameter == null) continue;
                    if (existingSurveyParameter == null)
                    {
                        var model = new SurveyParameterModel()
                        {
                            IsActive = true,
                            ParameterId = existingParameter.ParameterId,
                            SurveyId = item.SurveyId,
                            Value = item.Value
                        };
                        await _surveyParameterRepository.AddSurveyParameter(model);
                    }
                    else
                    {
                        existingSurveyParameter.Value = item.Value;
                        await _surveyParameterRepository.UpdateSurveyParameter(existingSurveyParameter);
                    }
                }
            }
           catch (Exception ex) { throw ex; }
        }
    }
}
