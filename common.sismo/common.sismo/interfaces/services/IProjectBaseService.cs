using common.sismo.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IProjectBaseService
    {

        Task<List<ProjectBaseModel>> ListBases(int projectId);

        Task SaveBase(ProjectBaseModel dto);

        Task<bool> UpdateBaseStatus(int baseId);

        Task<bool> AddFile(Stream fileStream, String fileName, Int32 baseId);
    }
}
