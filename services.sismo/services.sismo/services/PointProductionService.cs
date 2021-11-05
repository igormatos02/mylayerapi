//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace services.sismo.services
//{
//    public class PointProductionService : IPointProductionService
//    {
//        private readonly IFrontGroupLeaderRepository _frontGroupLeaderRepository;
//        private readonly IConfiguration _configuration;

//        public FrontGroupLeaderService(IFrontGroupLeaderRepository frontGroupLeaderRepository, IConfiguration configuration)
//        {
//            _frontGroupLeaderRepository = frontGroupLeaderRepository;
//            _configuration = configuration;
//        }
//        public async Task< ListDailyProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal? point, bool hasUnaccomplished)
//        {
            
//            try
//            {
//                List<TotalDailyProductionDTO> totalDailyProductionDTO = new StretchDAL().ListTotalDailyProductions(surveyId, operationalFrontId, line, point, hasUnaccomplished);//new PointProductionDAL().ListDailyProductions(surveyId, preplotPointType, operationalFrontId);
//                var pointProductionDal = new PointProductionDAL();
//                var opFrontDal = new OperationalFrontDAL();
//                OperationalFrontDTO operationalFront = opFrontDal.GetOperationalFront(operationalFrontId);
//                if (operationalFront != null && operationalFront.OperationalFrontType == OperationalFrontType.Charging)
//                {

//                    /* foreach (TotalDailyProductionDTO t in totalDailyProductionDTO)
//                     {

//                         foreach (StretchDTO s in t.Stretches)
//                         {
//                             var numberOfChargesInShotPoint = pointProductionDal.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfChargesInShotPoint);
//                             var numberOfFusesInShotPoint = pointProductionDal.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfFusesInShotPoint);
//                             t.NumberOfChargesInShotPoint += numberOfChargesInShotPoint.Value;
//                             t.NumberOfFusesInShotPoint += numberOfFusesInShotPoint.Value;
//                         }                                               
//                     }*/

//                }


//                result.Data = totalDailyProductionDTO;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< ListDailyStretches(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date)
//        {
            
//            try
//            {
//                List<StretchDTO> stretches = new StretchDAL().ListStretches(surveyId, operationalFrontId, date);
//                var pointProductionDal = new PointProductionDAL();
//                var opFrontDal = new OperationalFrontDAL();
//                OperationalFrontDTO operationalFront = opFrontDal.GetOperationalFront(operationalFrontId);
//                if (operationalFront != null && operationalFront.OperationalFrontType == OperationalFrontType.Charging)
//                {
//                    foreach (StretchDTO s in stretches)
//                    {
//                        var numberOfChargesInShotPoint = pointProductionDal.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfChargesInShotPoint);
//                        var numberOfFusesInShotPoint = pointProductionDal.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfFusesInShotPoint);
//                        s.NumberOfChargesInShotPoint += numberOfChargesInShotPoint.Value;
//                        s.NumberOfFusesInShotPoint += numberOfFusesInShotPoint.Value;
//                    }

//                }
//                result.Data = stretches;

//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public static async Task<Result> ImportBruteRobFile(string fileName, byte[] byteStream, BruteRobParametersDTO bruteRobParameters)
//        {
            
//            try
//            {
//                if (!BruteRobImportHelperBLL.HasValidExtension(fileName)) throw new FileLoadException("Tipo de arquivo inválido");

//                if (bruteRobParameters == null || bruteRobParameters.OperationalFrontId == 0 || bruteRobParameters.SurveyId == 0 ||
//                    bruteRobParameters.FrontGroupId == 0 || bruteRobParameters.FrontGroupLeaderId == 0)
//                    throw new Exception("Favor selecionar a turma e o líder da turma");

//                var consideringPoints =
//                    new PreplotPointDAL().ListPreplotPointsWithoutCoordinates(bruteRobParameters.SurveyId,
//                        PreplotPointType.ShotPoint, bruteRobParameters.LineStretches).ToList();

//                Stream stream = new MemoryStream(byteStream);
//                var registersAndProductions = BruteRobImportHelperBLL.ExtractSeismicRegistersAndProductions(stream, bruteRobParameters, consideringPoints);
//                stream.Flush();

//                var seisRegisters = registersAndProductions.Where(m => m.Key != null).Select(m => m.Key).ToList();
//                var repeatingFfids = seisRegisters.GroupBy(r => r.Ffid).SelectMany(grp => grp.Skip(1)).Select(m => m.Ffid).ToList(); //no proprio arquivo
//                repeatingFfids.AddRange(new SeismicRegisterDAL().ListExistingFfids(seisRegisters.Select(m => m.Ffid).ToList()));//no banco
//                if (repeatingFfids.Any())
//                {
//                    var errorMsg = "Existem FFID's repetidos no arquivo importado:" + Environment.NewLine;
//                    errorMsg = repeatingFfids.Aggregate(errorMsg, (current, ffid) => current + ffid + Environment.NewLine);
//                    throw new Exception(errorMsg);
//                }

//                var productions = registersAndProductions.Where(m => m.Value != null).Select(m => m.Value).ToList();
//                result.Data = new
//                {
//                    Productions = productions,
//                    SeisRegisters = seisRegisters
//                };
//            }
//            catch (Exception ex)
//            {
//                result.Data = null;
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< SaveSeismicRegistersWithProductions(int surveyId, int operationalFrontId, IList<SeismicRegisterDTO> registers, int frontGroupId, int frontGroupLeaderId)
//        {
            
//            try
//            {
//                var productions = registers.Select(m => m.PointProduction).ToList();
//                result = SaveProduction(surveyId, operationalFrontId, productions, frontGroupId, frontGroupLeaderId, 0, 0, true, "");
//                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
//                    throw new Exception("Erro ao Salvar as Produções.");
//                new SeismicRegisterDAL().SaveSeismicRegisters(registers);
//                result.Data = true;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< SaveProductionFromBruteRobFile(List<PointProductionDTO> productions, IEnumerable<SeismicRegisterDTO> seisRegisters, BruteRobParametersDTO bruteRobParameters)
//        {
            
//            try
//            {
//                if (bruteRobParameters == null || bruteRobParameters.OperationalFrontId == 0 || bruteRobParameters.SurveyId == 0 ||
//                    bruteRobParameters.FrontGroupId == 0 || bruteRobParameters.FrontGroupLeaderId == 0)
//                    throw new Exception("Favor selecionar a turma e o líder da turma");

//                var stretchesProductions = bruteRobParameters.LineStretches.
//                    Select(stretch => new List<PointProductionDTO>(
//                        productions.Where(p => p.Line == stretch.Line &&
//                            p.StationNumber >= stretch.InitialStation &&
//                            p.StationNumber <= stretch.FinalStation).ToList())).ToList();

//                //SALVAR POR TRECHO
//                var savingProductionResult = SaveMultipleStretchesProduction(bruteRobParameters.SurveyId,
//                    bruteRobParameters.OperationalFrontId, stretchesProductions,
//                    bruteRobParameters.FrontGroupId, bruteRobParameters.FrontGroupLeaderId);

//                result = savingProductionResult;

//                if (!string.IsNullOrWhiteSpace(savingProductionResult.ErrorMessage)) return result;

//                new SeismicRegisterDAL().SaveSeismicRegisters(seisRegisters);
//                result.Data = true;
//            }
//            catch (Exception ex)
//            {
//                result.Data = null;
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< GetImportRobScreenDetails(int operationalFrontId)
//        {
            
//            try
//            {
//                result.Data = new
//                {
//                    Leaders = (new FrontGroupLeaderDAL()).ListFrontGroupLeaders(operationalFrontId).ToList(),
//                    Groups = (new FrontGroupDAL()).ListFrontGroups(operationalFrontId).ToList()
//                };
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< GetProductionScreenDetails(int surveyId, int preplotPointType, int operationalFrontId, string date)
//        {
            
//            try
//            {
//                var operationalFrontType = new OperationalFrontDAL().GetOperationalFront(operationalFrontId).OperationalFrontType;
//                var stretchGroups = //operationalFrontType == OperationalFrontType.Permit
//                                    //? ListPermitGroupsProductionsPrivate(surveyId, operationalFrontId, DateParser.StringToDate(date)) :
//                    StretchBLL.ListStretchGroups(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, date);
//                //ListFrontGroupsProductionsPrivate(surveyId, (PreplotPointType)preplotPointType,operationalFrontId, DateParser.StringToDate(date));

//                result.Data = new
//                {
//                    Leaders = (new FrontGroupLeaderDAL()).ListFrontGroupLeaders(operationalFrontId).ToList(),
//                    Groups = (new FrontGroupDAL()).ListFrontGroups(operationalFrontId).ToList(),
//                    Lines = new PreplotPointDAL().ListLines(surveyId, (PreplotPointType)preplotPointType),
//                    StatusList = operationalFrontType == OperationalFrontType.Permit ?
//                        new List<GenericValueDTO>
//                            {
//                                new GenericValueDTO {Value = (int) ProductionStatus.Unaccomplished, Description = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, operationalFrontType)},
//                                new GenericValueDTO {Value = (int) ProductionStatus.Accomplished, Description = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, operationalFrontType)},
//                                new GenericValueDTO {Value = (int) ProductionStatus.OnlyReceptorStations, Description = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.OnlyReceptorStations, operationalFrontType)}
//                            }
//                        : new List<GenericValueDTO>
//                            {
//                                new GenericValueDTO {Value = (int) ProductionStatus.Unaccomplished, Description = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, operationalFrontType)},
//                                new GenericValueDTO {Value = (int) ProductionStatus.Accomplished, Description = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, operationalFrontType)}
//                            },
//                    QcStatusList = operationalFrontType == OperationalFrontType.Detonation ?
//                        Enumerations.ListEnumObjectsToScreen(typeof(QualityControlStatus))
//                        : null,
//                    DisplacementRules = (new DisplacementRuleDAL()).ListDisplacementRules(surveyId, true, (int)DisplacementRuleType.Seismic).Where(d => d.IsActive == true).ToList(),
//                    ReductionRules = (new ReductionRuleDAL()).ListReductionRules(surveyId, true).ToList(),
//                    StretchGroups = stretchGroups
//                    //SurveyLands = operationalFrontType == OperationalFrontType.Permit ? LandBLL.ListLands(surveyId, false) : new List<LandDTO>()
//                };
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< GetFieldProductionReportData(int surveyId, int preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation, int itemsPerPage)
//        {
            
