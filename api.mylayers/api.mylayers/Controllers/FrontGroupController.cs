using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FrontGroupController : ControllerBase
    {

        private readonly IFrontGroupService _frontGroupService;
        public FrontGroupController(IFrontGroupService frontGroupService)
        {
            _frontGroupService = frontGroupService;
        }


    }
}
