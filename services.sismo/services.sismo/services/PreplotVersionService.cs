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
    public class PreplotVersionService : IPreplotVersionService
    {

        private readonly IPreplotVersionRepository _preplotVersionRepository;
        private readonly IConfiguration _configuration;

        public PreplotVersionService(IPreplotVersionRepository preplotVersionRepository, IConfiguration configuration)
        {
            _preplotVersionRepository = preplotVersionRepository;
            _configuration = configuration;
        }
        

        public async Task<List<PreplotVersionModel>> ListPreplotVersions(int surveyId)
        {
            
            try
            {
                var index = 1;
                var versions = await _preplotVersionRepository.ListPreplotVersions(surveyId);
                foreach (PreplotVersionModel v in versions)
                    v.PreplotVersionNumber = index++;
               return versions;

            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<int> GetLastPreplotVersion(int surveyId)
        {
            try
            {
                var version =  await _preplotVersionRepository.ListPreplotVersions(surveyId);
                return version.Select(t => t.PreplotVersionId).Max();
            }
            catch (Exception ex) { throw ex; }
           
        }

        public async Task<int> InsertPreplotVersion(PreplotVersionModel dto)
        {
            
            try
            {
                return await _preplotVersionRepository.InsertPreplotVersion(dto);
    }
           catch (Exception ex) { throw ex; }
        }

    

        public async Task DeleteVersion(int surveyId, int versionId)
        {
           
            var model  = await _preplotVersionRepository.GetPreplotVersion(surveyId, versionId);
            await _preplotVersionRepository._Delete(model);
        }
    }
}
