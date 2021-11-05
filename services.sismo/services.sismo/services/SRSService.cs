using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public class SRSService : ISRSService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISRSRepository _ISRSRepository;
        private readonly IConfiguration _configuration;

        public SRSService(ISRSRepository ISRSRepository, ISurveyRepository surveyRepository, IConfiguration configuration)
        {
            _surveyRepository = surveyRepository;
            _ISRSRepository = ISRSRepository;
            _configuration = configuration;
        }
        /// <summary>
        /// Recebe uma coordenada em Lat e Long WGS84 e devolve o SRS SIRGAS UTM com a zona mais apropriada.
        /// </summary>
        /// <param name="longitude">Valor para o Campo Longitude</param>        
        /// <returns></returns>
        public async Task<SRSModel> GetUTMSirgasSRS(double longitude)
        {

            try
            {
                var SRSList = await _ISRSRepository.ListSpatialReferences();
                var res = SRSList.Where(x => x.CentralMeridian != null && longitude - 3 <= x.CentralMeridian && x.CentralMeridian <= longitude + 3 && x.SRSName.Contains("SIRGAS 2000")).FirstOrDefault();
                return res;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<SRSModel>> GetUtmSrsIdByLongitude(double longitude)
        {

            try
            {

                var SRSList = await _ISRSRepository.ListSpatialReferences();
                var res = SRSList.Where(x => x.CentralMeridian != null && longitude - 3 <= x.CentralMeridian && x.CentralMeridian <= longitude + 3);
                return res.ToList();
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<SRSModel>> ListDatums(CoordinateSystem coordinateSystemId, int surveyId)
        {

            try
            {
                if (coordinateSystemId == CoordinateSystem.Projected && surveyId != 0)
                {
                    var polygon = await _surveyRepository.GetSurveyPolygonGeometry(surveyId);
                     return await GetUtmSrsIdByLongitude(polygon.Centroid.Coordinate.X);
                }
                else
                {
                   return await _ISRSRepository.ListSRS(coordinateSystemId);
                }

            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<List<SRSModel>> ListSpatialReferences()
        {

            try
            {
                return await _ISRSRepository.ListSpatialReferences();
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
