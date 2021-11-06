using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FrontGroupLeaderController : ControllerBase
    {

        private readonly IFrontGroupLeaderService _frontGroupLeaderService;
        public FrontGroupLeaderController(IFrontGroupLeaderService frontGroupLeaderService)
        {
            _frontGroupLeaderService = frontGroupLeaderService;
        }


    }
}
