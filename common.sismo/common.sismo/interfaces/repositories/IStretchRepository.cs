using common.sismo.enums;
using common.sismo.models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IStretchRepository
    {
        Task<StretchModel> GetStretch(int surveyId, int operationalFrontId, int frontGroupId, int frontGroupLeaderId, string line, string date, decimal initialStation, decimal finalStation);
        Task<StretchModel> GetStretchByInitialStation(int surveyId, int operationalFrontId, string line, decimal initialStation);
        Task<StretchModel> GetStretchByFinalStation(int surveyId, int operationalFrontId, string line, decimal finalStation);
        Task<bool> DeleteStretch(StretchModel stretch);
        Task<String> GetFirstProductionDate(int surveyId);
        Task<String> GetLastProductionDate(int surveyId);
        Task<StretchModel> GetPreviousStationt(StretchModel model);
        Task<decimal> CalculateKm(int surveyId, PreplotPointType preplotPointType, string line, decimal initialStation, decimal finalStation, Geometry geomLine, int countPoints);
        Task<StretchModel> GetNextStation(StretchModel model);
        Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId, string date);
        Task<List<StretchModel>> ListStretches(int idSurvey, string lastDate);
        Task<List<StretchModel>> ListStretches(int idSurvey);
        Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId);
        Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId, string initialDate, string finalDate);
        Task<List<StretchModel>> ListStretchesWithIntersection(int surveyId, int operationalFrontId, int frontGroupId, int frontGroupLeaderId, string line, string date, decimal initialStation, decimal finalStation);
        Task<bool> HasAnyIntersectionedStretches(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation);
        Task SaveStretch(StretchModel model);
        Task<List<TotalDailyProductionModel>> ListTotalDailyProductions(int surveyId, int operationalFrontId, string line, decimal? point, bool hasUnaccomplished);
        Task<List<OpFrontTotalProductionGraphModel>> GetSurveyProductionData(int surveyId, String date);
        Task<List<RdoFrontOverFrontChartPointDataModel>> GetFrontOverFrontChartSerie(int surveyId, DateTime reportDate, int operationalFrontId);
        Task<List<ProductionPerLineChartSerieModel>> GetProductionPerLineSeries(int surveyId, string reportDate, IList<string> consideringLines, IList<OperationalFrontModel> operationalFronts);
    }
}
