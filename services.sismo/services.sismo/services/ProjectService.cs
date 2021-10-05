using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<SeismicProjectModel> GetSeismicProject(int projectId)
        {
            try {
                return await this.projectRepository.GetSeismicProject(projectId);
            }
            catch(Exception ex) {
                throw ex;
            }
        }
       
        public async Task<List<SeismicProjectModel>> ListProjects(bool? isActive)
        {
            try
            {
                return await this.projectRepository.ListProjects(isActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SeismicProjectModel> SaveProject(SeismicProjectModel model)
        {
            try
            {
                var projects = await this.projectRepository.ListProjects();
                if(projects.Where(m => m.Name.Trim().Equals(model.Name)).Count() > 0)
                {
                    throw new Exception("Project Name already in use by anothe Project");
                }
                if (projects.Any(x => x.ProjectId == model.ProjectId))
                {
                    await this.projectRepository.UpdateProject(model);
                    return model;
                }
                else return await this.projectRepository.AddProject(model);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ActivateDeactivateProject(int projectId)
        {
            try
            {
                var project = await this.projectRepository.GetSeismicProject(projectId);
                project.IsActive = !project.IsActive;
                await this.projectRepository.UpdateProject(project);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
