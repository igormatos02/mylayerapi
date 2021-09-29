using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public Task<SeismicProjectModel> GetSeismicProject(int projectId)
        {
            return this.projectRepository.GetSeismicProject(projectId);
        }
    }
}
