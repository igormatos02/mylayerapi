using common.sismo.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ITerrainChargeControlRepository
    {
        Task<TerrainChargeControlModel> GetTerrainChargeControl(int terrainChargeControlId);
        Task<List<TerrainChargeControlModel>> ListTerrainChargeControls(int surveyId);
        Task SaveTerrainChargeControls(TerrainChargeControlModel model);
        Task DeleteTerrainChargeControl(TerrainChargeControlModel dto);
    }
}
