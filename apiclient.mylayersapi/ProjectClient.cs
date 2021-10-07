using clientlib.mylayers;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace apiclient.mylayersapi
{
    public class ProjectClient : GenericClient
    {
        public ProjectClient(string baseurl, IHttpClientFactory clientFactory):
            base(clientFactory, baseurl)
        {
        
        }

        public async Task<ClientResponse<List<SeismicProjectModel>>> ListProjectsAsync()
        {
            return await Get<List<SeismicProjectModel>>("Project/List");
        }
           
    }
}
