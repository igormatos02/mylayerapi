using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPreplotPointRepository
    {
        Task<PreplotPointModel> GetPreplotPoint(int surveyId, PreplotPointType pointType, int preplotVersionId,
         int preplotPointId);


        Task<Boolean> ArePointsConnected(int surveyId, string lineNumber,
            decimal stationNumber1, decimal stationNumber2, PreplotPointType consideringPointsType);

        Task<PreplotPointModel> GetPreplotPoint(int surveyId, string lineNumber, decimal pointNumber,
            PreplotPointType pointType, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> GetPreplotPointsByLabel(int surveyId, string lineNumber, decimal pointNumber);

        Task<int?> GetPreplotPointId(int surveyId, string lineNumber, decimal pointNumber, int preplotPointType);

        Task<int> GetMaxPreplotPointId(int surveyId);
        Task<PreplotPointModel> GetPreplotPointWithoutCoordinates(int surveyId, string lineNumber, decimal pointNumber,
            PreplotPointType pointType);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId,
            string line, PaginationModel pagination, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, int preplotVersionId, PreplotPointType pointType, decimal initialShotPoint, decimal finalShotPoint,
            int initialReceiverLine, int finalReceiverLine, PaginationModel pagination, string toWkt, int toSrid, long countTotal);

        Task<List<int>> ListPreplotPointsIds(int surveyId, PreplotPointType pointType, int preplotVersionId, string line,
            PaginationModel pagination);
        Task<long> CountPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId, string line);

        Task<List<String>> ListLines(int surveyId, PreplotPointType pointType);
        Task<List<String>> ListLines(int surveyId, PreplotPointType pointType, string key);
        Task<List<String>> ListLinesByVersion(int surveyId, PreplotPointType pointType, int preplotVersionId);

        Task<List<PreplotPointType>> ListExistingPreplotPointTypes(int surveyId);

        Task<List<Decimal>> ListStationNumbers(int surveyId, PreplotPointType pointType, string line);

        Task<List<Decimal>> ListStationNumbers(int surveyId, PreplotPointType pointType, string line, decimal initialStation,
            decimal finalStation);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, int landId, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string line,decimal initialStation, 
            decimal finalStation, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType, string line,
            decimal initialStation, decimal finalStation);
        Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType,
            List<LineStretchModel> lineStretches);
        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, decimal initialStation,
            decimal finalStation, string toWkt, int toSrid);

        Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType,
            decimal initialStation, decimal finalStation);

        Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string line, string toWkt, int toSrid);


        Task<List<PointDetailModel>> GetPointDetail(int surveyId, int preplotVersionId, int preplotPointId);
        Task<List<LineStretchModel>> ListStretchesFromSwath(int surveyId, PreplotPointType pointType, decimal initialStation,
            decimal finalStation);


        Task<List<PreplotPointModel>> ListSurveyPreplotPoints(int surveyId, PreplotPointType pointType, String line,
            Int32 preplotVersionId);
        Task SavePreplotPoint(PreplotPointModel model);
    }
}
