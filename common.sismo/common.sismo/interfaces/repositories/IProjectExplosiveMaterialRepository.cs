using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IProjectExplosiveMaterialRepository
    {
        Task<ProjectExplosiveMaterialModel> GetProjectExplosiveMaterial(int entryId);

        Task<List<ProjectExplosiveMaterialModel>> ListProjectExplosiveMaterials(int ProjectId, Int32 entryType);
        Task<List<ProjectExplosiveMaterialModel>> ListDeletedProjectExplosiveMaterials(int ProjectId);

        Task SaveProjectExplosiveMaterials(ProjectExplosiveMaterialModel dto);

        Task DeleteProjectExplosiveMaterial(ProjectExplosiveMaterialModel dto);
    }
}
