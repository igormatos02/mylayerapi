using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IStretchService
    {

       Task<StretchModel> UnifyInterSectionedStretches(StretchModel newStretch, PreplotPointType preplotPointType);

        Task CalculateStretchKm(StretchModel stretch, PreplotPointType preplotPointType);

       Task<List<FrontGroupProductionModel>> ListFrontProduction(int surveyId, int preplotPointType, int operationalFrontId, string date);

       Task<List<FrontGroupProductionModel>> ListStretchGroups( int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date);

        Task<FrontGroupProductionModel> InstantiateFrontGroupProduction( String date, int frontGroupId, int frontGroupLeaderId, int groupIndex);

       Task<bool> DeleteStretch(StretchModel stretch);

       Task<FrontOverFrontChartSerie> GetFrontOverFrontChartSeries(int surveyId, string reportDate, bool isDateTimeNow);

       Task<ProductionChartSerie> GetProductionChartSeries(int surveyId, string reportDate);

       Task<List<OpFrontTotalProductionGraphModel>> GetSurveyProductionData(int surveyId);

       Task<ProductionPerLinePerOperationalFrontSerie> GetProductionPerLinePerOperationalFront(int surveyId, int linePointsType);

       Task<ProductionOfPeriodSerie> GetProductionOfPeriodChartData(int surveyId, int operationalFrontId, string initialDate, string finalDate);
    }
}
