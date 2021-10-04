using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPosplotCoordinateRepository
    {
        Task InsertPosplotCoordinate(PosplotCoordinateModel posplotCoordinate);
        Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, IList<int> preplotPointsIds, string toWkt, int toSrid);
        Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, PreplotPointType pointsType, string toWkt, int toSrid);
        Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, PreplotPointType pointsType, NetTopologySuite.Geometries.Geometry containerBuffer, string toWkt, int toSrid);
        Task<List<PosplotCoordinateModel>> ListPosplotCompare(Int32 surveyId, Int32 type, String line, Int32 operationalFrontId);
        Task DropPosplot(Int32 surveyId, Int32 preplotPointType);
    }
}
