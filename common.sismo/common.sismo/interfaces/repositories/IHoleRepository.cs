using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IHoleRepository
    {
         Task<HoleModel> GetHole(int surveyId, int preplotPointId, int preplotVersionId,
            PreplotPointType preplotPointType, int workNumber, int operationalFrontId, int holeNumber);
      
         Task<List<HoleModel>> ListHoles(int surveyId, int preplotPointId, int preplotVersionId,
            PreplotPointType preplotPointType, int operationalFrontId);
        Task DeleteHoles(IEnumerable<HoleModel> holes);
        Task DeleteHoles(PointProductionModel production);

         Task AddHoles(IEnumerable<HoleModel> holes);

         Task UpdateHoles(IEnumerable<HoleModel> holes);
    }
}
