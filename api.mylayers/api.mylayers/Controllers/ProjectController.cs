using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<SeismicProjectModel> Get(int id)
        {
            return await _projectService.GetSeismicProject(id);
        }
    }
}
