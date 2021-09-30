using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace api.mylayers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationalFrontController : ControllerBase
    {

        private readonly IOperationalFrontService _operationalFrontService;
        public OperationalFrontController(IOperationalFrontService operationalFrontService)
        {
            _operationalFrontService = operationalFrontService;
        }

        [HttpGet]
        public async Task<OperationalFrontModel> Get(int id)
        {
            return await _operationalFrontService.GetOperationalFront(id);
        }
    }
}
