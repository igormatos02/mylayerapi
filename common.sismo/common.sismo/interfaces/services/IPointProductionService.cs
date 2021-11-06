using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface  IPointProductionService
    {

        Task<List<TotalDailyProductionModel>> ListDailyProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal? point, bool hasUnaccomplished);

        Task<List<StretchModel>> ListDailyStretches(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date);

        Task<bool> SaveSeismicRegistersWithProductions(int surveyId, int operationalFrontId, IList<SeismicRegisterModel> registers, int frontGroupId, int frontGroupLeaderId);
        Task<List<PointProductionModel>> SaveProductionFromBruteRobFile(
        List<PointProductionModel> productions, IEnumerable<SeismicRegisterModel> seisRegisters, BruteRobParametersModel bruteRobParameters);
        Task<RobScreenDetailsModel> GetImportRobScreenDetails(int operationalFrontId);
        Task<ProductionScreenDetailsModel> GetProductionScreenDetails(int surveyId, int preplotPointType, int operationalFrontId, string date);

        Task<FieldReportDataModel> GetFieldProductionReportData( int surveyId, int preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation, int itemsPerPage);

        Task<FieldReportDataModel> GetFieldProductionReportData(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber, int itemsPerPage);

        Task<List<SeismicRegisterModel>> ListSeismicRegisters( int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation);

        Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, int swathNumber);

        Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation);

        Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, int shotPointsOpFrontId, int stationsOpFrontId, string line, decimal initialStation, decimal finalStation);

        Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber);

        Task<List<PointProductionModel>> ListOrBuildStretchPoints( int surveyId, int preplotPointType, int operationalFrontId, string line,int initialStation, int finalStation, string date, int frontGroupId, int frontGroupLeaderId);

        Task<bool> SaveProduction(int surveyId, int operationalFrontId,List<PointProductionModel> pointProductions, int frontGroupId, int frontGroupLeaderId, decimal initialStationBkp, decimal finalStationBkp, bool savingFromRobFile, String userLogin);

        Task<List<PointProductionModel>> SaveMultipleStretchesProduction(int surveyId, int operationalFrontId, List<List<PointProductionModel>> productionsPerStretch, int frontGroupId, int frontGroupLeaderId);

        Task<List<PointProductionModel>> DeleteFrontGroupProduction(int workNumber, IList<StretchModel> stretches);

        Task<List<PointProductionModel>> DeleteStretchProduction(int workNumber, StretchModel stretch);

        Task<List<PointProductionModel>> DeleteProduction(int surveyId, int operationalFrontId,IEnumerable<PointProductionModel> pointProductions, int workNumber);
        Task<IEnumerable<HoleModel>> InstantiateHoles(PointProductionModel production, int holesPerShotPoint, bool instantiateAbsentHoles, OperationalFrontType frontType);

        Task<IEnumerable<HoleModel>> ListHoles(PointProductionModel production, int holesPerShotPoint, OperationalFrontType frontType);
    }
}
