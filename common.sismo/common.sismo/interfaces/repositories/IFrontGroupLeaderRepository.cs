using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IFrontGroupLeaderRepository
    {
        Task<FrontGroupLeaderModel> GetFrontGroupLeader(int frontGroupLeaderId);
        Task<string> GetFrontGroupLeaderName(int frontGroupLeaderId);
        Task<List<FrontGroupLeaderModel>> ListFrontGroupLeaders(int operationalFrontId);
        Task<List<FrontGroupLeaderModel>> ListAllFrontGroupLeaders();
        Task<FrontGroupLeaderModel> SaveFrontGroupLeader(FrontGroupLeaderModel model);

    }
}
