using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISRSRepository
    {
        Task<SRSModel> GetSRS(int srId);
        Task<List<SRSModel>> ListSRS(CoordinateSystem coordinateSystemId);
        Task<List<SRSModel>> ListSpatialReferences();
    }
}
