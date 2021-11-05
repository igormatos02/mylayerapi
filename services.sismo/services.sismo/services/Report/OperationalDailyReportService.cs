//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace services.sismo.services.Report
//{
//    public class OperationalDailyReportService : IOperationalDailyReportService
//    {
//        public async Task< GetRdoData(int surveyId, string dateString)
//        {
            
//            try
//            {
//                var date = DateParser.StringToDate(dateString).Date;
//                var redRuleDal = new ReductionRuleDAL();
//                var prodDal = new PointProductionDAL();
//                var surveyDal = new SurveyDAL();
//                var opFrontsData = new List<OpFrontRdoDataDTO>();
//                var frontOverFrontLines = new List<RdoFrontOverFrontTableDataDTO>();
//                var survey = surveyDal.GetSurvey(surveyId);
//                var holesQuantity = surveyDal.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
//                var opFronts = surveyDal.ListSurveyOperationalFronts(surveyId).OrderBy(m => m.OperationalFrontType).ThenBy(m => m.OperationalFrontId).ToList();
//                var surveyLines = LineBLL.ListLines(surveyId, dateString).ToList();
//                var surveyShotPointLinesNames = surveyLines.Where(l => l.LinePointsType == PreplotPointType.ShotPoint).Select(l => l.LineName).ToList();
//                var producedStretchesUntilDate = new StretchDAL().ListStretches(surveyId, dateString).ToList();
//                var productionsOfDay = prodDal.ListProductions(surveyId, dateString).ToList();
//                var opFrontDal = new OperationalFrontDAL();
//                foreach (var prod in productionsOfDay)
//                    prod.Holes = PointProductionBLL.InstantiateHoles(prod, holesQuantity, true, redRuleDal,
//                        opFrontDal.GetOperationalFrontType(prod.OperationalFrontId));
//                foreach (var opFront in opFronts)
//                {
//                    var frontId = opFront.OperationalFrontId;
//                    var frontPointsType = PreplotPointBLL.GetPreplotPointTypeByOpFrontType(opFront.OperationalFrontType);
//                    var opFrontLines = surveyLines.Where(l => l.LinePointsType == frontPointsType).ToList();
//                    opFrontsData.Add(new OpFrontRdoDataDTO
//                    {
//                        SummaryLine = GetSummaryLine(surveyId, opFront.OperationalFrontId, opFront.Name, producedStretchesUntilDate, opFrontLines, date, prodDal),
//                        FrontName = opFront.Name.ToUpperInvariant(),
//                        FrontType = opFront.OperationalFrontType//,
//                        //GroupedProductions = GetGroupedProductions(productionsOfDay.Where(p => p.OperationalFrontId == frontId))
//                    });
//                    frontOverFrontLines.Add(GetFrontOverFrontLine(surveyShotPointLinesNames, opFront.OperationalFrontId, opFront.Name, opFront.OperationalFrontType,
//                        producedStretchesUntilDate, opFronts));
//                }
//                frontOverFrontLines.RemoveAt(frontOverFrontLines.Count - 1);
//                var frontOverFrontColumnsTitles = opFronts.Select(m => m.Name).ToList();
//                frontOverFrontColumnsTitles.RemoveAt(0);