//            try
//            {
//                var opFrontDal = new OperationalFrontDAL();
//                var surveyDal = new SurveyDAL();
//                var frontType = opFrontDal.GetOperationalFrontType(operationalFrontId);
//                var survey = surveyDal.GetSurveyWithoutDetails(surveyId);
//                int? previousStationsOpFrontId = null;
//                var previousOpFrontId = opFrontDal.GetPreviousOperationalFrontId(operationalFrontId);
//                List<PointProductionDTO> productionsList;
//                if (!previousOpFrontId.HasValue)
//                    throw new Exception("Não existe uma frente operacional anterior à selecionada.");
//                if (frontType == OperationalFrontType.Inspection && preplotPointType == (int)PreplotPointType.All)
//                {
//                    var topoFronts = surveyDal.ListSurveyOperationalFronts(surveyId, (int)OperationalFrontType.Topography).ToList();
//                    if (topoFronts.Any()) previousStationsOpFrontId = topoFronts.First().OperationalFrontId;
//                    if (!previousStationsOpFrontId.HasValue)
//                        throw new Exception("Não existe uma frente de Topografia anterior à frente selecionada.");
//                    productionsList =
//                        ListStretchPoints(surveyId, previousOpFrontId.Value, previousStationsOpFrontId.Value, line,
//                            initialStation, finalStation).OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//                }
//                else
//                    productionsList =
//                        ListStretchPoints(surveyId, (PreplotPointType)preplotPointType, previousOpFrontId.Value, line,
//                            initialStation, finalStation).OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//                var reportData = new FieldReportDataDTO
//                {
//                    HolesArrangementDescription = survey.HolesArrangementDescription,
//                    HolesArrangementImagePath = survey.HolesArrangementImagePath,
//                    HolesQuantity = survey.HolesPerShotPoint ?? 0,
//                    Line = line,
//                    PointProductions = productionsList.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / itemsPerPage).Select(x => x.Select(v => v.Value).ToList()).ToList()
//                };
//                result.Data = reportData;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< GetFieldProductionReportData(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber, int itemsPerPage)
//        {
            
//            try
//            {
//                var previousOpFrontId = new OperationalFrontDAL().GetPreviousOperationalFrontId(operationalFrontId);
//                if (!previousOpFrontId.HasValue) throw new Exception("Não existe uma frente operacional anterior à selecionada.");
//                var survey = new SurveyDAL().GetSurveyWithoutDetails(surveyId);
//                var productionsList = ListStretchPoints(surveyId, preplotPointType, previousOpFrontId.Value, swathNumber).OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//                var reportData = new FieldReportDataDTO
//                {
//                    HolesArrangementDescription = survey.HolesArrangementDescription,
//                    HolesArrangementImagePath = survey.HolesArrangementImagePath,
//                    HolesQuantity = survey.HolesPerShotPoint ?? 0,
//                    PointProductions = productionsList.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / itemsPerPage).Select(x => x.Select(v => v.Value).ToList()).ToList()
//                };
//                result.Data = reportData;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        //public async Task< ListDetonationProductions(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation, string date, int frontGroupLeaderId, int frontGroupId)
//        //{
//        //    
//        //    try
//        //    {
//        //        if (initialStation > finalStation)
//        //        {
//        //            var bkp = initialStation;
//        //            initialStation = finalStation;
//        //            finalStation = bkp;
//        //        }
//        //        var detonProds = new PointProductionDAL().ListStretchDetonationProductions(surveyId, operationalFrontId, line, initialStation, finalStation);
//        //        result.Data = detonProds.OrderBy(m => m.Ffid).GroupBy(m => new { m.PreplotPointId, m.PreplotPointType }).ToList();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        result.ErrorMessage = ex.Message;
//        //        result.ErrorCode = ErrorCode.DalException;
//        //    }
//        //    return result;
//        //}

//        public async Task< ListSeismicRegisters(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
//        {
            
//            try
//            {
//                if (initialStation > finalStation)
//                {
//                    var bkp = initialStation;
//                    initialStation = finalStation;
//                    finalStation = bkp;
//                }
//                var seismicRegisters = new SeismicRegisterDAL().ListSeismicRegisters(surveyId, operationalFrontId, line, initialStation, finalStation);
//                result.Data = seismicRegisters.OrderBy(m => m.Ffid).ToList();
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< ListSeismicRegisters(int surveyId, int operationalFrontId, int swathNumber)
//        {
            
//            try
//            {
//                var swath = new SwathDAL().GetSwath(surveyId, swathNumber);
//                var swathStretches = new PreplotPointDAL().ListStretchesFromSwath(surveyId, PreplotPointType.ShotPoint,
//                    swath.InitialShotPoint, swath.FinalShotPoint);
//                var seismicRegisters = new SeismicRegisterDAL().ListSeismicRegisters(surveyId, operationalFrontId, swathStretches);
//                result.Data = seismicRegisters.OrderBy(m => m.Ffid).ToList();
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public static IList<PointProductionDTO> ListStretchPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
//        {
//            if (initialStation > finalStation)
//            {
//                var bkp = initialStation;
//                initialStation = finalStation;
//                finalStation = bkp;
//            }
//            var surveyDal = new SurveyDAL();
//            var redRuleDal = new ReductionRuleDAL();
//            //BUSCA DADOS GERAIS DOS PONTOS
//            var surveyHolesQuantity = surveyDal.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
//            var surveyHolesDepth = surveyDal.GetSurveyDefaultHolesDepth(surveyId);
//            var pointProductionDal = new PointProductionDAL();
//            //BUSCA PRODUÇõES E PONTOS DO TRECHO
//            var productions =
//                pointProductionDal.ListStretchLastWorks(surveyId, preplotPointType,
//                    operationalFrontId, line, initialStation, finalStation).ToList();
//            var points =
//                new PreplotPointDAL().ListPreplotPointsWithoutCoordinates(surveyId, preplotPointType, line,
//                    initialStation, finalStation)
//                    .ToList();
//            var index = 1;
//            var opFrontType = new OperationalFrontDAL().GetOperationalFrontType(operationalFrontId);

//            foreach (var point in points)
//            {
//                var previousProductionObs = "";
//                previousProductionObs =
//                        pointProductionDal.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
//                            point.PreplotVersionId, point.PreplotPointType, operationalFrontId);

//                var pointProduction = productions.FirstOrDefault(sp =>
//                    sp.PreplotPointId == point.PreplotPointId &&
//                    sp.PreplotVersionId == point.PreplotVersionId &&
//                    sp.PreplotPointType == point.PreplotPointType);

//                if (pointProduction == null)
//                {
//                    var production = new PointProductionDTO
//                    {
//                        Index = index,
//                        IsFromDb = false,
//                        Line = line,
//                        PreplotPointType = point.PreplotPointType,
//                        StationNumber = point.StationNumber,
//                        PreplotPointId = point.PreplotPointId,
//                        PreplotVersionId = point.PreplotVersionId,
//                        SurveyDefaultHolesQuantity = surveyHolesQuantity,
//                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
//                        WorkNumber = 0,
//                        HolesDepth = 0,
//                        PreviousFrontProductionObservation = previousProductionObs
//                    };
//                    production.Holes = ListHoles(production, surveyHolesQuantity, redRuleDal, opFrontType);
//                    productions.Add(production);
//                }
//                else
//                {
//                    pointProduction.Index = index;
//                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity;
//                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
//                    //POINT INFO------------------------
//                    pointProduction.Line = point.LineName;
//                    pointProduction.StationNumber = point.StationNumber;
//                    //HOLES INFO------------------------
//                    pointProduction.Holes = ListHoles(pointProduction, surveyHolesQuantity, redRuleDal, opFrontType);
//                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;
//                }
//                index++;
//            }
//            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//        }

