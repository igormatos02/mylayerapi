using clientlib.mylayers;
using common.sismo.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace apiclient.mylayersapi
{
    public class SurveyClient : GenericClient
    {
        public SurveyClient(string baseurl, IHttpClientFactory clientFactory):
            base(clientFactory, baseurl)
        {
        
        }

        public async Task<ClientResponse<SurveyModel>> Get()
        {
            return await Get<SurveyModel>("Survey/Get");
        }

        public async Task<ClientResponse<List<SurveyModel>>> List()
        {
            return await Get<List<SurveyModel>>("Project/List");
        }

        public async Task<ClientResponse<List<SurveyModel>>> ListFromProject(int projectId)
        {
            return await Get<List<SurveyModel>>("Project/ListFromProject");
        }

        public async Task<ClientResponse<SurveyModel>> UpdateSurveyStatus(SurveyModel model)
        {
            var payload = JsonConvert.SerializeObject(model);
            return await Put<SurveyModel>("Project/UpdateSurveyStatus", payload);
        }
    }
}