//                result.Data = new RdoDataDTO
//                {
//                    OperationalFrontsData = opFrontsData,
//                    FrontOverFrontTableColumnsTitles = frontOverFrontColumnsTitles,
//                    FrontOverFrontTableLines = frontOverFrontLines,
//                    HolesQuantity = holesQuantity,
//                    TotalERPoints = surveyLines.Where(t => t.LinePointsType == PreplotPointType.ReceiverStation).Select(e => e.TotalPoints).Sum(),
//                    TotalPTPoints = surveyLines.Where(t => t.LinePointsType == PreplotPointType.ShotPoint).Select(e => e.TotalPoints).Sum(),
//                    TotalLR = surveyLines.Where(t => t.LinePointsType == PreplotPointType.ReceiverStation).Count(),
//                    TotalLT = surveyLines.Where(t => t.LinePointsType == PreplotPointType.ShotPoint).Count(),
//                    SurveyDatum = new SRSDAL().GetSRS(survey.DatumId).SRSName.Replace("zone", "zona"),
//                };
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        internal static RdoOpFrontSummaryDataDTO GetSummaryLine(int surveyId, int opFrontId, string opFrontName, IList<StretchDTO> producedStretchesUntilDate, IList<LineDTO> opFrontLines, DateTime reportDate, PointProductionDAL productionDal)
//        {
//            var producedStretchesofSurvey = producedStretchesUntilDate.Where(s => s.OperationalFrontId == opFrontId).ToList();
//            var producedStretchesOfDay = producedStretchesofSurvey.Where(s => s.Date == reportDate).ToList();
//            var producedStretchesOfMonth = producedStretchesofSurvey.Where(s => s.Date.Month == reportDate.Month && s.Date.Year == reportDate.Year).ToList();
//            var lineData = new RdoOpFrontSummaryDataDTO
//            {
//                //DIA
//                KmOfDay = producedStretchesOfDay.Sum(m => m.TotalKm) ?? 0,
//                TotalRealizedOfDay = (producedStretchesOfDay.Sum(m => m.RealizedCount) ?? 0) + (producedStretchesOfDay.Sum(m => m.RealizedOnlyStationsCount) ?? 0),
//                TotalNotRealizedOfDay = producedStretchesOfDay.Sum(m => m.NotRealizedCount) ?? 0,
//                //MÊS
//                KmOfMonth = producedStretchesOfMonth.Sum(m => m.TotalKm) ?? 0,
//                TotalRealizedOfMonth = (producedStretchesOfMonth.Sum(m => m.RealizedCount) ?? 0) + (producedStretchesOfMonth.Sum(m => m.RealizedOnlyStationsCount) ?? 0),
//                TotalNotRealizedOfMonth = producedStretchesOfMonth.Sum(m => m.NotRealizedCount) ?? 0,
//                //PROGRAMA
//                KmOfSurvey = producedStretchesofSurvey.Sum(m => m.TotalKm) ?? 0,
//                TotalRealizedOfSurvey = (producedStretchesofSurvey.Sum(m => m.RealizedCount) ?? 0) + (producedStretchesofSurvey.Sum(m => m.RealizedOnlyStationsCount) ?? 0),
//                TotalNotRealizedOfSurvey = producedStretchesofSurvey.Sum(m => m.NotRealizedCount) ?? 0,
//                //INÍCIO ATIVIDADES
//                FirstProductionDate = productionDal.GetFirstProductionDate(surveyId, opFrontId),
//                //PROGRAMA RECEBIDO
//                TotalKmOfCurrentPreplotVersion = opFrontLines.Sum(m => m.TotalKm),
//                TotalPointsOfCurrentPreplotVersion = opFrontLines.Sum(m => m.TotalPoints)
//            };
//            //KM2 - ÁREA
//            lineData.TotalAreaOfCurrentPreplotVersion = 81;
//            lineData.AreaOfDay = (lineData.TotalRealizedOfDay + lineData.TotalNotRealizedOfDay) * (lineData.TotalAreaOfCurrentPreplotVersion / lineData.TotalPointsOfCurrentPreplotVersion);
//            lineData.AreaOfMonth = (lineData.TotalRealizedOfMonth + lineData.TotalNotRealizedOfMonth) * (lineData.TotalAreaOfCurrentPreplotVersion / lineData.TotalPointsOfCurrentPreplotVersion);
//            lineData.AreaOfSurvey = (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey) * (lineData.TotalAreaOfCurrentPreplotVersion / lineData.TotalPointsOfCurrentPreplotVersion);

