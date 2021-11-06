using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ReductionRuleController : ControllerBase
    {

        private readonly IReductionRuleService _ReductionRuleService;
        public ReductionRuleController(IReductionRuleService ReductionRuleService)
        {
            _ReductionRuleService = ReductionRuleService;
        }


    }
}
