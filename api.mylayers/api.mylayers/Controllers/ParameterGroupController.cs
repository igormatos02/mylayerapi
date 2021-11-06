using common.sismo.interfaces;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ParameterGroupController : ControllerBase
    {

        private readonly IParameterGroupService _ParameterGroupService;
        public ParameterGroupController(IParameterGroupService ParameterGroupService)
        {
            _ParameterGroupService = ParameterGroupService;
        }


    }
}
