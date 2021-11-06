using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.mylayers.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SwathController : ControllerBase
    {

        private readonly ISwathService _SwathService;
        public SwathController(ISwathService SwathService)
        {
            _SwathService = SwathService;
        }


    }
}