//            if (lineData.FirstProductionDate != DateTime.MinValue)
//                lineData.FirstProductionDateString = DateParser.DateToString(lineData.FirstProductionDate);
//            //KMs REALIZED AND NOT REALIZED
//            if (lineData.TotalRealizedOfDay + lineData.TotalNotRealizedOfDay > 0)
//            {
//                lineData.KmRealizedOfDay = lineData.KmOfDay * ((decimal)lineData.TotalRealizedOfDay / (lineData.TotalRealizedOfDay + lineData.TotalNotRealizedOfDay));
//                lineData.KmNotRealizedOfDay = lineData.KmOfDay * ((decimal)lineData.TotalNotRealizedOfDay / (lineData.TotalRealizedOfDay + lineData.TotalNotRealizedOfDay));
//            }
//            if (lineData.TotalRealizedOfMonth + lineData.TotalNotRealizedOfMonth > 0)
//            {
//                lineData.KmRealizedOfMonth = lineData.KmOfMonth * ((decimal)lineData.TotalRealizedOfMonth / (lineData.TotalRealizedOfMonth + lineData.TotalNotRealizedOfMonth));
//                lineData.KmNotRealizedOfMonth = lineData.KmOfMonth * ((decimal)lineData.TotalNotRealizedOfMonth / (lineData.TotalRealizedOfMonth + lineData.TotalNotRealizedOfMonth));
//            }
//            if (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey > 0)
//            {
//                lineData.KmRealizedOfSurvey = lineData.KmOfSurvey * ((decimal)lineData.TotalRealizedOfSurvey / (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey));
//                lineData.KmNotRealizedOfSurvey = lineData.KmOfSurvey * ((decimal)lineData.TotalNotRealizedOfSurvey / (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey));
//            }
//            //PERCENTUAIS
//            if (lineData.TotalPointsOfCurrentPreplotVersion > 0)
//            {
//                lineData.PercentageOfDay = (lineData.TotalRealizedOfDay + lineData.TotalNotRealizedOfDay) / (decimal)lineData.TotalPointsOfCurrentPreplotVersion * 100;
//                lineData.PercentageOfMonth = (lineData.TotalRealizedOfMonth + lineData.TotalNotRealizedOfMonth) / (decimal)lineData.TotalPointsOfCurrentPreplotVersion * 100;
//                lineData.PercentageOfSurvey = (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey) / (decimal)lineData.TotalPointsOfCurrentPreplotVersion * 100;
//            }
//            //MÉDIAS DO PROGRAMA
//            var surveyDays = reportDate.Subtract(lineData.FirstProductionDate).Days;
//            if (surveyDays <= 0) surveyDays = 1;
//            lineData.DailyAverageKm = lineData.KmOfSurvey / surveyDays;
//            lineData.DailyAverageArea = lineData.AreaOfSurvey / surveyDays;
//            lineData.DailyAverageTotalRealized = lineData.TotalRealizedOfSurvey / surveyDays;
//            lineData.DailyAverageTotalNotRealized = lineData.TotalNotRealizedOfSurvey / surveyDays;
//            lineData.DailyAveragePercentage = lineData.PercentageOfSurvey / surveyDays;

//            //RESTANTE PARA TÉRMINO
//            lineData.MissingKmToCurrentPreplotVersionTotal = lineData.TotalKmOfCurrentPreplotVersion - lineData.KmOfSurvey;
//            lineData.MissingAreaToCurrentPreplotVersionTotal = lineData.TotalAreaOfCurrentPreplotVersion - lineData.AreaOfSurvey;
//            lineData.MissingPointsToCurrentPreplotVersionTotal = lineData.TotalPointsOfCurrentPreplotVersion - (lineData.TotalRealizedOfSurvey + lineData.TotalNotRealizedOfSurvey);
//            lineData.MissingPercentageToCurrentPreplotVersionTotal = 100 - lineData.PercentageOfSurvey;
//            //PREVISÃO DE TÉRMINO
//            if (lineData.MissingPercentageToCurrentPreplotVersionTotal == 0)
//            {
//                var lastProdDate = DateParser.DateToString(productionDal.GetLastProductionDate(surveyId, opFrontId));
//                lineData.FinishingDateForecast = null;
//                lineData.FinishingDateForecastString = "Encerrada em " + lastProdDate;
//            }
//            else
//            {
//                if (lineData.DailyAverageTotalRealized + lineData.DailyAverageTotalNotRealized == 0)
//                    lineData.FinishingDateForecastString = "Não iniciada";
//                else
//                {
//                    lineData.FinishingDateForecast = reportDate.AddDays(Math.Ceiling(lineData.MissingPointsToCurrentPreplotVersionTotal / (double)(lineData.DailyAverageTotalRealized + lineData.DailyAverageTotalNotRealized)));
//                    lineData.FinishingDateForecastString = DateParser.DateToString(lineData.FinishingDateForecast.Value);
//                }
//            }
//            return lineData;
//        }

