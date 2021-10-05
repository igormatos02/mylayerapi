using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISeismicRegisterRepository
    {
        Task<int> GetLastFfid(int surveyId);
        Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation);
        Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, List<LineStretchModel> swathStretches);
        Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, string date);
        Task SaveSeismicRegister(SeismicRegisterModel model);
        Task<List<int>> ListExistingFfids(List<int> ffids);
        Task SaveSeismicRegisters(IEnumerable<SeismicRegisterModel> registers);
        Task DeleteSeismicRegisters(IEnumerable<PointProductionModel> productions);
        Task DeleteSeismicRegisters(IEnumerable<SeismicRegisterModel> registers);

    }
}
