using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IProjectRepository
    {
        Task<SeismicProjectModel> GetSeismicProject(Int32 projectId);
        Task<List<SeismicProjectModel>> ListProjects(bool? isActive = true);
        Task AddProject(SeismicProjectModel project);
        Task UpdateProject(SeismicProjectModel modifiedProject);
    }
}
