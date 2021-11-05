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
    public class FrontGroupLeaderService : IFrontGroupLeaderService
    {
        private readonly IFrontGroupLeaderRepository _frontGroupLeaderRepository;
        private readonly IConfiguration _configuration;

        public FrontGroupLeaderService(IFrontGroupLeaderRepository frontGroupLeaderRepository, IConfiguration configuration)
        {
            _frontGroupLeaderRepository = frontGroupLeaderRepository;
            _configuration = configuration;
        }

        public async Task<List<FrontGroupLeaderModel>> ListFrontGroupLeaders(int operationalFrontId)
        {
            try
            {
                return await _frontGroupLeaderRepository.ListFrontGroupLeaders(operationalFrontId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<FrontGroupLeaderModel>> ListAllFrontGroupLeaders()
        {
            
            try
            {
                return await _frontGroupLeaderRepository.ListAllFrontGroupLeaders();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FrontGroupLeaderModel> GetFrontGroupLeader(int frontGroupLeaderId)
        {
            
            try
            {
                return await _frontGroupLeaderRepository.GetFrontGroupLeader(frontGroupLeaderId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FrontGroupLeaderModel> SaveFrontGroupLeader(FrontGroupLeaderModel model)
        {
            
            try
            {
                return await _frontGroupLeaderRepository.SaveFrontGroupLeader(model);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