//        internal static RdoFrontOverFrontTableDataDTO GetFrontOverFrontLine(IEnumerable<string> shotPointLinesNames, int opFrontId, string opFrontName, OperationalFrontType opFrontType, IList<StretchDTO> producedStretchesUntilDate, IList<OperationalFrontDTO> opFronts)
//        {
//            var lineData = new RdoFrontOverFrontTableDataDTO { FrontName = opFrontName, FrontId = opFrontId };
//            var orderedOpFrontsForColumns =
//                opFronts.OrderBy(m => m.OperationalFrontType).ThenBy(m => m.OperationalFrontId).ToList();
//            orderedOpFrontsForColumns.RemoveAt(0);

//            foreach (var otherFront in orderedOpFrontsForColumns)
//            {
//                var dist = new DistanceToOtherFrontDTO
//                {
//                    OperationalFrontId = otherFront.OperationalFrontId,
//                    Distance =
//                    (opFrontType == OperationalFrontType.Drilling ||
//                    opFrontType == OperationalFrontType.Charging ||
//                    opFrontType == OperationalFrontType.Detonation ||
//                    otherFront.OperationalFrontType == OperationalFrontType.Drilling ||
//                    otherFront.OperationalFrontType == OperationalFrontType.Charging ||
//                    otherFront.OperationalFrontType == OperationalFrontType.Detonation) ?
//                        (producedStretchesUntilDate.Where(s => s.OperationalFrontId == opFrontId
//                            && shotPointLinesNames.Contains(s.Line))
//                            .Sum(s => s.TotalKm) ?? 0) -
//                        (producedStretchesUntilDate.Where(s => s.OperationalFrontId == otherFront.OperationalFrontId
//                            && shotPointLinesNames.Contains(s.Line))
//                            .Sum(s => s.TotalKm) ?? 0)
//                        :
//                        (producedStretchesUntilDate.Where(s => s.OperationalFrontId == opFrontId)
//                            .Sum(s => s.TotalKm) ?? 0) -
//                        (producedStretchesUntilDate.Where(s => s.OperationalFrontId == otherFront.OperationalFrontId)
//                            .Sum(s => s.TotalKm) ?? 0)

//                };
//                dist.DistanceString = dist.Distance > 0 ?
//                    Convert.ToString(dist.Distance, CultureInfo.InvariantCulture) : "-";
//                lineData.DistancesToOtherFronts.Add(dist);
//            }
//            return lineData;
//        }

//        internal static List<RdoProductionsGroupedByTypeDTO> GetGroupedProductions(IEnumerable<PointProductionDTO> productionsOfDay)
//        {
//            return productionsOfDay
//                .GroupBy(p => p.PreplotPointType)
//                .Select(g => new RdoProductionsGroupedByTypeDTO
//                {
//                    PointType = g.Key,
//                    PointTypeName = Enumerations.GetEnumDescription(g.Key),
//                    PointProductions = g.OrderBy(p => p.Line).ThenBy(p => p.StationNumber).ToList()
//                }).ToList();
//        }

//    }
//}
