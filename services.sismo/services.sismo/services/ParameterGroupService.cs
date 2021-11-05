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
    public class ParameterGroupService : IParameterGroupService
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly ISurveyParameterService _surveyParameterService;
        private readonly IParameterGroupRepository _parameterGroupRepository;
        private readonly IConfiguration _configuration;

        public ParameterGroupService(
            IParameterRepository parameterRepository,
            ISurveyParameterService surveyParameterService,
            IParameterGroupRepository parameterGroupRepository, 
            IConfiguration configuration)
        {
            _parameterRepository = parameterRepository;
            _surveyParameterService = surveyParameterService;
            _parameterGroupRepository = parameterGroupRepository;
            _configuration = configuration;
        }
        public async Task<ParameterGroupModel> GetParameterGroup(int parameterGroupId)
        {
            
            try
            {
                return await _parameterGroupRepository.GetParameterGroup(parameterGroupId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<string> GetSurveyParameterValue(Int32 surveyId, String key)
        {
            
            try
            {


                var surveyParameters = await _surveyParameterService.ListSurveyParameters(surveyId);
                var paramater = surveyParameters.FirstOrDefault(t => t.Key == key);

                return paramater != null ? paramater.Value : "";
            }
              catch (Exception ex) { throw ex; }

        }

        public async Task<List<ParameterGroupModel>> ListParameterGroups()
        {
            
            try
            {
               return await _parameterGroupRepository.ListParameterGroups();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<ParameterModel>> ListParameters()
        {   
            try
            {
                return await _parameterRepository.ListParameters();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<ParameterGroupModel>> ListParameterGroup()
        {
            var list = await _parameterGroupRepository.ListParameterGroups();
            return list;
        }
    }
}
