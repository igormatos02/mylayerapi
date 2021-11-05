using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ISRSService
    {
       Task<SRSModel> GetUTMSirgasSRS(double longitude);
       Task<List<SRSModel>> GetUtmSrsIdByLongitude(double longitude);
       Task<List<SRSModel>> ListDatums(CoordinateSystem coordinateSystemId, int surveyId);
       Task<List<SRSModel>> ListSpatialReferences();
    }
}
