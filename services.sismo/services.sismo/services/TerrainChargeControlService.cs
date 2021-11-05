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
    public class TerrainChargeControlService : ITerrainChargeControlService
    {
        private readonly ITerrainChargeControlRepository _terrainChargeControlRepository;
        private readonly IConfiguration _configuration;

        public TerrainChargeControlService(ITerrainChargeControlRepository terrainChargeControlRepository, IConfiguration configuration)
        {
            _terrainChargeControlRepository = terrainChargeControlRepository;
            _configuration = configuration;
        }

        public async Task<List<TerrainChargeControlModel>> ListTerrainChargeControls(int surveyId)
        {
            
            try
            {
               
                return await _terrainChargeControlRepository.ListTerrainChargeControls(surveyId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> SaveTerrainChargeControls(TerrainChargeControlModel model)
        {
            
            try
            {

                await _terrainChargeControlRepository.SaveTerrainChargeControls(model);
                return true;
              
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<bool> DeleteTerrainChargeControl(TerrainChargeControlModel model)
        {
            
            try
            {
                await _terrainChargeControlRepository.DeleteTerrainChargeControl(model);

                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<TerrainChargeControlModel> GetTerrainChargeControl(int TerrainChargeControlId)
        {
            
            try
            {
              return  await _terrainChargeControlRepository.GetTerrainChargeControl(TerrainChargeControlId);

            }
            catch (Exception ex) { throw ex; }
            }
    }
}
