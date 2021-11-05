using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IProjectBaseRepository
    {
        Task<ProjectBaseModel> GetBase(int baseId);
        Task<List<ProjectBaseModel>> ListBases(int projectId);
        Task<int> AddBase(ProjectBaseModel Base);
        Task UpdateBase(ProjectBaseModel modifiedBase);
        Task DeleteBase(ProjectBaseModel modifiedBase);

        Task AddFile(String fileName, Int32 baseId);
    }
}
