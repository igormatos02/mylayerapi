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
    public class HoleController : ControllerBase
    {

        private readonly IHoleService _HoleService;
        public HoleController(IHoleService HoleService)
        {
            _HoleService = HoleService;
        }


    }
}
