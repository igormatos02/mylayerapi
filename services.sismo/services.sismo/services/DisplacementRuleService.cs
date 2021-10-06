using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class DisplacementRuleService : IDisplacementRuleService
    {
        private readonly IDisplacementRuleRepository _displacementRuleRepository;
        private readonly IConfiguration _configuration;

        public DisplacementRuleService(IDisplacementRuleRepository displacementRuleRepository, IConfiguration configuration)
        {
            _displacementRuleRepository = displacementRuleRepository;
            _configuration = configuration;
        }

        public async Task<DisplacementRuleModel> GetDisplacementRule(int displacementRuleId)
        {
            try
            {
                return await _displacementRuleRepository.GetDisplacementRule(displacementRuleId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<DisplacementRuleModel>> ListDisplacementRules(int surveyId, bool onlyActives, int type)
        {
            try
            {
                return await _displacementRuleRepository.ListDisplacementRules(surveyId, onlyActives, type);
                
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<DisplacementRuleModel>> ListAllDisplacementRules()
        {
            try
            {
                return await _displacementRuleRepository.ListDisplcementRules();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<DisplacementRuleModel> SaveDisplacementRule(Stream fileStream, DisplacementRuleModel model, String fileExtension)
        {
            try
            {
                String location = "";
                var file = new byte[0];
                if (fileStream != null)
                {
                    file = new byte[fileStream.Length];
                    await fileStream.ReadAsync(file, 0, (int)fileStream.Length);
                    String path = _configuration["MediaDirectory"] + "images/";
                    String newFileName = Guid.NewGuid().ToString() + "." + fileExtension;
                    location = path + newFileName;
                    model.ImagePath = newFileName;
                }
                return await _displacementRuleRepository.SaveDisplacementRules(model);
                if (fileStream != null)
                {
                    DirectoryHelper.SaveFileFromStream(location, file);
                }
              
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
