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
    public class PreplotVersionController : ControllerBase
    {

        private readonly IPreplotVersionService _PreplotVersionService;
        public PreplotVersionController(IPreplotVersionService PreplotVersionService)
        {
            _PreplotVersionService = PreplotVersionService;
        }


    }
}
