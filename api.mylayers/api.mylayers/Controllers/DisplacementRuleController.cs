using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DisplacementRuleController : ControllerBase
    {

        private readonly IDisplacementRuleService _displacementRuleService;
        public DisplacementRuleController(IDisplacementRuleService displacementRuleService)
        {
            _displacementRuleService = displacementRuleService;
        }


    }
}
