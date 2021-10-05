using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IProjectService
    {
        Task<SeismicProjectModel> GetSeismicProject(Int32 projectId);
        Task<List<SeismicProjectModel>> ListProjects(bool? isActive);

        Task<SeismicProjectModel> SaveProject(SeismicProjectModel model);

        Task ActivateDeactivateProject(int projectId);
    }
}

