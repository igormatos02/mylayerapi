using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<SeismicProjectModel> Get(int id)
        {
            return await _projectService.GetSeismicProject(id);
        }

        [HttpGet]
        [Route("List")]
        public async Task<List<SeismicProjectModel>> List(bool? isActive)
        {
            return await _projectService.ListProjects(isActive);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<SeismicProjectModel> Save(SeismicProjectModel model)
        {
            return await _projectService.SaveProject(model);
        }

        [HttpPut]
        [Route("ActivateDeactivate")]
        public async Task ActivateDeactivate(int projectId)
        {
            await _projectService.ActivateDeactivateProject(projectId);
        }
    }
}