//        public static IList<PointProductionDTO> ListStretchPoints(int surveyId, int shotPointsOpFrontId, int stationsOpFrontId, string line, decimal initialStation, decimal finalStation)
//        {
//            if (initialStation > finalStation)
//            {
//                var bkp = initialStation;
//                initialStation = finalStation;
//                finalStation = bkp;
//            }
//            var surveyDal = new SurveyDAL();
//            var redRuleDal = new ReductionRuleDAL();
//            //BUSCA DADOS GERAIS DOS PONTOS
//            var surveyHolesQuantity = surveyDal.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
//            var surveyHolesDepth = surveyDal.GetSurveyDefaultHolesDepth(surveyId);
//            var pointProductionDal = new PointProductionDAL();
//            var preplotPointDal = new PreplotPointDAL();
//            //BUSCA PRODUÇõES E PONTOS DO TRECHO
//            var shotPointProductions =
//                pointProductionDal.ListStretchLastWorks(surveyId, PreplotPointType.ShotPoint,
//                    shotPointsOpFrontId, line, initialStation, finalStation).ToList();
//            var stationsProductions =
//                pointProductionDal.ListStretchLastWorks(surveyId, PreplotPointType.ReceiverStation,
//                    stationsOpFrontId, line, initialStation, finalStation).ToList();
//            var points =
//                preplotPointDal.ListPreplotPointsWithoutCoordinates(surveyId, PreplotPointType.All, line,
//                    initialStation, finalStation)
//                    .ToList();
//            var index = 1;
//            var shotPointOpFrontType = new OperationalFrontDAL().GetOperationalFrontType(shotPointsOpFrontId);
//            var productions = new List<PointProductionDTO>();
//            foreach (var point in points)
//            {
//                var previousProductionObs = "";
//                previousProductionObs =
//                        pointProductionDal.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
//                            point.PreplotVersionId, point.PreplotPointType, shotPointsOpFrontId);

//                var pointProduction =
//                    shotPointProductions.FirstOrDefault(sp =>
//                        sp.PreplotPointId == point.PreplotPointId &&
//                        sp.PreplotVersionId == point.PreplotVersionId &&
//                        sp.PreplotPointType == point.PreplotPointType)
//                        ??
//                    stationsProductions.FirstOrDefault(sp =>
//                        sp.PreplotPointId == point.PreplotPointId &&
//                        sp.PreplotVersionId == point.PreplotVersionId &&
//                        sp.PreplotPointType == point.PreplotPointType);
//                if (pointProduction == null)
//                {
//                    var builtProduction = new PointProductionDTO
//                    {
//                        Index = index,
//                        IsFromDb = false,
//                        Line = line,
//                        PreplotPointType = point.PreplotPointType,
//                        StationNumber = point.StationNumber,
//                        PreplotPointId = point.PreplotPointId,
//                        PreplotVersionId = point.PreplotVersionId,
//                        SurveyDefaultHolesQuantity = surveyHolesQuantity,
//                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
//                        WorkNumber = 0,
//                        HolesDepth = 0,
//                        PreviousFrontProductionObservation = previousProductionObs
//                    };
//                    //if (point.PreplotPointType == PreplotPointType.ShotPoint)
//                    builtProduction.Holes = ListHoles(builtProduction, surveyHolesQuantity, redRuleDal, shotPointOpFrontType);
//                    productions.Add(builtProduction);
//                }
//                else
//                {
//                    pointProduction.Index = index;
//                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity;
//                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
//                    //POINT INFO------------------------
//                    pointProduction.Line = point.LineName;
//                    pointProduction.StationNumber = point.StationNumber;
//                    //HOLES INFO------------------------
//                    //if (point.PreplotPointType == PreplotPointType.ShotPoint)
//                    pointProduction.Holes = ListHoles(pointProduction, surveyHolesQuantity, redRuleDal, shotPointOpFrontType);
//                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;

//                    productions.Add(pointProduction);
//                }

//                index++;
//            }
//            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//        }

//        public static IList<PointProductionDTO> ListStretchPoints(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber)
//        {
//            var surveyDal = new SurveyDAL();
//            var redRuleDal = new ReductionRuleDAL();
//            //BUSCA DADOS GERAIS DOS PONTOS
//            var surveyHolesQuantity = surveyDal.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
//            var surveyHolesDepth = surveyDal.GetSurveyDefaultHolesDepth(surveyId);
//            var pointProductionDal = new PointProductionDAL();
//            //BUSCA PRODUÇõES E PONTOS DO TRECHO
//            var swath = new SwathDAL().GetSwath(surveyId, swathNumber);
//            var productions =
//                pointProductionDal.ListStretchLastWorks(surveyId,
//                    operationalFrontId, (PreplotPointType)preplotPointType, swath.InitialShotPoint,
//                    swath.FinalShotPoint).ToList();
//            var points =
//                new PreplotPointDAL().ListPreplotPointsWithoutCoordinates(surveyId, (PreplotPointType)preplotPointType,
//                    swath.InitialShotPoint, swath.FinalShotPoint).ToList();
//            var index = 1;
//            var opFrontType = new OperationalFrontDAL().GetOperationalFrontType(operationalFrontId);

//            foreach (var point in points)
//            {
//                var previousProductionObs = "";
//                previousProductionObs =
//                        pointProductionDal.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
//                            point.PreplotVersionId, point.PreplotPointType, operationalFrontId);

//                var pointProduction = productions.FirstOrDefault(sp =>
//                    sp.PreplotPointId == point.PreplotPointId &&
//                    sp.PreplotVersionId == point.PreplotVersionId &&
//                    sp.PreplotPointType == point.PreplotPointType);

//                if (pointProduction == null)
//                {
//                    var production = new PointProductionDTO
//                    {
//                        Index = index,
//                        IsFromDb = false,
//                        Line = point.LineName,
//                        PreplotPointType = point.PreplotPointType,
//                        StationNumber = point.StationNumber,
//                        PreplotPointId = point.PreplotPointId,
//                        PreplotVersionId = point.PreplotVersionId,
//                        SurveyDefaultHolesQuantity = surveyHolesQuantity,
//                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
//                        WorkNumber = 0,
//                        HolesDepth = 0,
//                        PreviousFrontProductionObservation = previousProductionObs
//                    };
//                    production.Holes = ListHoles(production, surveyHolesQuantity, redRuleDal, opFrontType);
//                    productions.Add(production);
//                }
//                else
//                {
//                    pointProduction.Index = index;
//                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity;
//                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
//                    //POINT INFO------------------------
//                    pointProduction.Line = point.LineName;
//                    pointProduction.StationNumber = point.StationNumber;
//                    //HOLES INFO------------------------
//                    pointProduction.Holes = ListHoles(pointProduction, surveyHolesQuantity, redRuleDal, opFrontType);
//                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;
//                }
//                index++;
//            }
//            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//        }

//        public async Task< ListOrBuildStretchPoints(int surveyId, int preplotPointType, int operationalFrontId, string line,
//            int initialStation, int finalStation, string date, int frontGroupId, int frontGroupLeaderId)
//        {
//            if (initialStation > finalStation)
//            {
//                var bkp = initialStation;
//                initialStation = finalStation;
//                finalStation = bkp;
//            }
            
//            try
//            {
//                var surveyDal = new SurveyDAL();
//                var opFrontDal = new OperationalFrontDAL();
//                var pointProductionDal = new PointProductionDAL();
//                //BUSCA DADOS GERAIS DOS PONTOS
//                var frontGroupName = new FrontGroupDAL().GetFrontGroupName(frontGroupId);
//                var frontGroupLeaderName = new FrontGroupLeaderDAL().GetFrontGroupLeaderName(frontGroupLeaderId);
//                var dateFormated = DateParser.StringToDate(date);
//                var surveyHolesDepth = surveyDal.GetSurveyDefaultHolesDepth(surveyId);
//                var surveyHolesQuantity = surveyDal.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
//                var opFrontType = opFrontDal.GetOperationalFrontType(operationalFrontId);
//                var previousOperationalFrontId = opFrontDal.GetPreviousOperationalFrontId(operationalFrontId);


//                //BUSCA PRODUÇõES E PONTOS DO TRECHO
//                var productions = pointProductionDal.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, line, initialStation, finalStation, 1).ToList();
//                var points = new PreplotPointDAL().ListPreplotPointsWithoutCoordinates(surveyId, (PreplotPointType)preplotPointType, line, initialStation, finalStation).ToList();
//                IList<PointProductionDTO> newProductions = new List<PointProductionDTO>();

//                foreach (var point in points)
//                {
//                    var pointProduction = productions.FirstOrDefault(sp =>
//                        sp.PreplotPointId == point.PreplotPointId &&
//                        sp.PreplotVersionId == point.PreplotVersionId &&
//                        sp.PreplotPointType == point.PreplotPointType);

//                    var previousProduction = new PointProductionDTO();
//                    if (previousOperationalFrontId.HasValue)
//                        previousProduction =
//                            pointProductionDal.GetPreviousFrontLastProductionOrRework(surveyId, point.PreplotPointId,
//                                point.PreplotVersionId, point.PreplotPointType, previousOperationalFrontId.Value) ??
//                            new PointProductionDTO();

//                    if (pointProduction == null)
//                    {

//                        var productionStatus = (previousProduction.Status == (int)ProductionStatus.OnlyReceptorStations || previousProduction.Status == (int)ProductionStatus.Accomplished)
//                                                ? (int)ProductionStatus.Accomplished
//                                                : (int)ProductionStatus.Unaccomplished;

//                        var finalHolesQuantity = (opFrontType == OperationalFrontType.Drilling ? surveyHolesQuantity : (previousProduction.Status != (int)ProductionStatus.Accomplished
//                                                ? 0
//                                                : previousProduction.FinalHolesQuantity));

//                        var numberOfChargesInShotPoint = previousProduction.Status != (int)ProductionStatus.Accomplished
//                                                ? 0
//                                                : previousProduction.FinalHolesQuantity;

//                        var numberOfFusesInshotPoint = previousProduction.Status != (int)ProductionStatus.Accomplished
//                                                ? 0
//                                                : previousProduction.FinalHolesQuantity;


