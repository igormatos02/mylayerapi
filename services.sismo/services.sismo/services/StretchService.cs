using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class StretchService : IStretchService
    {
        private readonly IFrontGroupLeaderRepository _frontGroupLeaderRepository;
        private readonly IFrontGroupRepository _frontGroupRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IPreplotVersionRepository _preplotVersionRepository;
        private readonly IPreplotPointService _preplotPointService;
        private readonly IOperationalFrontRepository _operationalFrontRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly IPointProductionRepository _pointProductionRepository;
        private readonly IStretchRepository _stretchRepository;
        private readonly IConfiguration _configuration;

        public StretchService(
            IFrontGroupLeaderRepository frontGroupLeaderRepository,
            IFrontGroupRepository frontGroupRepository,
            ILineRepository lineRepository,
            IPreplotVersionRepository preplotVersionRepository,
            IPreplotPointService preplotPointService,
            IOperationalFrontRepository operationalFrontRepository,
            ISurveyRepository surveyRepository,
            IPreplotPointRepository preplotPointRepository,
            IPointProductionRepository pointProductionRepository,
            IStretchRepository stretchRepository, 
            IConfiguration configuration)
        {
            _frontGroupLeaderRepository = frontGroupLeaderRepository;
            _frontGroupRepository = frontGroupRepository;
            _lineRepository = lineRepository;
            _preplotVersionRepository = preplotVersionRepository;
            _preplotPointService = preplotPointService;
            _operationalFrontRepository = operationalFrontRepository;
            _surveyRepository = surveyRepository;
            _preplotPointRepository = preplotPointRepository;
            _pointProductionRepository = pointProductionRepository;
            _stretchRepository = stretchRepository;
            _configuration = configuration;
        }
        public async Task SaveStretch(StretchModel stretch, bool recursive)
        {
            ///Arquivo de UML Save Strech com o diagrama do fluxo


            var operationalFrontType = await _operationalFrontRepository.GetOperationalFrontType(stretch.OperationalFrontId);
            PreplotPointType preplotPointType =  _preplotPointService.GetPreplotPointTypeByOpFrontType(operationalFrontType);

            // if(stretch.FinalStation<stretch.FinalStationBkp || stretch.InitialStation>stretch.InitialStationBkp)
            await _stretchRepository.DeleteStretch(stretch);
            //Parte 1
            //Unifica trechos com interceção para a mesma equipe no mesmo dia 
            //Apaga os trechos ja existentes na interseção e cria um novo unificado
            if (!recursive)
                stretch = await UnifyInterSectionedStretches(stretch, preplotPointType);

            stretch.NotRealizedCount = await _pointProductionRepository.CountProductions(stretch.SurveyId, stretch.Line, stretch.InitialStation, stretch.FinalStation, preplotPointType, (int)ProductionStatus.Unaccomplished, stretch.OperationalFrontId);
            stretch.RealizedCount = await _pointProductionRepository.CountProductions(stretch.SurveyId, stretch.Line, stretch.InitialStation, stretch.FinalStation, preplotPointType, (int)ProductionStatus.Accomplished, stretch.OperationalFrontId);
            stretch.RealizedOnlyStationsCount = await _pointProductionRepository.CountProductions(stretch.SurveyId, stretch.Line, stretch.InitialStation, stretch.FinalStation, preplotPointType, (int)ProductionStatus.OnlyReceptorStations, stretch.OperationalFrontId);
            //stretch.PendingCount = pointProdDal.CountProductions(stretch.SurveyId, stretch.Line, stretch.InitialStation, stretch.FinalStation, preplotPointType, (int)ProductionStatus.Pending, stretch.OperationalFrontId);


            //Parte 2 Ajusta os trechos
            await CalculateStretchKm(stretch, preplotPointType);
            await _stretchRepository.SaveStretch(stretch);

        }
        
        public async Task<StretchModel> UnifyInterSectionedStretches(StretchModel newStretch, PreplotPointType preplotPointType)
        {

            List<StretchModel> stretches = await _stretchRepository.ListStretchesWithIntersection(newStretch.SurveyId, newStretch.OperationalFrontId, newStretch.FrontGroupId, newStretch.FrontGroupLeaderId, newStretch.Line, newStretch.DateString, newStretch.InitialStation, newStretch.FinalStation);
            foreach (StretchModel dto in stretches)
            {
                if (dto.InitialStation < newStretch.InitialStation)
                    newStretch.InitialStation = dto.InitialStation;
                if (dto.FinalStation > newStretch.FinalStation)
                    newStretch.FinalStation = dto.FinalStation;

                await _stretchRepository.DeleteStretch(dto);
            }
            await _stretchRepository.SaveStretch(newStretch);

            int countPoints=0;
            //Recalcula o km do novo trecho unificado 
            newStretch.TotalKm = await _stretchRepository.CalculateKm(newStretch.SurveyId, preplotPointType, newStretch.Line, newStretch.InitialStation, newStretch.FinalStation,  null,  countPoints);

            return newStretch;
        }
       
        public async Task CalculateStretchKm(StretchModel stretch, PreplotPointType preplotPointType)
        {
            StretchModel nextStretch = await _stretchRepository.GetNextStation(stretch);
            StretchModel previousStretch = await _stretchRepository.GetPreviousStationt(stretch);

            var initialStation = stretch.InitialStation;
            var finalStation = stretch.FinalStation;
            bool isPreviousStretchConnected = await _preplotPointRepository.ArePointsConnected(stretch.SurveyId, stretch.Line,
                    previousStretch.FinalStation, stretch.InitialStation, preplotPointType);

            bool isNextStretchConnected = await _preplotPointRepository.ArePointsConnected(stretch.SurveyId, stretch.Line,
                    stretch.FinalStation, nextStretch.InitialStation, preplotPointType);

            if (previousStretch.DateString != null && isPreviousStretchConnected && previousStretch.KmRight == false)
            {
                initialStation = previousStretch.FinalStation;
                stretch.KmLeft = true;
            }

            if (nextStretch.DateString != null && isNextStretchConnected && nextStretch.KmLeft == false)
            {
                finalStation = nextStretch.InitialStation;
                stretch.KmRight = true;
            }

          
            int countPoints=0;
            stretch.TotalKm = await _stretchRepository.CalculateKm(stretch.SurveyId, preplotPointType, stretch.Line, initialStation, finalStation, null,  countPoints);
        }

        public async Task<List<FrontGroupProductionModel>> ListFrontProduction(int surveyId, int preplotPointType, int operationalFrontId,
            string date)
        {
            
            try
            {
                return await ListStretchGroups(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, date);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<FrontGroupProductionModel>> ListStretchGroups(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date)
        {
            var grouIndex = 1;
            
            var stretches = await _stretchRepository.ListStretches(surveyId, operationalFrontId, date);
            var stretchGroups = new List<FrontGroupProductionModel>();

            foreach (var stretch in stretches)
            {
                var hasInserted = false;

                //stretch.Points = new PreplotPointDAL().ListStationNumbers(surveyId, preplotPointType, stretch.Line);
                //stretch.PointsCount = stretch.Points.Count;

                var StretchModel = stretch;
                foreach (var @group in stretchGroups.
                    Where(@group =>
                        @group.FrontGroupLeaderId == StretchModel.FrontGroupLeaderId &&
                        @group.FrontGroupId == StretchModel.FrontGroupId))
                {
                    @group.Stretches.Add(StretchModel);
                    hasInserted = true;
                }
                if (hasInserted) continue;
                var newGroup = await InstantiateFrontGroupProduction(date, StretchModel.FrontGroupId, StretchModel.FrontGroupLeaderId, grouIndex);
                newGroup.Index = grouIndex++;
                stretchGroups.Add(newGroup);
                newGroup.Stretches.Add(StretchModel);
            }

            return stretchGroups;
        }

        public async Task<FrontGroupProductionModel> InstantiateFrontGroupProduction(String date, int frontGroupId, int frontGroupLeaderId, int groupIndex)
        {
            return new FrontGroupProductionModel
            {
                Index = groupIndex,
                IsFromDb = true,
                IsOpen = false,
                FrontGroupId = frontGroupId,
                FrontGroupName = await _frontGroupRepository.GetFrontGroupName(frontGroupId),
                FrontGroupLeaderId = frontGroupLeaderId,
                FrontGroupLeaderName = await _frontGroupLeaderRepository.GetFrontGroupLeaderName(frontGroupLeaderId),
                Date = date
            };
        }

        public async Task<bool> DeleteStretch(StretchModel stretch)
        {
            var type = await _operationalFrontRepository.GetOperationalFrontType(stretch.OperationalFrontId);
            PreplotPointType preplotPointType = _preplotPointService.GetPreplotPointTypeByOpFrontType(type);
            await _stretchRepository.DeleteStretch(stretch);
           return true;

        }

        public async Task<FrontOverFrontChartSerie> GetFrontOverFrontChartSeries(int surveyId, string reportDate, bool isDateTimeNow)
        {
            
            try
            {
               
                var surveyEndDate = await _surveyRepository.GetSurveyEndDate(surveyId);
                var date = surveyEndDate.HasValue ? (surveyEndDate.Value < DateTime.Now ? surveyEndDate.Value : DateTime.Now) : DateTime.Now;
                if (reportDate != "") date = DateHelper.GetDBValue(reportDate, "NormalDate");
                var opFronts = await _operationalFrontRepository.ListSurveyOperationalFronts(surveyId);

                var serie = new FrontOverFrontChartSerie();
                foreach (var front in opFronts)
                {
                    var serieValues = await _stretchRepository.GetFrontOverFrontChartSerie(surveyId, date, front.OperationalFrontId);
                    serie.Data.Add(new Serie()
                    {
                        Data  = serieValues.Select(m => new SerieValue() { Value = m.AccumulatedProduction ?? 0 }).ToList(),
                        Name = front.Name,
                        Color = front.OperationalFrontColor
                    }); 
                }

               
                var firstFront = opFronts.FirstOrDefault();
                if (firstFront != null)
                {
                    var dates = await _stretchRepository.GetFrontOverFrontChartSerie(surveyId, date,
                            firstFront.OperationalFrontId);
                    serie.Dates = dates
                            .Select(m => m.Date.ToString("dd/MM/yyyy"))
                            .ToList();
                }

                return serie;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProductionChartSerie> GetProductionChartSeries(int surveyId, string reportDate)
        {
            
            try
            {
                var _productions = await _stretchRepository.GetSurveyProductionData(surveyId, reportDate);
                var productions = _productions.OrderBy(m => m.OperationalFrontId);
                var fronts = await _operationalFrontRepository.ListSurveyOperationalFronts(surveyId);
                 
               return new ProductionChartSerie
               {
                        OperationalFronts = fronts.OrderBy(f => f.OperationalFrontId).Select(f => f.Name).ToList(),
                        TotalMissingSerie = productions.Select(m => m.TotalMissing).ToList(),
                        TotalRealizedSerie = productions.Select(m => m.TotalRealized).ToList(),
                        TotalNotRealizedSerie = productions.Select(m => m.TotalNotRealized).ToList()
                    
                };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<OpFrontTotalProductionGraphModel>> GetSurveyProductionData(int surveyId)
        {
            
            try
            {
                var data = await _stretchRepository.GetSurveyProductionData(surveyId, "");
                    return data.OrderBy(m => m.OperationalFrontId)
                    .ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProductionPerLinePerOperationalFrontSerie> GetProductionPerLinePerOperationalFront(int surveyId, int linePointsType)
        {
            
            try
            {
                var version = await _preplotVersionRepository.GetPreplotVersion(surveyId, DateTime.Now);
                if (version == null)
                    return new ProductionPerLinePerOperationalFrontSerie();
                else
                {
                    var versionId = version.PreplotVersionId;
                    var consideringLines = await _lineRepository.ListLinesNames(surveyId, versionId, (PreplotPointType)linePointsType);
                    if (consideringLines == null)
                        return new ProductionPerLinePerOperationalFrontSerie();
                    else
                    {
                        var lines = consideringLines.ToList();
                        var opFronts = await _operationalFrontRepository.ListSurveyOperationalFronts(surveyId);
                        if (opFronts == null)
                            return new ProductionPerLinePerOperationalFrontSerie();
                        else
                        {
                            var fronts = opFronts.ToList();
                            var series = await _stretchRepository.GetProductionPerLineSeries(surveyId,
                                DateHelper.DateToString(DateTime.Now), lines, fronts);
                           return new ProductionPerLinePerOperationalFrontSerie
                            {
                                Lines = lines,
                                Series = series
                            };
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProductionOfPeriodSerie> GetProductionOfPeriodChartData(int surveyId, int operationalFrontId, string initialDate, string finalDate)
        {
            
            try
            {
                
                var frontIniDate = await _pointProductionRepository.GetFirstProductionDate(surveyId, operationalFrontId);
                var frontLastProdDate = await _pointProductionRepository.GetLastProductionDate(surveyId, operationalFrontId);
                var iniDate = DateHelper.StringToDate(initialDate).Date;
                var finDate = DateHelper.StringToDate(finalDate).Date;
                var stretches = await _stretchRepository.ListStretches(surveyId, operationalFrontId, initialDate, finalDate);
                var seriePt = new List<SerieValue>();
                var serieKm = new List<SerieValue>();
                var serieAveragePt = new List<SerieValue>();
                var serieAverageKm = new List<SerieValue>();
                var days = new List<string>();
                int lastAvgxIndex = 0;
                int x = 0;
                for (var d = iniDate.Date; d <= finDate.Date; d = d.AddDays(1))
                {
                    var day = d.ToString("dd/MM");
                    days.Add(day);
                    seriePt.Add(new SerieValue
                    {
                        Name = day,
                        Y = (stretches.Where(m => m.Date == d).Sum(m => m.RealizedCount + m.RealizedOnlyStationsCount) ?? 0)
                    });
                    serieKm.Add(new SerieValue
                    {
                        Name = day,
                        Y = (stretches.Where(m => m.Date == d).Sum(m => m.TotalKm) ?? 0)
                    });
                    if (d <= frontLastProdDate)
                    {
                        var avgConsideringStretches = stretches.Where(m => m.Date >= frontIniDate && m.Date <= d).ToList();
                        var avgIniDate = iniDate > frontIniDate ? iniDate : frontIniDate;
                        var avgDaysCount =
                            (decimal)(d.Subtract(avgIniDate).Days > 0 ? d.Subtract(avgIniDate).Days + 1 : 1);
                        serieAveragePt.Add(new SerieValue
                        {
                            Name = day,
                            Y =
                                Math.Round(
                                    (avgConsideringStretches.Sum(m => m.RealizedCount + m.RealizedOnlyStationsCount) ??
                                     0) / avgDaysCount, 0)
                        });
                        serieAverageKm.Add(new SerieValue
                        {
                            Name = day,
                            Y =
                                Math.Round((decimal)((avgConsideringStretches.Sum(m => m.TotalKm) ?? 0) / avgDaysCount), 4)
                        });
                    }
                    else
                        lastAvgxIndex = x;
                    x++;
                }

                return new ProductionOfPeriodSerie
                {
                    Days = days,

                    Pontos = new Serie
                    {
                        Data = seriePt,
                        Name = "Pontos"
                    },
                    Quilometragem = new Serie
                    {
                        Data = serieKm,
                        Name = "km"
                    },
                    MediaPt = new Serie
                    {
                        Data = serieAveragePt,
                        Name = "Média Pontos"
                    },
                    MediaKm = new Serie
                    {
                        Data = serieAverageKm,
                        Name = "Média km"
                    },
                   
                    LastAvgX = lastAvgxIndex
                };
            }
            catch (Exception ex) { throw ex; }
        }

      
    }

}
