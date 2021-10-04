using common.sismo.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IFrontGroupRepository
    {
        Task<FrontGroupModel> GetFrontGroup(int frontGroupId);
        Task<string> GetFrontGroupName(int frontGroupId);
        Task<List<FrontGroupModel>> ListFrontGroups(int operationalFrontId);
        Task<List<FrontGroupModel>> ListAllFrontGroups();
        Task<FrontGroupModel> SaveFrontGroup(FrontGroupModel model);
    }
}