//                        var newProd = new PointProductionDTO
//                        {
//                            IsFromDb = false,
//                            IsFromAnotherDate = false,
//                            Line = line,
//                            PreplotPointType = point.PreplotPointType,
//                            PreplotPointTypeName = Enumerations.GetEnumDescription(point.PreplotPointType),
//                            DateString = date,
//                            Date = dateFormated,
//                            SavingDate = dateFormated,
//                            StationNumber = point.StationNumber,
//                            PreplotPointId = point.PreplotPointId,
//                            PreplotVersionId = point.PreplotVersionId,
//                            FrontGroupId = frontGroupId,
//                            FrontGroupName = frontGroupName,
//                            FrontGroupLeaderId = frontGroupLeaderId,
//                            FrontGroupLeaderName = frontGroupLeaderName,
//                            SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
//                            //PREVIOUS PRODUCTION INFO----------
//                            PreviousFrontProduction = previousProduction,
//                            //DEFAULT VALUES--------------
//                            Status = productionStatus,
//                            Observation = previousProduction.Observation,
//                            DisplacementRuleId = previousProduction.DisplacementRuleId,
//                            ReductionRuleId = previousProduction.ReductionRuleId,
//                            FinalHolesQuantity = finalHolesQuantity,
//                            SurveyDefaultHolesQuantity = surveyHolesQuantity,
//                            WorkNumber = 1,
//                            NumberOfChargesInShotPoint = numberOfChargesInShotPoint,
//                            NumberOfFusesInShotPoint = numberOfFusesInshotPoint,
//                            FusesType = previousProduction.FusesType,
//                            ChargesType = previousProduction.ChargesType
//                        };
//                        newProd.HolesDepth = (opFrontType == OperationalFrontType.Permit ||
//                                              opFrontType == OperationalFrontType.Topography ||
//                                              opFrontType == OperationalFrontType.SeismoA ||
//                                              opFrontType == OperationalFrontType.SeismoB ||
//                                              opFrontType == OperationalFrontType.Gravimetry ||
//                                              opFrontType == OperationalFrontType.Magnetometry)
//                                              ? 0 :
//                                              (previousProduction.HolesDepth >= 0 ?
//                                                previousProduction.HolesDepth : surveyHolesQuantity);
//                        newProductions.Add(newProd);
//                    }
//                    else
//                    {
//                        if (pointProduction.Date != dateFormated) pointProduction.IsFromAnotherDate = true;
//                        //POINT INFO------------------------
//                        pointProduction.Line = point.LineName;
//                        pointProduction.StationNumber = point.StationNumber;
//                        //PREVIOUS PRODUCTION INFO----------
//                        pointProduction.PreviousFrontProduction = previousProduction;
//                        pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity;
//                        pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
//                        pointProduction.SavingDate = dateFormated;
//                        if (pointProduction.Observation == null || pointProduction.Observation == "")
//                            pointProduction.Observation = previousProduction.Observation;
//                        if (opFrontType == OperationalFrontType.Charging || opFrontType == OperationalFrontType.Detonation)
//                        {
//                            pointProduction.ReductionRuleId = previousProduction.ReductionRuleId;
//                            pointProduction.DisplacementRuleId = previousProduction.DisplacementRuleId;
//                            pointProduction.HolesDepth = previousProduction.HolesDepth;
//                            pointProduction.ChargesType = previousProduction.ChargesType;
//                            pointProduction.FusesType = previousProduction.FusesType;
//                        }

//                        newProductions.Add(pointProduction);
//                    }
//                }
//                result.Data = newProductions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< SaveProduction(int surveyId, int operationalFrontId,
//            List<PointProductionDTO> pointProductions, int frontGroupId, int frontGroupLeaderId, decimal initialStationBkp, decimal finalStationBkp, bool savingFromRobFile, String userLogin)
//        {
            
//            var pointProductionDal = new PointProductionDAL();
//            var holeDal = new HoleDAL();
//            var stretchDal = new StretchDAL();
//            pointProductionDal.BeginTransaction();
//            holeDal.BeginTransaction();
//            stretchDal.BeginTransaction();
//            try
//            {
//                var operationalFront = new OperationalFrontDAL().GetOperationalFront(operationalFrontId);

//                List<PointProductionDTO> invalidProductions;
//                List<PointProductionDTO> validProductions;
//                List<HoleDTO> holesToDelete;
//                List<HoleDTO> holesToCreate;

//                ValidateProduction(surveyId, pointProductions, frontGroupId, frontGroupLeaderId,
//                    out validProductions, out invalidProductions, out holesToDelete, out holesToCreate, operationalFront);

//                foreach (PointProductionDTO p in validProductions)
//                    p.LastEditorUserLogin = userLogin;

//                if (invalidProductions.Any())
//                {
//                    invalidProductions.AddRange(validProductions);
//                    var productionsWithErrors = invalidProductions;
//                    result.Data = productionsWithErrors;
//                    result.ErrorCode = ErrorCode.PassToCallback;
//                    result.ErrorMessage =
//                        "A produção do trecho selecionado não foi gravada. Existem alguns pontos impossibilitados de serem salvos.";
//                }
//                else
//                {
//                    var firstPointProductionDto = pointProductions.ToList().FirstOrDefault();
//                    var stretchDto = new StretchDTO();
//                    if (firstPointProductionDto != null)
//                    {
//                        stretchDto.DateString = DateParser.DateToString(firstPointProductionDto.Date);
//                        stretchDto.FinalStation = pointProductions.Max(m => m.StationNumber);
//                        stretchDto.InitialStation = pointProductions.Min(m => m.StationNumber);
//                        stretchDto.FinalStationBkp = finalStationBkp == 0 ? pointProductions.Max(m => m.StationNumber) : finalStationBkp;
//                        stretchDto.InitialStationBkp = initialStationBkp == 0 ? pointProductions.Min(m => m.StationNumber) : initialStationBkp;
//                        stretchDto.FrontGroupId = frontGroupId;
//                        stretchDto.FrontGroupLeaderId = frontGroupLeaderId;
//                        stretchDto.Line = firstPointProductionDto.Line;
//                        stretchDto.OperationalFrontId = operationalFrontId;
//                        stretchDto.SurveyId = surveyId;
//                    }

//                    //TRATAR A AÇÂO COMO TRANSACTION, COM POSSIBILIDADE DE ROLLBACK EM CASO DE ERRO
//                    if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//                        holeDal.DeleteHoles(holesToDelete);

//                    var productionsToDelete = new List<PointProductionDTO>();
//                    if (stretchDto.InitialStation > stretchDto.InitialStationBkp)
//                    {
//                        var prodsToDelete = pointProductionDal.ListStretchProductions(surveyId,
//                            PreplotPointType.All,
//                            operationalFront.OperationalFrontId, stretchDto.Line,
//                            stretchDto.InitialStationBkp, stretchDto.InitialStation, 1).OrderBy(m => m.StationNumber).ToList();
//                        prodsToDelete.Remove(prodsToDelete.LastOrDefault());
//                        productionsToDelete.AddRange(prodsToDelete);
//                    }
//                    if (stretchDto.FinalStation < stretchDto.FinalStationBkp)
//                    {
//                        var prodsToDelete = pointProductionDal.ListStretchProductions(surveyId,
//                            PreplotPointType.All,
//                            operationalFront.OperationalFrontId, stretchDto.Line,
//                            stretchDto.FinalStation, stretchDto.FinalStationBkp, 1).OrderBy(m => m.StationNumber).ToList();
//                        prodsToDelete.Remove(prodsToDelete.FirstOrDefault());
//                        productionsToDelete.AddRange(prodsToDelete);
//                    }

//                    var deletionResult = DeleteProduction(surveyId, operationalFrontId, productionsToDelete, 1, pointProductionDal, holeDal);
//                    if (!string.IsNullOrWhiteSpace(deletionResult.ErrorMessage))
//                        throw new Exception("Não foi possível editar o trecho selecionado. Verifique as produções lançadas.");

//                    if (operationalFront.OperationalFrontType == OperationalFrontType.Detonation && !savingFromRobFile)
//                    {
//                        var seisRegDal = new SeismicRegisterDAL();
//                        var lastFfid = seisRegDal.GetLastFfid(surveyId);
//                        var seisRegisters = new List<SeismicRegisterDTO>();
//                        foreach (var prod in validProductions)
//                        {
//                            prod.LastEditorUserLogin = userLogin;
//                            lastFfid++;
//                            if (!prod.Ffid.HasValue)
//                                prod.Ffid = lastFfid;
//                            if (prod.Ffid != null)
//                                seisRegisters.Add(new SeismicRegisterDTO()
//                                {
//                                    Ffid = prod.Ffid.Value,
//                                    SurveyId = surveyId,
//                                    Comment = prod.Observation,
//                                    Date = prod.Date,
//                                    IsNoiseTest = false,
//                                    LineName = prod.Line,
//                                    OperationalFrontId = prod.OperationalFrontId,
//                                    PointNumber = prod.StationNumber,
//                                    PreplotPointId = prod.PreplotPointId,
//                                    PreplotPointType = (int)prod.PreplotPointType,
//                                    PreplotVersionId = prod.PreplotVersionId,
//                                    QualityControlStatus = prod.QualityControlStatus,
//                                    WorkNumber = prod.WorkNumber
//                                });
//                        }
//                        pointProductionDal.SaveProductions(validProductions);
//                        if (seisRegisters.Any()) seisRegDal.SaveSeismicRegisters(seisRegisters);
//                    }
//                    else
//                        pointProductionDal.SaveProductions(validProductions);

