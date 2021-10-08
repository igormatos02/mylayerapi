using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace api.mylayers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyController : ControllerBase
    {

        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<SurveyModel> Get(int id)
        {
            return await _surveyService.GetSurvey(id);
        }

        [HttpGet]
        [Route("ListActives")]
        public async Task<List<SurveyModel>> ListActives()
        {
            return await _surveyService.ListSurveys(true);
        }

        [HttpGet]
        [Route("List")]
        public async Task<List<SurveyModel>> List()
        {
            return await _surveyService.ListSurveys(false);
        }

        [HttpGet]
        [Route("ListFromProject/{projectId}")]
        public async Task<List<SurveyModel>> ListFromProject(int projectId)
        {
            return await _surveyService.ListSurveys(projectId);
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task UpdateSurveyStatus(SurveyModel model)
        {
              await _surveyService.UpdateSurveyStatus(model.SurveyId, model.IsActive);
        }
    }
}
