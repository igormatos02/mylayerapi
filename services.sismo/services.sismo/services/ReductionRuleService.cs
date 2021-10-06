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
    public class ReductionRuleService : IReductionRuleService
    {
        private readonly IReductionRuleRepository _reductionRuleRepository;
        private readonly IConfiguration _configuration;

        public ReductionRuleService(IReductionRuleRepository reductionRuleRepository, IConfiguration configuration)
        {
            this._reductionRuleRepository = reductionRuleRepository;
            this._configuration = configuration;
        }

        public async Task<ReductionRuleModel> GetReductionRule(int reductionRuleId)
        {
            try
            {
                return await _reductionRuleRepository.GetReductionRule(reductionRuleId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<ReductionRuleModel>> ListReductionRules(int surveyId)
        {
            try
            {
                return await _reductionRuleRepository.ListReductionRules(surveyId);
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<ReductionRuleModel>> ListAllReductionRules()
        {
            try
            {
                return await _reductionRuleRepository.ListAllReductionRules();
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<ReductionRuleModel> SaveDisplacementRule(Stream fileStream, ReductionRuleModel model, String fileExtension)
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
                return await _reductionRuleRepository.SaveReductionRules(model);
                if (fileStream != null)
                {
                    DirectoryHelper.SaveFileFromStream(location, file);
                }

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