//                    StretchBLL.SaveStretch(stretchDto, false, stretchDal);

//                    if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//                        || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//                        holeDal.AddHoles(holesToCreate);
//                    //-----------------------------------------------------------------------------
//                    pointProductionDal.CommitTransaction();
//                    holeDal.CommitTransaction();
//                    stretchDal.CommitTransaction();

//                    result.Data = new StretchDAL().GetSurveyProductionData(surveyId, "");
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                stretchDal.RollbackTransaction();
//                holeDal.RollbackTransaction();
//                pointProductionDal.RollbackTransaction();

//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< SaveMultipleStretchesProduction(int surveyId, int operationalFrontId,
//            List<List<PointProductionDTO>> productionsPerStretch, int frontGroupId, int frontGroupLeaderId)
//        {
            
//            var pointProductionDal = new PointProductionDAL();
//            var holeDal = new HoleDAL();
//            var stretchDal = new StretchDAL();
//            pointProductionDal.BeginTransaction();
//            holeDal.BeginTransaction();
//            stretchDal.BeginTransaction();
//            try
//            {
//                var operationalFront = new OperationalFrontDAL().GetOperationalFront(operationalFrontId);

//                var invalidProductions = new List<PointProductionDTO>();
//                var validProductions = new List<PointProductionDTO>();
//                var stretchesWithHoles = new List<StretchWithHolesToCreateAndDelete>();
//                foreach (var stretchProductions in productionsPerStretch)
//                {
//                    var holesOnStretchToDelete = new List<HoleDTO>();
//                    var holesOnStretchToCreate = new List<HoleDTO>();
//                    var invalidProductionsOnStretch = new List<PointProductionDTO>();
//                    var validProductionsOnStretch = new List<PointProductionDTO>();

//                    ValidateProduction(surveyId, stretchProductions, frontGroupId, frontGroupLeaderId,
//                        out validProductionsOnStretch, out invalidProductionsOnStretch, out holesOnStretchToDelete, out holesOnStretchToCreate,
//                        operationalFront);

//                    invalidProductions.AddRange(invalidProductionsOnStretch);
//                    validProductions.AddRange(validProductionsOnStretch);
//                    stretchesWithHoles.Add(new StretchWithHolesToCreateAndDelete()
//                    {
//                        HolesToCreate = holesOnStretchToCreate,
//                        HolesToDelete = holesOnStretchToDelete,
//                        Productions = stretchProductions
//                    });
//                }
//                if (invalidProductions.Any())
//                {
//                    invalidProductions.AddRange(validProductions);
//                    var productionsWithErrors = invalidProductions;
//                    result.Data = productionsWithErrors;
//                    result.ErrorCode = ErrorCode.PassToCallback;
//                    result.ErrorMessage =
//                        "A produção dos trechos selecionados não foi gravada. Existem alguns trechos ou pontos impossibilitados de serem salvos.";
//                }
//                else
//                {
//                    foreach (var stretchProductions in stretchesWithHoles)
//                    {
//                        var firstPointProductionDto = stretchProductions.Productions.ToList().FirstOrDefault();
//                        var stretchDto = new StretchDTO();
//                        if (firstPointProductionDto != null)
//                        {
//                            stretchDto.DateString = DateParser.DateToString(firstPointProductionDto.Date);
//                            stretchDto.FinalStation = stretchProductions.Productions.Max(m => m.StationNumber);
//                            stretchDto.InitialStation = stretchProductions.Productions.Min(m => m.StationNumber);
//                            stretchDto.FrontGroupId = frontGroupId;
//                            stretchDto.FrontGroupLeaderId = frontGroupLeaderId;
//                            stretchDto.Line = firstPointProductionDto.Line;
//                            stretchDto.OperationalFrontId = operationalFrontId;
//                            stretchDto.SurveyId = surveyId;
//                        }

//                        //TRATAR A AÇÂO COMO TRANSACTION, COM POSSIBILIDADE DE ROLLBACK EM CASO DE ERRO
//                        if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//                            holeDal.DeleteHoles(stretchProductions.HolesToDelete);

//                        pointProductionDal.SaveProductions(validProductions);
//                        StretchBLL.SaveStretch(stretchDto, false, stretchDal);

//                        if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//                            || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//                            holeDal.AddHoles(stretchProductions.HolesToCreate);
//                        //-----------------------------------------------------------------------------
//                    }
//                }
//                pointProductionDal.CommitTransaction();
//                holeDal.CommitTransaction();
//                stretchDal.CommitTransaction();
//                result.Data = true;
//                return result;
//            }
//            catch (Exception ex)
//            {
//                stretchDal.RollbackTransaction();
//                holeDal.RollbackTransaction();
//                pointProductionDal.RollbackTransaction();

//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< DeleteFrontGroupProduction(int workNumber, IList<StretchDTO> stretches)
//        {
            
//            try
//            {
//                if (stretches != null)
//                {
//                    var allStretchesProductions = new List<PointProductionDTO>();
//                    var productionDal = new PointProductionDAL();
//                    var firstStretch = stretches.FirstOrDefault();

//                    if (firstStretch != null)
//                    {
//                        var surveyId = firstStretch.SurveyId;
//                        var opFronId = firstStretch.OperationalFrontId;
//                        foreach (var stretch in stretches)
//                        {
//                            var stretchProductions = productionDal.ListStretchProductions(stretch.SurveyId,
//                                PreplotPointType.All, stretch.OperationalFrontId, stretch.Line,
//                                stretch.InitialStation,
//                                stretch.FinalStation, workNumber);

//                            allStretchesProductions.AddRange(stretchProductions);
//                        }
//                        result = DeleteProduction(surveyId, opFronId, allStretchesProductions, workNumber, productionDal, new HoleDAL());
//                        if (string.IsNullOrWhiteSpace(result.ErrorMessage))
//                        {
//                            foreach (var stretch in stretches)
//                                StretchBLL.DeleteStretch(stretch);
//                        }

//                    }
//                    else
//                        throw new Exception("Não existem trechos a serem excluídos.");
//                }
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        //public async Task< DeleteLandProduction(int surveyId, int operationalFrontId, int landId, string date)
//        //{
//        //    
//        //    try
//        //    {
//        //        var pointProductions = new PointProductionDAL().ListLandProductions(surveyId, operationalFrontId, landId,
//        //            date);

//        //        DeleteProduction(surveyId, operationalFrontId, pointProductions, 1);

//        //            result.Data = true;
//        //        }
//        //    catch (Exception ex)
//        //    {
//        //        result.ErrorMessage = ex.Message;
//        //        result.ErrorCode = ErrorCode.DalException;
//        //    }
//        //    return result;
//        //}

//        public async Task< DeleteStretchProduction(int workNumber, StretchDTO stretch)
//        {
            
//            try
//            {
//                var productionDal = new PointProductionDAL();
//                var pointProductions = productionDal.ListStretchProductions(stretch.SurveyId, PreplotPointType.All,
//                    stretch.OperationalFrontId, stretch.Line, stretch.InitialStation, stretch.FinalStation, workNumber);

//                result = DeleteProduction(stretch.SurveyId, stretch.OperationalFrontId, pointProductions, workNumber, productionDal, new HoleDAL());

