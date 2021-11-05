using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class FrontGroupService : IFrontGroupService
    {
        private readonly IFrontGroupRepository _frontGroupRepository;
        private readonly IConfiguration _configuration;

        public FrontGroupService(IFrontGroupRepository frontGroupRepository, IConfiguration configuration)
        {
            _frontGroupRepository = frontGroupRepository;
            _configuration = configuration;
        }
        public async Task<List<FrontGroupModel>> ListFrontGroups(int operationalFrontId)
        {   
            try
            {
                return await _frontGroupRepository.ListFrontGroups(operationalFrontId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<FrontGroupModel>> ListAllFrontGroups()
        {   
            try
            {
                return await _frontGroupRepository.ListAllFrontGroups();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FrontGroupModel> GetFrontGroup(int frontGroupId)
        {
            
            try
            {
               return await _frontGroupRepository.GetFrontGroup(frontGroupId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FrontGroupModel> SaveFrontGroup(FrontGroupModel frontGroup)
        {
        
            try
            {
                return await _frontGroupRepository.SaveFrontGroup(frontGroup);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
