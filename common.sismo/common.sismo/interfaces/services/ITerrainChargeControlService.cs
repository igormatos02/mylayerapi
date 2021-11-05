using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ITerrainChargeControlService
    {
       Task<List<TerrainChargeControlModel>> ListTerrainChargeControls(int surveyId);

       Task<bool> SaveTerrainChargeControls(TerrainChargeControlModel model);

       Task<bool> DeleteTerrainChargeControl(TerrainChargeControlModel model);

       Task<TerrainChargeControlModel> GetTerrainChargeControl(int TerrainChargeControlId);
    }
}
