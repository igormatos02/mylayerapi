using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IProjectExplosiveMaterialTypeService
    {
       Task<List<ProjectExplosiveMaterialTypeModel>> ListProjectExplosiveMaterialTypes(int projectId);

       Task SaveProjectExplosiveMaterialTypes(ProjectExplosiveMaterialTypeModel dto);

       Task<ProjectExplosiveMaterialTypeModel> GetProjectExplosiveMaterialType(int projectExplosiveMaterialTypeId);
    }
}
