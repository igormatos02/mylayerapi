using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IProjectExplosiveMaterialTypeRepository
    {
        Task<ProjectExplosiveMaterialTypeModel> GetProjectExplosiveMaterialType(int idProjectExplosiveMaterialType);

        Task<List<ProjectExplosiveMaterialTypeModel>> ListProjectExplosiveMaterialTypes(int projectId);

        Task SaveProjectExplosiveMaterialTypes(ProjectExplosiveMaterialTypeModel dto);
    }
}
