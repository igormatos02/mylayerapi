using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPointProductionRepository
    {
        Task<PointProductionModel> GetProduction(int surveyId, int preplotPointId, int preplotVersionId, int workNumber, PreplotPointType pointType, int operationalFrontId);


        Task<DateTime> GetFirstProductionDate(int surveyId, int operationalFrontId);

        Task<DateTime> GetLastProductionDate(int surveyId, int operationalFrontId);
        Task<List<PointProductionModel>> ListLandProductions(int surveyId, int operationalFrontId, int landId, string date);

        Task<List<PointProductionModel>> ListStretchProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation,
            decimal finalStation, int workNumber);
        Task<List<PointProductionModel>> ListStretchLastWorks(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation,
            decimal finalStation);

        Task<List<PointProductionModel>> ListStretchLastWorks(int surveyId, int operationalFrontId, PreplotPointType preplotPointType, decimal initialStation, decimal finalStation);

        Task<List<PointProductionModel>> ListStretchReworks(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, int initialStation,
            int finalStation);

        Task<List<DailyProductionModel>> ListDailyProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId);

        Task<List<StretchModel>> ListDailyStretchesProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, DateTime date);
        Task<List<LandStretchModel>> ListDailyLandStretchesProductions(int surveyId, int operationalFrontId, DateTime date);


        Task<List<PointProductionModel>> ListProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date);

        Task<List<PointProductionModel>> ListProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId);
        Task<List<PointProductionModel>> ListProductions(int surveyId, string date);

        Task<List<DateTime>> ListProductionsDates(int surveyId, PreplotPointType preplotPointType, int operationalFrontId);

        Task<List<KeyValuePair<int, int>>> ListProductionsFrontGroupAndLeaders(int surveyId,
            PreplotPointType preplotPointType, int operationalFrontId, string date);

        Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date);

        Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date, int landId);

        Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date, KeyValuePair<int, int> frontGroupAndLeader);
        Task<List<PointProductionModel>> ListProductionsAndReworks(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId);

        Task<List<PointProductionModel>> ListProductionsWithDisplacementOrReduction(int surveyId, OperationalFrontType operationalFrontType);
        Task<int> CountProductions(int surveyId, string line, decimal initialStation, decimal finalStation, PreplotPointType pointType,
            int status, int operatonalFrontId);

        Task<PointProductionModel> GetPreviousFrontLastProductionOrRework(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);

        Task<string> GetPreviousFrontsLastProductionsOrReworksObservations(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int opFrontId);

        Task<int?> GetPreviousFrontLastProductionOrReworkDisplacementRuleId(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);
        Task<int?> GetPreviousFrontLastProductionOrReworkReductionRuleId(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);

        Task<decimal> GetPreviousFrontLastProductionOrReworkHolesDepth(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);

        Task<int?> GetPreviousFrontLastProductionOrReworkFusesInSp(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);
        Task<int?> GetPreviousFrontLastProductionOrReworkChargesInSp(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId);
        Task<List<PointProductionModel>> ListReworks(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId);

        Task<int> CountNextOperationalFrontsProductions(PointProductionModel production);
        Task<bool> HasNextOperationalFrontsProductionsInFutureDates(PointProductionModel production);

        Task<bool> HasNextOperationalFrontsProductionsInFutureDatesWithAccomplishedStatus(PointProductionModel production);

        Task<bool> HasNextOperationalFrontsProductionsInPastDates(PointProductionModel production);
        Task<int> CountPreviousOperationalFrontsProductions(PointProductionModel production, int previousOperationalFrontId);
        Task<List<PointProductionModel>> ListPreviousOperationalFrontsProductionsInPastDates(PointProductionModel production, int previousOperationalFrontId);

        Task<bool> HasPreviousOperationalFrontsProductionsInFutureDates(PointProductionModel production, int previousOperationalFrontId);

        Task<bool> HasProductionsWithOnlyStationsStatusInPastDates(PointProductionModel production);

        Task<int> GetMaxWorkNumber(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId);
        Task SaveProduction(PointProductionModel production);
        Task SaveProductions(IEnumerable<PointProductionModel> productions);

        Task<bool> DeleteProduction(PointProductionModel production);
        Task DeleteProductionDate(string date, int operationalFrontId, int surveyId);
    }
}
