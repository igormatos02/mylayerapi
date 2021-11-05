using common.sismo.models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IHoleCoordinateRepository
    {
       Task<HoleCoordinateModel> AddHoleCoordinate(HoleCoordinateModel holeCoord);

        Task UpdateHolesCoordinates(IEnumerable<HoleCoordinateModel> holesCoordinates);

        Task UpdateAllHoleCoordinates(int surveyId);

        Task<bool> DeleteHoleCoordinate(int surveyId, int holeCoordinateId);

        Task<bool> DeleteHolesCoordinates(int surveyId, int fileId);

        Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, string toWkt, int toSrid);

        Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, int fileId, string toWkt, int toSrid);

        Task<Geometry> GetHolesEnvelopeBufferGeometry(int surveyId, int fileId, double bufferSizeInDegrees);

        Task<Geometry> GetUnitedHolesCoordinatesGeometry(int surveyId, int fileId);

        Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, string line, string toWkt, int toSrid);
        Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, IEnumerable<int> preplotPointsIds, string toWkt, int toSrid);

        Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, DateTime date, string line, string toWkt, int toSrid);

        Task<HoleCoordinateModel> GetHoleCoordinate(int surveyId, int preplotPointId, int holeNumber, DateTime acquisitionTime, string toWkt, int toSrid);

    }
}
