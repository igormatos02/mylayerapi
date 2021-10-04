using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IOperationalFrontRepository
    {
        Task<bool> HasAnyProduction(int operationalFrontId);
        Task<OperationalFrontModel> GetOperationalFront(Int32 operationalFrontId);
        Task<List<OperationalFrontProductionModel>> GetOperationalFrontProduction(int surveyId, string date);
        Task<string> GetOperationalFrontName(int operationalFrontId);
        Task<OperationalFrontType> GetOperationalFrontType(int operationalFrontId);
        Task<OperationalFrontModel> GetPreviousOperationalFront(int operationalFrontId);
        Task<int?> GetPreviousOperationalFrontId(int operationalFrontId);
        Task<List<OperationalFrontModel>> ListNextOperationalFronts(int operationalFrontId);
        Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId);
        Task<OperationalFrontModel> AddOperationalFront(OperationalFrontModel model);
        Task UpdateOperationalFront(OperationalFrontModel model);
        Task DeleteOperationalFront(int operationalFrontId);
    }
}