//                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
//                    StretchBLL.DeleteStretch(stretch);
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< DeleteProduction(int surveyId, int operationalFrontId,
//            IEnumerable<PointProductionDTO> pointProductions, int workNumber, PointProductionDAL pointProductionDal, HoleDAL holeDal)
//        {
            
//            try
//            {
//                var invalidProductions = new List<PointProductionDTO>();
//                var validProductions = new List<PointProductionDTO>();
//                var holesToDelete = new List<HoleDTO>();
//                var opFrontType = new OperationalFrontDAL().GetOperationalFrontType(operationalFrontId);

//                if (pointProductions != null)
//                {
//                    foreach (var production in pointProductions)
//                    {
//                        var existingProduction = pointProductionDal.GetProduction(surveyId, production.PreplotPointId,
//                            production.PreplotVersionId,
//                            workNumber, production.PreplotPointType, operationalFrontId);
//                        if (existingProduction == null)
//                        {
//                            invalidProductions.Add(production);
//                            continue;
//                        }
//                        if (pointProductionDal.HasNextOperationalFrontsProductionsInFutureDates(production))
//                        {
//                            invalidProductions.Add(production);
//                            continue;
//                        }
//                        validProductions.Add(production);
//                        holesToDelete.AddRange(holeDal.ListHoles(production.SurveyId,
//                            production.PreplotPointId, production.PreplotVersionId,
//                            production.PreplotPointType, operationalFrontId));
//                    }
//                }
//                if (invalidProductions.Any())
//                {
//                    invalidProductions.AddRange(validProductions);
//                    var productionsWithErrors = invalidProductions;
//                    result.Data = productionsWithErrors;
//                    result.ErrorCode = ErrorCode.PassToCallback;
//                    result.ErrorMessage = "Nenhuma produção foi excluída. Existem alguns pontos com inconsistências.";
//                }
//                else
//                {
//                    if (holesToDelete.Any()) holeDal.DeleteHoles(holesToDelete);
//                    if (opFrontType == OperationalFrontType.Detonation) new SeismicRegisterDAL().DeleteSeismicRegisters(validProductions);
//                    foreach (var production in validProductions)
//                        pointProductionDal.DeleteProduction(production);

//                    result.Data = true;
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        #region Private Methods

//        //private async Task< SaveGenericProduction(int surveyId, int operationalFrontId,
//        //    IEnumerable<PointProductionDTO> pointProductions, int frontGroupId, int frontGroupLeaderId)
//        //{
//        //    
//        //    try
//        //    {
//        //        var pointProductionDal = new PointProductionDAL();
//        //        var holeDal = new HoleDAL();
//        //        List<PointProductionDTO> invalidProductions;
//        //        List<PointProductionDTO> validProductions;
//        //        List<HoleDTO> holesToDelete;
//        //        List<HoleDTO> holesToCreate;
//        //        var operationalFront = new OperationalFrontDAL().GetOperationalFront(operationalFrontId);

//        //        ValidateProduction(surveyId, pointProductions, frontGroupId, frontGroupLeaderId,
//        //            out validProductions, out invalidProductions, out holesToDelete, out holesToCreate, operationalFront);

//        //        if (invalidProductions.Any())
//        //        {
//        //            invalidProductions.AddRange(validProductions);
//        //            var productionsWithErrors = invalidProductions;
//        //            result.Data = productionsWithErrors;
//        //            result.ErrorCode = ErrorCode.PassToCallback;
//        //            result.ErrorMessage = "A produção do trecho selecionado não foi gravada. Existem alguns pontos impossibilitados de serem salvos.";
//        //        }
//        //        else
//        //        {
//        //            //TRATAR A AÇÂO COMO TRANSACTION, COM POSSIBILIDADE DE ROLLBACK EM CASO DE ERRO
//        //            if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//        //                holeDal.DeleteHoles(holesToDelete);

//        //            pointProductionDal.SaveProductions(validProductions);

//        //            StretchDTO stretchDTO = new StretchDTO();
//        //            PointProductionDTO  firstPointProductionDTO  =  pointProductions.ToList().FirstOrDefault();
//        //            if(firstPointProductionDTO !=null){
//        //                stretchDTO.DateString = DateParser.DateToString(firstPointProductionDTO.DateString);
//        //                stretchDTO.FinalStation = pointProductions.Max(m => m.StationNumber);
//        //                stretchDTO.InitialStation = pointProductions.Min(m => m.StationNumber);
//        //                stretchDTO.FinalStationBkp = pointProductions.Max(m => m.StationNumber);
//        //                stretchDTO.InitialStationBkp = pointProductions.Min(m => m.StationNumber);
//        //                stretchDTO.FrontGroupId = frontGroupId;
//        //                stretchDTO.FrontGroupLeaderId = frontGroupLeaderId;
//        //                stretchDTO.Line = firstPointProductionDTO.Line;
//        //                stretchDTO.OperationalFrontId = operationalFrontId;
//        //                stretchDTO.SurveyId = surveyId;                        

//        //            }
//        //            StretchBLL.SaveStretch(stretchDTO);


//        //            if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Charging
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
//        //                || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
//        //                holeDal.AddHoles(holesToCreate);
//        //            //-----------------------------------------------------------------------------

//        //            result.Data = true;
//        //        }
//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        result.ErrorMessage = ex.Message;
//        //        result.ErrorCode = ErrorCode.DalException;
//        //    }
//        //    return result;
//        //}

//        private static void ValidateProduction(int surveyId,
//            IEnumerable<PointProductionDTO> pointProductions,
//            int frontGroupId, int frontGroupLeaderId, out List<PointProductionDTO> validProductions,
//            out List<PointProductionDTO> invalidProductions, out List<HoleDTO> holesToDelete,
//            out List<HoleDTO> holesToCreate, OperationalFrontDTO operationalFront)
//        {
//            invalidProductions = new List<PointProductionDTO>();
//            validProductions = new List<PointProductionDTO>();
//            holesToDelete = new List<HoleDTO>();
//            holesToCreate = new List<HoleDTO>();
//            var pointProductionDal = new PointProductionDAL();
//            var holeDal = new HoleDAL();
//            var redRuleDal = new ReductionRuleDAL();
//            var previousOperationalFront = new OperationalFrontDAL().GetPreviousOperationalFront(operationalFront.OperationalFrontId);
//            var holesPerShotPoint = new SurveyDAL().GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);

//            foreach (var production in pointProductions)
//            {
//                //Validar se a produção já existe
//                var existingProduction = pointProductionDal.GetProduction(surveyId, production.PreplotPointId,
//                    production.PreplotVersionId, production.WorkNumber, production.PreplotPointType, operationalFront.OperationalFrontId);
//                production.SurveyId = surveyId;
//                production.OperationalFrontId = operationalFront.OperationalFrontId;
//                production.FrontGroupId = frontGroupId;
//                production.FrontGroupLeaderId = frontGroupLeaderId;
//                production.IsActive = true;

//                //Validar produção em ponto Liberado Somente ER
//                if (production.Status == (int)ProductionStatus.Accomplished &&
//                    production.PreplotPointType == PreplotPointType.ShotPoint &&
//                    pointProductionDal.HasProductionsWithOnlyStationsStatusInPastDates(production))
//                {
//                    production.SavingErrorAlert = "Produção não liberada pela Permissoria para Pontos de Tiro.";
//                    invalidProductions.Add(production);
//                    continue;
//                }
//                //Validar produção de Detonação sem FFID
//                if (production.Ffid != null && production.Ffid > 0 &&
//                    production.Status != (int)ProductionStatus.Accomplished)
//                {
//                    production.SavingErrorAlert = "Produção com FFID não pode ter situação 'Não Detonado'.";
//                    invalidProductions.Add(production);
//                    continue;
//                }
//                if (existingProduction != null && existingProduction.IsActive) //EDIÇÃO
//                {
//                    if (production.SavingDate.Date != existingProduction.Date.Date) //EDIÇÃO DE DATA
//                    {
//                        production.SavingErrorAlert =
//                                "A produção desta frente operacional para este ponto já existe na data de " + existingProduction.Date.Date.ToShortDateString() + ". Tente salvar a produção selecionando essa data.";
//                        invalidProductions.Add(production);
//                        continue;
//                        //if (production.WorkNumber == 1) //EDIÇÃO DE DATA DE TRABALHO
//                        //{
//                        //    var reworks = pointProductionDal.ListReworks(surveyId, production.PreplotPointId,
//                        //        production.PreplotVersionId, production.PreplotPointType, operationalFront.OperationalFrontId);

//                        //    if (reworks.Any(rework => production.DateString > rework.DateString))
//                        //    {
//                        //        production.SavingErrorAlert =
//                        //            "A data selecionada é maior que a data de um retrabalho deste ponto.";
//                        //        invalidProductions.Add(production);
//                        //        continue;
//                        //    }
//                        //    if (pointProductionDal.HasNextOperationalFrontsProductionsInPastDates(production))
//                        //    {
//                        //        production.SavingErrorAlert =
//                        //            "A produção da próxima frente operacional tem data menor que a data selecionada.";
//                        //        invalidProductions.Add(production);
//                        //        continue;
//                        //    }
//                        //    if (operationalFront.PreviousOperationalFrontId.HasValue)
//                        //    {
//                        //        if (pointProductionDal.HasPreviousOperationalFrontsProductionsInFutureDates(production, operationalFront.PreviousOperationalFrontId.Value))
//                        //        {
//                        //            production.SavingErrorAlert =
//                        //                "A produção da frente operacional anterior tem data maior que a data selecionada.";
//                        //            invalidProductions.Add(production);
//                        //            continue;
//                        //        }
//                        //    }
//                        //}
//                        //else //EDIÇÃO DE DATA DE RETRABALHO
//                        //{
//                        //    var firstWork = pointProductionDal.GetProduction(surveyId, production.PreplotPointId,
//                        //        production.PreplotVersionId, 1, production.PreplotPointType, operationalFront.OperationalFrontId);
//                        //    if (firstWork != null && production.DateString < firstWork.DateString)
//                        //    {
//                        //        production.SavingErrorAlert =
//                        //            "A data selecionada para o retrabalho é anterior à data da primeira produção.";
//                        //        invalidProductions.Add(production);
//                        //        continue;
//                        //    }
//                        //}
//                    }
//                    //SE NÃO É EDIÇÃO DE DATA, MAS É DE TURMA/LIDER
//                    if (existingProduction.FrontGroupId != frontGroupId || existingProduction.FrontGroupLeaderId != frontGroupLeaderId)
//                    {
//                        production.SavingErrorAlert = "A produção deste ponto já foi lançada para outra Turma.";
//                        invalidProductions.Add(production);
//                        continue;
//                    }
//                    //if (production.Status != existingProduction.Status || //EDIÇÃO DE STATUS OU DE DADOS DOS FUROS
//                    //    production.ReductionRuleId != existingProduction.ReductionRuleId)// ||
//                    ////!production.HolesDepth.Equals(existingProduction.HolesDepth))// ||
//                    ////production.ChargesType != existingProduction.ChargesType ||
//                    ////production.FusesType != existingProduction.FusesType) ||
//                    ////production.NumberOfFusesPerHole != existingProduction.NumberOfFusesPerHole ||
//                    ////production.NumberOfChargesPerHole != existingProduction.NumberOfChargesPerHole ||
//                    ////production.NumberOfFusesInShotPoint != existingProduction.NumberOfFusesInShotPoint ||
//                    ////production.NumberOfChargesInShotPoint != existingProduction.NumberOfChargesInShotPoint)
//                    //{
//                    //    if (pointProductionDal.HasNextOperationalFrontsProductionsInFutureDatesWithAccomplishedStatus(production))
//                    //    {
//                    //        production.SavingErrorAlert =
//                    //            "Existe produção para o ponto na próxima frente operacional. Não é possível editar o status ou dados dos furos.";
//                    //        invalidProductions.Add(production);
//                    //        continue;
//                    //    }
//                    //}
//                    //SELECIONA OS FUROS A EXCLUIR
//                    var existingHoles = holeDal.ListHoles(production.SurveyId,
//                        production.PreplotPointId, production.PreplotVersionId,
//                        production.PreplotPointType, operationalFront.OperationalFrontId);
//                    holesToDelete.AddRange(existingHoles);
//                }
//                else //INSERÇÃO
//                {
//                    if (operationalFront.PreviousOperationalFrontId.HasValue)
//                    {
//                        var previousPastProductions =
//                            pointProductionDal.ListPreviousOperationalFrontsProductionsInPastDates(production, operationalFront.PreviousOperationalFrontId.Value) ??
//                            new List<PointProductionDTO>();
//                        var pointProductionDtos = previousPastProductions as IList<PointProductionDTO> ?? previousPastProductions.ToList();
//                        if (operationalFront.OperationalFrontType != OperationalFrontType.Inspection)
//                        {
//                            if (!pointProductionDtos.Any())
//                            {
//                                production.SavingErrorAlert = "Não existe produção para o ponto, em uma data anterior, na frente predecessora.";
//                                invalidProductions.Add(production);
//                                continue;
//                            }
//                            if (pointProductionDtos.Any(prod => prod.Status == (int)ProductionStatus.Unaccomplished))
//                            {
//                                if (production.Status == (int)ProductionStatus.Accomplished)
//                                {
//                                    production.SavingErrorAlert = "A produção para o ponto na frente anterior tem status igual a " +
//                                                                  Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, previousOperationalFront.OperationalFrontType);
//                                    invalidProductions.Add(production);
//                                    continue;
//                                }
//                            }
//                        }
//                        //if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling)
//                        //{
//                        //    if (pointProductionDal.CountNextOperationalFrontsProductions(production) 
//                        //        < 
//                        //        pointProductionDal.CountProductionsAndReworks(surveyId,production.PreplotPointId, production.PreplotVersionId, production.PreplotPointType,operationalFront.OperationalFrontId))
//                        //    {
//                        //        production.SavingErrorAlert = "Já existem furos ainda não carregados para o PT.";
//                        //        invalidProductions.Add(production);
//                        //        continue;
//                        //    }
//                        //}
//                        //if (operationalFront.OperationalFrontType == OperationalFrontType.Charging ||
//                        //    operationalFront.OperationalFrontType == OperationalFrontType.Detonation)
//                        //{
//                        //    if (pointProductionDal.CountPreviousOperationalFrontsProductions(production, operationalFront.PreviousOperationalFrontId.Value) 
//                        //        <= 
//                        //        pointProductionDal.CountProductionsAndReworks(surveyId,production.PreplotPointId, production.PreplotVersionId, production.PreplotPointType,operationalFrontId))
//                        //    {
//                        //        production.SavingErrorAlert = operationalFront.OperationalFrontType == OperationalFrontType.Charging
//                        //            ? "Não existem furos a serem carregados neste PT."
//                        //            : "Não existem cargas a serem detonadas neste PT.";
//                        //        invalidProductions.Add(production);
//                        //        continue;
//                        //    }
//                        //}
//                    }
//                    production.WorkNumber = pointProductionDal.GetMaxWorkNumber(surveyId, production.PreplotPointId,
//                        production.PreplotVersionId, production.PreplotPointType, operationalFront.OperationalFrontId) + 1;
//                }

//                if (operationalFront.OperationalFrontType == OperationalFrontType.Gravimetry && !production.HasOasis)
//                {
//                    production.SavingErrorAlert = "O dado ainda não foi processado no Oasis montaj.";
//                    invalidProductions.Add(production);
//                    continue;
//                }

//                if (production.Status == (int)ProductionStatus.Accomplished)
//                {
//                    //SELECIONA OS FUROS A INSERIR
//                    var newHoles = InstantiateHoles(production, holesPerShotPoint, false, redRuleDal, operationalFront.OperationalFrontType);
//                    holesToCreate.AddRange(newHoles);
//                }
//                production.SavingErrorAlert = "OK";
//                validProductions.Add(production);
//            }
//        }

//        public static IEnumerable<HoleDTO> InstantiateHoles(PointProductionDTO production, int holesPerShotPoint, bool instantiateAbsentHoles, ReductionRuleDAL reductionRuleDal, OperationalFrontType frontType)
//        {
//            var holes = new List<HoleDTO>();

//            var finalHolesQuantity = holesPerShotPoint;
//            if (production.ReductionRuleId.HasValue)
//                finalHolesQuantity = reductionRuleDal.GetReductionFinalHolesQuantity(production.ReductionRuleId.Value);
//            if (production.Status != (int)ProductionStatus.Accomplished)
//                finalHolesQuantity = 0;

//            var centralPosition = holesPerShotPoint / 2;
//            var countLeft = 1;
//            var countRight = 1;

//            for (var i = 0; i < finalHolesQuantity; i++)
//            {
//                int holeNumber;
//                if (i == 0)
//                    holeNumber = centralPosition;
//                else
//                {
//                    if (i % 2 != 0)
//                    {
//                        holeNumber = centralPosition + countRight;
//                        countRight++;
//                    }
//                    else
//                    {
//                        holeNumber = centralPosition - countLeft;
//                        countLeft++;
//                    }
//                }
//                var newHole = new HoleDTO()
//                {
//                    Depth = production.HolesDepth,
//                    HoleNumber = holeNumber,
//                    IsActive = true,
//                    OperationalFrontId = production.OperationalFrontId,
//                    PreplotPointId = production.PreplotPointId,
//                    PreplotPointType = production.PreplotPointType,
//                    PreplotVersionId = production.PreplotVersionId,
//                    SurveyId = production.SurveyId,
//                    FrontGroupId = production.FrontGroupId,
//                    WorkNumber = production.WorkNumber,
//                    Date = production.Date,
//                };
//                if (frontType != OperationalFrontType.Drilling)
//                {
//                    newHole.ChargesTypeId = production.ChargesType;
//                    newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
//                    newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
//                    newHole.FusesTypeId = production.FusesType;
//                }
//                holes.Add(newHole);
//            }
//            if (instantiateAbsentHoles)
//            {
//                for (var i = finalHolesQuantity; i < holesPerShotPoint; i++)
//                {
//                    int holeNumber;
//                    if (i == 0)
//                        holeNumber = centralPosition;
//                    else
//                    {
//                        if (i % 2 != 0)
//                        {
//                            holeNumber = centralPosition + countRight;
//                            countRight++;
//                        }
//                        else
//                        {
//                            holeNumber = centralPosition - countLeft;
//                            countLeft++;
//                        }
//                    }
//                    holes.Add(new HoleDTO { HoleNumber = holeNumber });
//                }
//            }
//            return holes.OrderBy(h => h.HoleNumber).ToList();
//        }

//        public static IEnumerable<HoleDTO> ListHoles(PointProductionDTO production, int holesPerShotPoint, ReductionRuleDAL reductionRuleDal, OperationalFrontType frontType)
//        {
//            var holes = new List<HoleDTO>();

//            var finalHolesQuantity = holesPerShotPoint;
//            if (production.ReductionRuleId.HasValue)
//                finalHolesQuantity = reductionRuleDal.GetReductionFinalHolesQuantity(production.ReductionRuleId.Value);
//            if (production.Status != (int)ProductionStatus.Accomplished)
//                finalHolesQuantity = 0;

//            var centralPosition = holesPerShotPoint / 2;
//            var countLeft = 1;
//            var countRight = 1;
//            if (finalHolesQuantity != 2)
//            {
//                for (var i = 0; i < finalHolesQuantity; i++)
//                {
//                    int holeNumber;
//                    if (i == 0)
//                        holeNumber = centralPosition;
//                    else
//                    {
//                        if (i % 2 != 0)
//                        {
//                            holeNumber = centralPosition + countRight;
//                            countRight++;
//                        }
//                        else
//                        {
//                            holeNumber = centralPosition - countLeft;
//                            countLeft++;
//                        }
//                    }
//                    var newHole = new HoleDTO()
//                    {
//                        Depth = production.HolesDepth,
//                        HoleNumber = holeNumber,
//                        IsActive = true,
//                        OperationalFrontId = production.OperationalFrontId,
//                        PreplotPointId = production.PreplotPointId,
//                        PreplotPointType = production.PreplotPointType,
//                        PreplotVersionId = production.PreplotVersionId,
//                        SurveyId = production.SurveyId,
//                        FrontGroupId = production.FrontGroupId,
//                        WorkNumber = production.WorkNumber,
//                        Date = production.Date,
//                    };
//                    if (frontType != OperationalFrontType.Drilling)
//                    {
//                        newHole.ChargesTypeId = production.ChargesType;
//                        newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
//                        newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
//                        newHole.FusesTypeId = production.FusesType;
//                    }
//                    holes.Add(newHole);
//                }
//                for (var i = finalHolesQuantity; i < holesPerShotPoint; i++)
//                {
//                    int holeNumber;
//                    if (i == 0)
//                        holeNumber = centralPosition;
//                    else
//                    {
//                        if (i % 2 != 0)
//                        {
//                            holeNumber = centralPosition + countRight;
//                            countRight++;
//                        }
//                        else
//                        {
//                            holeNumber = centralPosition - countLeft;
//                            countLeft++;
//                        }
//                    }
//                    holes.Add(new HoleDTO { HoleNumber = holeNumber });
//                }
//            }
//            else
//            {
//                var newHole = new HoleDTO()
//                {
//                    Depth = production.HolesDepth,
//                    HoleNumber = 0,
//                    IsActive = true,
//                    OperationalFrontId = production.OperationalFrontId,
//                    PreplotPointId = production.PreplotPointId,
//                    PreplotPointType = production.PreplotPointType,
//                    PreplotVersionId = production.PreplotVersionId,
//                    SurveyId = production.SurveyId,
//                    FrontGroupId = production.FrontGroupId,
//                    WorkNumber = production.WorkNumber,
//                    Date = production.Date,
//                };
//                if (frontType != OperationalFrontType.Drilling)
//                {
//                    newHole.ChargesTypeId = production.ChargesType;
//                    newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
//                    newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
//                    newHole.FusesTypeId = production.FusesType;
//                }
//                holes.Add(newHole);
//                holes.Add(new HoleDTO { HoleNumber = 1 });
//                var newHole2 = new HoleDTO()
//                {
//                    Depth = production.HolesDepth,
//                    HoleNumber = 2,
//                    IsActive = true,
//                    OperationalFrontId = production.OperationalFrontId,
//                    PreplotPointId = production.PreplotPointId,
//                    PreplotPointType = production.PreplotPointType,
//                    PreplotVersionId = production.PreplotVersionId,
//                    FrontGroupId = production.FrontGroupId,
//                    SurveyId = production.SurveyId,
//                    WorkNumber = production.WorkNumber,
//                    Date = production.Date,
//                };
//                if (frontType != OperationalFrontType.Drilling)
//                {
//                    newHole2.ChargesTypeId = production.ChargesType;
//                    newHole2.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
//                    newHole2.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
//                    newHole2.FusesTypeId = production.FusesType;
//                }
//                holes.Add(newHole2);
//            }
//            return holes.OrderBy(h => h.HoleNumber).ToList();
//        }

//        //private static List<PointProductionDTO> BuildProductions(int surveyId, int operationalFrontId, string date,
//        //    LandDTO land, int frontGroupId, int frontGroupLeaderId)
//        //{
//        //    var productions = new List<PointProductionDTO>();
//        //    if (land == null) return productions;
//        //    var points = new PreplotPointDAL().ListPreplotPoints(surveyId, land.LandId);
//        //    if (points != null)
//        //    {
//        //        productions.AddRange(points.Select(point => new PointProductionDTO()
//        //        {
//        //            DateString = DateParser.StringToDate(date), 
//        //            FrontGroupId = frontGroupId, 
//        //            FrontGroupLeaderId = frontGroupLeaderId, 
//        //            IsActive = true, 
//        //            OperationalFrontId = operationalFrontId, 
//        //            PreplotPointId = point.PreplotPointId, 
//        //            PreplotPointType = point.PreplotPointType, 
//        //            PreplotVersionId = point.PreplotVersionId, 
//        //            SurveyId = surveyId, 
//        //            WorkNumber = 1
//        //        }));
//        //    }
//        //    return productions;
//        //}

//        #endregion
//        //private async Task< SavePermitProduction(int surveyId, int operationalFrontId,
//        //    IEnumerable<PointProductionDTO> pointProductions, int frontGroupId, int frontGroupLeaderId)
//        //{
//        //    
//        //    try
//        //    {
//        //        var validProductions = new List<PointProductionDTO>();
//        //        var invalidProductions = new List<PointProductionDTO>();
//        //        var pointProductionDal = new PointProductionDAL();

//        //        foreach (var production in pointProductions)
//        //        {
//        //            production.SurveyId = surveyId;
//        //            production.OperationalFrontId = operationalFrontId;
//        //            production.FrontGroupId = frontGroupId;
//        //            production.FrontGroupLeaderId = frontGroupLeaderId;
//        //            production.IsActive = true;
//        //            production.WorkNumber = 1;

//        //            //Validar se a produção já existe
//        //            var existingProduction = pointProductionDal.GetProduction(surveyId, production.PreplotPointId,
//        //                production.PreplotVersionId, 1, production.PreplotPointType, operationalFrontId);
//        //            //Se já existe a produção deste ponto
//        //            if (existingProduction != null)
//        //            {
//        //                //Validar se há mudança de status
//        //                if (existingProduction.Status != production.Status)
//        //                {
//        //                    //Validar se existe produção das frentes posteriores, em datas posteriores, para o mesmo ponto
//        //                    if (pointProductionDal.HasNextOperationalFrontsProductionsInFutureDates(production))
//        //                    {
//        //                        production.SavingErrorAlert = "Já existe produção da próxima frente operacional para este ponto.";
//        //                        invalidProductions.Add(production);
//        //                        continue;
//        //                    }
//        //                }
//        //                //Validar se há mudança de data
//        //                if (existingProduction.DateString != production.DateString)
//        //                {
//        //                    //Validar se existe produção das frentes posteriores, em datas anteriores, para o mesmo ponto
//        //                    if (pointProductionDal.HasNextOperationalFrontsProductionsInPastDates(production))
//        //                    {
//        //                        production.SavingErrorAlert = "A produção da próxima frente operacional tem data anterior à data selecionada.";
//        //                        invalidProductions.Add(production);
//        //                        continue;
//        //                    }
//        //                }
//        //            }
//        //            validProductions.Add(production);
//        //        }
//        //        if (invalidProductions.Any())
//        //        {
//        //            result.Data = invalidProductions;
//        //            result.ErrorCode = ErrorCode.PassToCallback;
//        //            result.ErrorMessage = "A produção não foi gravada. Existem alguns pontos nos trechos selecionados que não podem ser salvos.";
//        //        }
//        //        else
//        //        {
//        //            pointProductionDal.SaveProductions(validProductions);
//        //            result.Data = true;
//        //        }
//        //        return result;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        result.ErrorMessage = ex.Message;
//        //        result.ErrorCode = ErrorCode.DalException;
//        //    }
//        //    return result;
//        //}

//        //private static List<FrontGroupProductionDTO> ListPermitGroupsProductionsPrivate(int surveyId, int operationalFrontId, DateTime date )
//        //{
//        //    var groupedDailyStretchesByGroup = new PointProductionDAL().ListDailyLandStretchesProductions(surveyId,
//        //        operationalFrontId, date).GroupBy(m => new {m.FrontGroupId, m.FrontGroupLeaderId});
//        //    var stretchGroups = new List<FrontGroupProductionDTO>();
//        //    var groupIndex = 0;

//        //    foreach (var frontGroupStretches in groupedDailyStretchesByGroup)
//        //    {
//        //        var stretchesGroupedByLand = frontGroupStretches.GroupBy(m => m.LandId);
//        //        var newGroup = InstantiateFrontGroupProduction(date, frontGroupStretches.Key.FrontGroupId, frontGroupStretches.Key.FrontGroupLeaderId, groupIndex++);
//        //        foreach (var stretches in stretchesGroupedByLand)
//        //        {
//        //            var newStretches = stretches.Select(m => m).ToList();
//        //            var firstStretch = stretches.FirstOrDefault();
//        //            if (firstStretch != null)
//        //            {
//        //                var newLand = new LandDTO()
//        //                {
//        //                    LandId = firstStretch.LandId,
//        //                    Name = string.IsNullOrWhiteSpace(firstStretch.LandName) ? "Área Devoluta" : firstStretch.LandName,
//        //                    PermitStatus = firstStretch.PermitStatus
//        //                };
//        //                newGroup.ProductionLands.Add(newLand);
//        //            }
//        //            newGroup.Stretches.AddRange(newStretches);
//        //            if (newGroup.Stretches.Any())
//        //                newGroup.LastStrechIndex = newGroup.Stretches.Select(m => m.Index).Max();
//        //        }
//        //        stretchGroups.Add(newGroup);
//        //    }
//        //    return stretchGroups;
//        //}

//        //private static List<FrontGroupProductionDTO> ListFrontGroupsProductionsPrivate(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, DateTime date)
//        //{
//        //    var preplotPointDal = new PreplotPointDAL();
//        //    var groupedDailyStretches = new PointProductionDAL().ListDailyStretchesProductions(surveyId, preplotPointType, operationalFrontId, date).GroupBy(m => new { m.FrontGroupId, m.FrontGroupLeaderId });
//        //    var stretchGroups = new List<FrontGroupProductionDTO>();
//        //    var groupIndex = 0;

//        //    foreach (var frontGroupStretches in groupedDailyStretches)
//        //    {
//        //        var newGroup = InstantiateFrontGroupProduction(date, frontGroupStretches.Key.FrontGroupId, frontGroupStretches.Key.FrontGroupLeaderId, groupIndex++);
//        //        var newStretches = frontGroupStretches.Select(m => m).ToList();
//        //        foreach (var stretch in newStretches)
//        //        {
//        //            stretch.Points = preplotPointDal.ListStationNumbers(surveyId, preplotPointType, stretch.Line,
//        //                stretch.InitialStation, stretch.FinalStation);
//        //        }
//        //        newGroup.Stretches.AddRange(newStretches);
//        //        if (newGroup.Stretches.Any())
//        //            newGroup.LastStrechIndex = newGroup.Stretches.Select(m => m.Index).Max();
//        //        stretchGroups.Add(newGroup);
//        //    }
//        //    return stretchGroups;
//        //}
//    }
//}
