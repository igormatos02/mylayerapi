using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.interfaces.services.Report;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class PointProductionService : IPointProductionService
    {
        private readonly IStretchService _stretchService;
        private readonly IDisplacementRuleRepository _displacementRuleRepository;
        private readonly ISwathRepository _swathRepository;
        private readonly IFrontGroupLeaderRepository _frontGroupLeaderRepository;
        private readonly IFrontGroupRepository _frontGroupRepository;
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly IHoleRepository _holeRepository;
        private readonly IReductionRuleRepository _reductionRuleRepository;
        private readonly ISeismicRegisterRepository _seismicRegisterRepository;
        private readonly IBruteRobImportHelperService _bruteRobImportHelperService;
        private readonly IPointProductionRepository _pointProductionRepository;
        private readonly IStretchRepository _stretchRepository;
        private readonly IOperationalFrontRepository _operationalFrontRepository;
        private readonly IConfiguration _configuration;

        public PointProductionService(
            IStretchService stretchService,
            IDisplacementRuleRepository displacementRuleRepository,
            ISwathRepository swathRepository,
            IFrontGroupLeaderRepository frontGroupLeaderRepository,
            IFrontGroupRepository frontGroupRepository,
            IPreplotPointRepository preplotPointRepository,
            ISurveyRepository surveyRepository,
            IHoleRepository holeRepository,
            IReductionRuleRepository reductionRuleRepository,
            ISeismicRegisterRepository seismicRegisterRepository,
            IBruteRobImportHelperService bruteRobImportHelperService,
            IOperationalFrontRepository operationalFrontRepository,
            IStretchRepository stretchRepository,
            IPointProductionRepository pointProductionRepository,
            IConfiguration configuration)
        {
            _stretchService = stretchService;
            _displacementRuleRepository = displacementRuleRepository;
            _swathRepository = swathRepository;
            _frontGroupLeaderRepository = frontGroupLeaderRepository;
            _frontGroupRepository = frontGroupRepository;
            _preplotPointRepository = preplotPointRepository;
            _surveyRepository = surveyRepository;
            _holeRepository = holeRepository;
            _reductionRuleRepository = reductionRuleRepository;
            _seismicRegisterRepository = seismicRegisterRepository;
            _bruteRobImportHelperService = bruteRobImportHelperService;
            _operationalFrontRepository = operationalFrontRepository;
            _stretchRepository = stretchRepository;
            _pointProductionRepository = pointProductionRepository;
            _configuration = configuration;
        }

        public async Task<List<TotalDailyProductionModel>> ListDailyProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal? point, bool hasUnaccomplished)
        {

            try
            {
                var totalDailyProductionModel = await _stretchRepository.ListTotalDailyProductions(surveyId, operationalFrontId, line, point, hasUnaccomplished);//new  await _pointProductionRepository().ListDailyProductions(surveyId, preplotPointType, operationalFrontId);

                var operationalFront = await _operationalFrontRepository.GetOperationalFront(operationalFrontId);
                if (operationalFront != null && operationalFront.OperationalFrontType == OperationalFrontType.Charging)
                {

                    /* foreach (TotalDailyProductionModel t in totalDailyProductionModel)
                     {

                         foreach (StretchModel s in t.Stretches)
                         {
                             var numberOfChargesInShotPoint =  await _pointProductionRepository.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfChargesInShotPoint);
                             var numberOfFusesInShotPoint =  await _pointProductionRepository.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1).Sum(m => m.NumberOfFusesInShotPoint);
                             t.NumberOfChargesInShotPoint += numberOfChargesInShotPoint.Value;
                             t.NumberOfFusesInShotPoint += numberOfFusesInShotPoint.Value;
                         }                                               
                     }*/

                }


                return totalDailyProductionModel;
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<List<StretchModel>> ListDailyStretches(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date)
        {
            try
            {
                var stretches = await _stretchRepository.ListStretches(surveyId, operationalFrontId, date);

                var operationalFront = await _operationalFrontRepository.GetOperationalFront(operationalFrontId);
                if (operationalFront != null && operationalFront.OperationalFrontType == OperationalFrontType.Charging)
                {
                    foreach (var s in stretches)
                    {
                        var s1 = await _pointProductionRepository.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1);
                        var s2 = await _pointProductionRepository.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, s.Line, s.InitialStation, s.FinalStation, 1);
                        var numberOfChargesInShotPoint =s1.Sum(m => m.NumberOfChargesInShotPoint);
                        var numberOfFusesInShotPoint = s2.Sum(m => m.NumberOfFusesInShotPoint);
                        s.NumberOfChargesInShotPoint += numberOfChargesInShotPoint.Value;
                        s.NumberOfFusesInShotPoint += numberOfFusesInShotPoint.Value;
                    }

                }
                return stretches;

            }
            catch (Exception ex) { throw ex; }
        }

        //    public async Task<bool> ImportBruteRobFile(string fileName, byte[] byteStream, BruteRobParametersModel bruteRobParameters)
        //    {

        //    try
        //    {
        //        if (!_bruteRobImportHelperService.HasValidExtension(fileName)) throw new FileLoadException("Tipo de arquivo inválido");

        //        if (bruteRobParameters == null || bruteRobParameters.OperationalFrontId == 0 || bruteRobParameters.SurveyId == 0 ||
        //            bruteRobParameters.FrontGroupId == 0 || bruteRobParameters.FrontGroupLeaderId == 0)
        //            throw new Exception("Favor selecionar a turma e o líder da turma");

        //        var consideringPoints =await _pointProductionRepository.ListPreplotPointsWithoutCoordinates(bruteRobParameters.SurveyId,
        //                PreplotPointType.ShotPoint, bruteRobParameters.LineStretches).ToList();

        //        Stream stream = new MemoryStream(byteStream);
        //        var registersAndProductions = BruteRobImportHelperBLL.ExtractSeismicRegistersAndProductions(stream, bruteRobParameters, consideringPoints);
        //        stream.Flush();

        //        var seisRegisters = registersAndProductions.Where(m => m.Key != null).Select(m => m.Key).ToList();
        //        var repeatingFfids = seisRegisters.GroupBy(r => r.Ffid).SelectMany(grp => grp.Skip(1)).Select(m => m.Ffid).ToList(); //no proprio arquivo
        //        repeatingFfids.AddRange(new SeismicRegisterDAL().ListExistingFfids(seisRegisters.Select(m => m.Ffid).ToList()));//no banco
        //        if (repeatingFfids.Any())
        //        {
        //            var errorMsg = "Existem FFID's repetidos no arquivo importado:" + Environment.NewLine;
        //            errorMsg = repeatingFfids.Aggregate(errorMsg, (current, ffid) => current + ffid + Environment.NewLine);
        //            throw new Exception(errorMsg);
        //        }

        //        var productions = registersAndProductions.Where(m => m.Value != null).Select(m => m.Value).ToList();
        //        result.Data = new
        //        {
        //            Productions = productions,
        //            SeisRegisters = seisRegisters
        //        };
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        public async Task<bool> SaveSeismicRegistersWithProductions(int surveyId, int operationalFrontId, IList<SeismicRegisterModel> registers, int frontGroupId, int frontGroupLeaderId)
        {

            try
            {
                var productions = registers.Select(m => m.PointProduction).ToList();
                var result = await SaveProduction(surveyId, operationalFrontId, productions, frontGroupId, frontGroupLeaderId, 0, 0, true, "");
               // if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                 //   throw new Exception("Erro ao Salvar as Produções.");
                await _seismicRegisterRepository.SaveSeismicRegisters(registers);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<PointProductionModel>> SaveProductionFromBruteRobFile(List<PointProductionModel> productions, IEnumerable<SeismicRegisterModel> seisRegisters, BruteRobParametersModel bruteRobParameters)
        {

            try
            {
                if (bruteRobParameters == null || bruteRobParameters.OperationalFrontId == 0 || bruteRobParameters.SurveyId == 0 ||
                    bruteRobParameters.FrontGroupId == 0 || bruteRobParameters.FrontGroupLeaderId == 0)
                    throw new Exception("Favor selecionar a turma e o líder da turma");

                var stretchesProductions = bruteRobParameters.LineStretches.
                    Select(stretch => new List<PointProductionModel>(
                        productions.Where(p => p.Line == stretch.Line &&
                            p.StationNumber >= stretch.InitialStation &&
                            p.StationNumber <= stretch.FinalStation).ToList())).ToList();

                //SALVAR POR TRECHO
                var savingProductionResult = await SaveMultipleStretchesProduction(bruteRobParameters.SurveyId,
                    bruteRobParameters.OperationalFrontId, stretchesProductions,
                    bruteRobParameters.FrontGroupId, bruteRobParameters.FrontGroupLeaderId);

                await _seismicRegisterRepository.SaveSeismicRegisters(seisRegisters);
                return savingProductionResult;

             
              
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<RobScreenDetailsModel> GetImportRobScreenDetails(int operationalFrontId)
        {

            try
            {
                return new RobScreenDetailsModel
                {
                    Leaders = await _frontGroupLeaderRepository.ListFrontGroupLeaders(operationalFrontId),
                    Groups = await _frontGroupRepository.ListFrontGroups(operationalFrontId)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProductionScreenDetailsModel> GetProductionScreenDetails(int surveyId, int preplotPointType, int operationalFrontId, string date)
        {

            try
            {
                var operationalFrontType = await _operationalFrontRepository.GetOperationalFront(operationalFrontId);
                var stretchGroups = //operationalFrontType == OperationalFrontType.Permit
                                    //? ListPermitGroupsProductionsPrivate(surveyId, operationalFrontId, DateHelper.StringToDate(date)) :
                    await _stretchService.ListStretchGroups(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, date);
                //ListFrontGroupsProductionsPrivate(surveyId, (PreplotPointType)preplotPointType,operationalFrontId, DateHelper.StringToDate(date));
                var displacementRules = await _displacementRuleRepository.ListDisplacementRules(surveyId, true, (int)DisplacementRuleType.Seismic);
               return new ProductionScreenDetailsModel
                {
                    Leaders = await _frontGroupLeaderRepository.ListFrontGroupLeaders(operationalFrontId),
                    Groups = await _frontGroupRepository.ListFrontGroups(operationalFrontId),
                    Lines = await _preplotPointRepository.ListLines(surveyId, (PreplotPointType)preplotPointType),
                    StatusList = operationalFrontType.OperationalFrontType == OperationalFrontType.Permit ?
                        new List<GenericValueModel>
                        {
                            new GenericValueModel { Value = (int)ProductionStatus.Unaccomplished, Description = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, operationalFrontType.OperationalFrontType) },
                            new GenericValueModel { Value = (int)ProductionStatus.Accomplished, Description = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, operationalFrontType.OperationalFrontType) },
                            new GenericValueModel { Value = (int)ProductionStatus.OnlyReceptorStations, Description = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.OnlyReceptorStations, operationalFrontType.OperationalFrontType) }
                        }
                        : new List<GenericValueModel>
                        {
                            new GenericValueModel { Value = (int)ProductionStatus.Unaccomplished, Description = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, operationalFrontType.OperationalFrontType) },
                            new GenericValueModel { Value = (int)ProductionStatus.Accomplished, Description = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, operationalFrontType.OperationalFrontType) }
                        },
                    QcStatusList = operationalFrontType.OperationalFrontType == OperationalFrontType.Detonation ?
                        EnumHelper.ListEnumObjectsToScreen(typeof(QualityControlStatus))
                        : null,
                    DisplacementRules =displacementRules.Where(d => d.IsActive == true).ToList(),
                    ReductionRules = await _reductionRuleRepository.ListReductionRules(surveyId, true),
                    StretchGroups = stretchGroups
                    //SurveyLands = operationalFrontType == OperationalFrontType.Permit ? LandBLL.ListLands(surveyId, false) : new List<LandModel>()
                };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FieldReportDataModel> GetFieldProductionReportData(int surveyId, int preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation, int itemsPerPage)
        {

            try
            {
             
                var frontType = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);
                var survey = await _surveyRepository.GetSurvey(surveyId);
                int? previousStationsOpFrontId = null;
                var previousOpFrontId = await _operationalFrontRepository.GetPreviousOperationalFrontId(operationalFrontId);
                List<PointProductionModel> productionsList;
                if (!previousOpFrontId.HasValue)
                    throw new Exception("Não existe uma frente operacional anterior à selecionada.");
                if (frontType == OperationalFrontType.Inspection && preplotPointType == (int)PreplotPointType.All)
                {
                    var topoFronts = await _operationalFrontRepository.ListSurveyOperationalFronts(surveyId, (int)OperationalFrontType.Topography);
                    if (topoFronts.Any()) previousStationsOpFrontId = topoFronts.First().OperationalFrontId;
                    if (!previousStationsOpFrontId.HasValue)
                        throw new Exception("Não existe uma frente de Topografia anterior à frente selecionada.");
                    var _productionsList = await
                        ListStretchPoints(surveyId, previousOpFrontId.Value, previousStationsOpFrontId.Value, line,
                            initialStation, finalStation);
                    productionsList = _productionsList.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
                }
                else { 
                    var _productionsList =
                        await ListStretchPoints(surveyId, (PreplotPointType)preplotPointType, previousOpFrontId.Value, line,
                            initialStation, finalStation);
                   productionsList = _productionsList.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
                }
                var reportData = new FieldReportDataModel
                {
                    HolesArrangementDescription = survey.HolesArrangementDescription,
                    HolesArrangementImagePath = survey.HolesArrangementImagePath,
                    HolesQuantity = survey.HolesPerShotPoint ?? 0,
                    Line = line,
                    PointProductions = productionsList.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / itemsPerPage).Select(x => x.Select(v => v.Value).ToList()).ToList()
                };
                return reportData;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FieldReportDataModel> GetFieldProductionReportData(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber, int itemsPerPage)
        {

            try
            {
                var previousOpFrontId = await _operationalFrontRepository.GetPreviousOperationalFrontId(operationalFrontId);
                if (!previousOpFrontId.HasValue) throw new Exception("Não existe uma frente operacional anterior à selecionada.");
                var survey =  await _surveyRepository.GetSurvey(surveyId);
                var _productionsList = await ListStretchPoints(surveyId, preplotPointType, previousOpFrontId.Value, swathNumber);
                var productionsList = _productionsList.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList(); ;
                var reportData = new FieldReportDataModel
                {
                    HolesArrangementDescription = survey.HolesArrangementDescription,
                    HolesArrangementImagePath = survey.HolesArrangementImagePath,
                    HolesQuantity = survey.HolesPerShotPoint ?? 0,
                    PointProductions = productionsList.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / itemsPerPage).Select(x => x.Select(v => v.Value).ToList()).ToList()
                };
               return reportData;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
        {

            try
            {
                if (initialStation > finalStation)
                {
                    var bkp = initialStation;
                    initialStation = finalStation;
                    finalStation = bkp;
                }
                var seismicRegisters = await _seismicRegisterRepository.ListSeismicRegisters(surveyId, operationalFrontId, line, initialStation, finalStation);
               return seismicRegisters.OrderBy(m => m.Ffid).ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, int swathNumber)
        {

            try
            {
                var swath = await _swathRepository.GetSwath(surveyId, swathNumber);
                var swathStretches = await _preplotPointRepository.ListStretchesFromSwath(surveyId, PreplotPointType.ShotPoint,
                    swath.InitialShotPoint, swath.FinalShotPoint);
                var seismicRegisters = await _seismicRegisterRepository.ListSeismicRegisters(surveyId, operationalFrontId, swathStretches);
                return seismicRegisters.OrderBy(m => m.Ffid).ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
        {
            if (initialStation > finalStation)
            {
                var bkp = initialStation;
                initialStation = finalStation;
                finalStation = bkp;
            }
           
            //BUSCA DADOS GERAIS DOS PONTOS
            var surveyHolesQuantity = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
            var surveyHolesDepth = await _surveyRepository.GetSurveyDefaultHolesDepth(surveyId);
          
            //BUSCA PRODUÇõES E PONTOS DO TRECHO
            var productions =
                 await _pointProductionRepository.ListStretchLastWorks(surveyId, preplotPointType,
                    operationalFrontId, line, initialStation, finalStation);
            var points = await _preplotPointRepository.ListPreplotPointsWithoutCoordinates(surveyId, preplotPointType, line,
                    initialStation, finalStation);
            var index = 1;
            var opFrontType = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);

            foreach (var point in points)
            {
                var previousProductionObs = "";
                previousProductionObs =
                         await _pointProductionRepository.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
                            point.PreplotVersionId, point.PreplotPointType, operationalFrontId);

                var pointProduction = productions.FirstOrDefault(sp =>
                    sp.PreplotPointId == point.PreplotPointId &&
                    sp.PreplotVersionId == point.PreplotVersionId &&
                    sp.PreplotPointType == point.PreplotPointType);

                if (pointProduction == null)
                {
                    var production = new PointProductionModel
                    {
                        Index = index,
                        IsFromDb = false,
                        Line = line,
                        PreplotPointType = point.PreplotPointType,
                        StationNumber = point.StationNumber,
                        PreplotPointId = point.PreplotPointId,
                        PreplotVersionId = point.PreplotVersionId,
                        SurveyDefaultHolesQuantity = surveyHolesQuantity.Value,
                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
                        WorkNumber = 0,
                        HolesDepth = 0,
                        PreviousFrontProductionObservation = previousProductionObs
                    };
                    production.Holes = await ListHoles(production, surveyHolesQuantity.Value, opFrontType);
                    productions.Add(production);
                }
                else
                {
                    pointProduction.Index = index;
                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity.Value;
                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
                    //POINT INFO------------------------
                    pointProduction.Line = point.LineName;
                    pointProduction.StationNumber = point.StationNumber;
                    //HOLES INFO------------------------
                    pointProduction.Holes = await ListHoles(pointProduction, surveyHolesQuantity.Value, opFrontType);
                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;
                }
                index++;
            }
            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
        }

        public async Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, int shotPointsOpFrontId, int stationsOpFrontId, string line, decimal initialStation, decimal finalStation)
        {
            if (initialStation > finalStation)
            {
                var bkp = initialStation;
                initialStation = finalStation;
                finalStation = bkp;
            }
         
            //BUSCA DADOS GERAIS DOS PONTOS
            var surveyHolesQuantity = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
            var surveyHolesDepth = await _surveyRepository.GetSurveyDefaultHolesDepth(surveyId);

            //BUSCA PRODUÇõES E PONTOS DO TRECHO
            var shotPointProductions =
                await _pointProductionRepository.ListStretchLastWorks(surveyId, PreplotPointType.ShotPoint,
                    shotPointsOpFrontId, line, initialStation, finalStation);
            var stationsProductions =
                 await _pointProductionRepository.ListStretchLastWorks(surveyId, PreplotPointType.ReceiverStation,
                    stationsOpFrontId, line, initialStation, finalStation);
            var points =
                await _preplotPointRepository.ListPreplotPointsWithoutCoordinates(surveyId, PreplotPointType.All, line,
                    initialStation, finalStation);
            var index = 1;
            var shotPointOpFrontType = await _operationalFrontRepository.GetOperationalFrontType(shotPointsOpFrontId);
            var productions = new List<PointProductionModel>();
            foreach (var point in points)
            {
                var previousProductionObs = "";
                previousProductionObs =
                         await _pointProductionRepository.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
                            point.PreplotVersionId, point.PreplotPointType, shotPointsOpFrontId);

                var pointProduction =
                    shotPointProductions.FirstOrDefault(sp =>
                        sp.PreplotPointId == point.PreplotPointId &&
                        sp.PreplotVersionId == point.PreplotVersionId &&
                        sp.PreplotPointType == point.PreplotPointType)
                        ??
                    stationsProductions.FirstOrDefault(sp =>
                        sp.PreplotPointId == point.PreplotPointId &&
                        sp.PreplotVersionId == point.PreplotVersionId &&
                        sp.PreplotPointType == point.PreplotPointType);
                if (pointProduction == null)
                {
                    var builtProduction = new PointProductionModel
                    {
                        Index = index,
                        IsFromDb = false,
                        Line = line,
                        PreplotPointType = point.PreplotPointType,
                        StationNumber = point.StationNumber,
                        PreplotPointId = point.PreplotPointId,
                        PreplotVersionId = point.PreplotVersionId,
                        SurveyDefaultHolesQuantity = surveyHolesQuantity.Value,
                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
                        WorkNumber = 0,
                        HolesDepth = 0,
                        PreviousFrontProductionObservation = previousProductionObs
                    };
                    //if (point.PreplotPointType == PreplotPointType.ShotPoint)
                    builtProduction.Holes = await ListHoles(builtProduction, surveyHolesQuantity.Value, shotPointOpFrontType);
                    productions.Add(builtProduction);
                }
                else
                {
                    pointProduction.Index = index;
                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity.Value;
                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
                    //POINT INFO------------------------
                    pointProduction.Line = point.LineName;
                    pointProduction.StationNumber = point.StationNumber;
                    //HOLES INFO------------------------
                    //if (point.PreplotPointType == PreplotPointType.ShotPoint)
                    pointProduction.Holes = await ListHoles(pointProduction, surveyHolesQuantity.Value, shotPointOpFrontType);
                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;

                    productions.Add(pointProduction);
                }

                index++;
            }
            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
        }

        public async Task<IList<PointProductionModel>> ListStretchPoints(int surveyId, int preplotPointType, int operationalFrontId, int swathNumber)
        {
          
            //BUSCA DADOS GERAIS DOS PONTOS
            var surveyHolesQuantity = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
            var surveyHolesDepth = await _surveyRepository.GetSurveyDefaultHolesDepth(surveyId);
         
            //BUSCA PRODUÇõES E PONTOS DO TRECHO
            var swath = await _swathRepository.GetSwath(surveyId, swathNumber);
            var productions = await
                _pointProductionRepository.ListStretchLastWorks(surveyId,
                    operationalFrontId, (PreplotPointType)preplotPointType, swath.InitialShotPoint,
                    swath.FinalShotPoint);
            var points =
                await _preplotPointRepository.ListPreplotPointsWithoutCoordinates(surveyId, (PreplotPointType)preplotPointType,
                    swath.InitialShotPoint, swath.FinalShotPoint);
            var index = 1;
            var opFrontType =await  _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);

            foreach (var point in points)
            {
                var previousProductionObs = "";
                previousProductionObs = await
                       _pointProductionRepository.GetPreviousFrontsLastProductionsOrReworksObservations(surveyId, point.PreplotPointId,
                            point.PreplotVersionId, point.PreplotPointType, operationalFrontId);

                var pointProduction = productions.FirstOrDefault(sp =>
                    sp.PreplotPointId == point.PreplotPointId &&
                    sp.PreplotVersionId == point.PreplotVersionId &&
                    sp.PreplotPointType == point.PreplotPointType);

                if (pointProduction == null)
                {
                    var production = new PointProductionModel
                    {
                        Index = index,
                        IsFromDb = false,
                        Line = point.LineName,
                        PreplotPointType = point.PreplotPointType,
                        StationNumber = point.StationNumber,
                        PreplotPointId = point.PreplotPointId,
                        PreplotVersionId = point.PreplotVersionId,
                        SurveyDefaultHolesQuantity = surveyHolesQuantity.Value,
                        SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
                        WorkNumber = 0,
                        HolesDepth = 0,
                        PreviousFrontProductionObservation = previousProductionObs
                    };
                    production.Holes = await ListHoles(production, surveyHolesQuantity.Value, opFrontType);
                    productions.Add(production);
                }
                else
                {
                    pointProduction.Index = index;
                    pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity.Value;
                    pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
                    //POINT INFO------------------------
                    pointProduction.Line = point.LineName;
                    pointProduction.StationNumber = point.StationNumber;
                    //HOLES INFO------------------------
                    pointProduction.Holes = await ListHoles(pointProduction, surveyHolesQuantity.Value, opFrontType);
                    pointProduction.PreviousFrontProductionObservation = previousProductionObs;
                }
                index++;
            }
            return productions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
        }

        public async Task<List<PointProductionModel>> ListOrBuildStretchPoints(int surveyId, int preplotPointType, int operationalFrontId, string line,
           int initialStation, int finalStation, string date, int frontGroupId, int frontGroupLeaderId)
        {
            if (initialStation > finalStation)
            {
                var bkp = initialStation;
                initialStation = finalStation;
                finalStation = bkp;
            }

            try
            {
               
                //BUSCA DADOS GERAIS DOS PONTOS
                var frontGroupName = await _frontGroupRepository.GetFrontGroupName(frontGroupId);
                var frontGroupLeaderName = await _frontGroupLeaderRepository.GetFrontGroupLeaderName(frontGroupLeaderId);
                var dateFormated = DateHelper.StringToDate(date);
                var surveyHolesDepth = await _surveyRepository.GetSurveyDefaultHolesDepth(surveyId);
                var surveyHolesQuantity = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
                var opFrontType = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);
                var previousOperationalFrontId = await _operationalFrontRepository.GetPreviousOperationalFrontId(operationalFrontId);


                //BUSCA PRODUÇõES E PONTOS DO TRECHO
                var productions = await _pointProductionRepository.ListStretchProductions(surveyId, (PreplotPointType)preplotPointType, operationalFrontId, line, initialStation, finalStation, 1);
                var points = await _preplotPointRepository.ListPreplotPointsWithoutCoordinates(surveyId, (PreplotPointType)preplotPointType, line, initialStation, finalStation);
                IList<PointProductionModel> newProductions = new List<PointProductionModel>();

                foreach (var point in points)
                {
                    var pointProduction = productions.FirstOrDefault(sp =>
                        sp.PreplotPointId == point.PreplotPointId &&
                        sp.PreplotVersionId == point.PreplotVersionId &&
                        sp.PreplotPointType == point.PreplotPointType);

                    var previousProduction = new PointProductionModel();
                    if (previousOperationalFrontId.HasValue)
                        previousProduction =
                             await _pointProductionRepository.GetPreviousFrontLastProductionOrRework(surveyId, point.PreplotPointId,
                                point.PreplotVersionId, point.PreplotPointType, previousOperationalFrontId.Value) ??
                            new PointProductionModel();

                    if (pointProduction == null)
                    {

                        var productionStatus = (previousProduction.Status == (int)ProductionStatus.OnlyReceptorStations || previousProduction.Status == (int)ProductionStatus.Accomplished)
                                                ? (int)ProductionStatus.Accomplished
                                                : (int)ProductionStatus.Unaccomplished;

                        var finalHolesQuantity = (opFrontType == OperationalFrontType.Drilling ? surveyHolesQuantity : (previousProduction.Status != (int)ProductionStatus.Accomplished
                                                ? 0
                                                : previousProduction.FinalHolesQuantity));

                        var numberOfChargesInShotPoint = previousProduction.Status != (int)ProductionStatus.Accomplished
                                                ? 0
                                                : previousProduction.FinalHolesQuantity;

                        var numberOfFusesInshotPoint = previousProduction.Status != (int)ProductionStatus.Accomplished
                                                ? 0
                                                : previousProduction.FinalHolesQuantity;


                        var newProd = new PointProductionModel
                        {
                            IsFromDb = false,
                            IsFromAnotherDate = false,
                            Line = line,
                            PreplotPointType = point.PreplotPointType,
                            PreplotPointTypeName = EnumHelper.GetEnumDescription(point.PreplotPointType),
                            DateString = date,
                            Date = dateFormated,
                            SavingDate = dateFormated,
                            StationNumber = point.StationNumber,
                            PreplotPointId = point.PreplotPointId,
                            PreplotVersionId = point.PreplotVersionId,
                            FrontGroupId = frontGroupId,
                            FrontGroupName = frontGroupName,
                            FrontGroupLeaderId = frontGroupLeaderId,
                            FrontGroupLeaderName = frontGroupLeaderName,
                            SurveyDefaultHolesDepth = surveyHolesDepth ?? 0,
                            //PREVIOUS PRODUCTION INFO----------
                            PreviousFrontProduction = previousProduction,
                            //DEFAULT VALUES--------------
                            Status = productionStatus,
                            Observation = previousProduction.Observation,
                            DisplacementRuleId = previousProduction.DisplacementRuleId,
                            ReductionRuleId = previousProduction.ReductionRuleId,
                            FinalHolesQuantity = finalHolesQuantity.Value,
                            SurveyDefaultHolesQuantity = surveyHolesQuantity.Value,
                            WorkNumber = 1,
                            NumberOfChargesInShotPoint = numberOfChargesInShotPoint,
                            NumberOfFusesInShotPoint = numberOfFusesInshotPoint,
                            FusesType = previousProduction.FusesType,
                            ChargesType = previousProduction.ChargesType
                        };
                        newProd.HolesDepth = (opFrontType == OperationalFrontType.Permit ||
                                              opFrontType == OperationalFrontType.Topography ||
                                              opFrontType == OperationalFrontType.SeismoA ||
                                              opFrontType == OperationalFrontType.SeismoB ||
                                              opFrontType == OperationalFrontType.Gravimetry ||
                                              opFrontType == OperationalFrontType.Magnetometry)
                                              ? 0 :
                                              (previousProduction.HolesDepth >= 0 ?
                                                previousProduction.HolesDepth : surveyHolesQuantity.Value);
                        newProductions.Add(newProd);
                    }
                    else
                    {
                        if (pointProduction.Date != dateFormated) pointProduction.IsFromAnotherDate = true;
                        //POINT INFO------------------------
                        pointProduction.Line = point.LineName;
                        pointProduction.StationNumber = point.StationNumber;
                        //PREVIOUS PRODUCTION INFO----------
                        pointProduction.PreviousFrontProduction = previousProduction;
                        pointProduction.SurveyDefaultHolesQuantity = surveyHolesQuantity.Value;
                        pointProduction.SurveyDefaultHolesDepth = surveyHolesDepth ?? 0;
                        pointProduction.SavingDate = dateFormated;
                        if (pointProduction.Observation == null || pointProduction.Observation == "")
                            pointProduction.Observation = previousProduction.Observation;
                        if (opFrontType == OperationalFrontType.Charging || opFrontType == OperationalFrontType.Detonation)
                        {
                            pointProduction.ReductionRuleId = previousProduction.ReductionRuleId;
                            pointProduction.DisplacementRuleId = previousProduction.DisplacementRuleId;
                            pointProduction.HolesDepth = previousProduction.HolesDepth;
                            pointProduction.ChargesType = previousProduction.ChargesType;
                            pointProduction.FusesType = previousProduction.FusesType;
                        }

                        newProductions.Add(pointProduction);
                    }
                }
               return newProductions.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> SaveProduction(int surveyId, int operationalFrontId,
           List<PointProductionModel> pointProductions, int frontGroupId, int frontGroupLeaderId, decimal initialStationBkp, decimal finalStationBkp, bool savingFromRobFile, String userLogin)
        {
            await _surveyRepository.GetSurvey(surveyId);
            return true;
            //await _pointProductionRepository.BeginTransaction();
            //holeDal.BeginTransaction();
            //stretchDal.BeginTransaction();
            //try
            //{
            //    var operationalFront = await _operationalFrontRepository.GetOperationalFront(operationalFrontId);

            //    List<PointProductionModel> invalidProductions;
            //    List<PointProductionModel> validProductions;
            //    List<HoleModel> holesToDelete;
            //    List<HoleModel> holesToCreate;

            //    ValidateProduction(surveyId, pointProductions, frontGroupId, frontGroupLeaderId,
            //        out validProductions, out invalidProductions, out holesToDelete, out holesToCreate, operationalFront);

            //    foreach (PointProductionModel p in validProductions)
            //        p.LastEditorUserLogin = userLogin;

            //    if (invalidProductions.Any())
            //    {
            //        invalidProductions.AddRange(validProductions);
            //        var productionsWithErrors = invalidProductions;
            //        result.Data = productionsWithErrors;
            //        result.ErrorCode = ErrorCode.PassToCallback;
            //        result.ErrorMessage =
            //            "A produção do trecho selecionado não foi gravada. Existem alguns pontos impossibilitados de serem salvos.";
            //    }
            //    else
            //    {
            //        var firstPointProductionModel = pointProductions.ToList().FirstOrDefault();
            //        var stretchModel = new StretchModel();
            //        if (firstPointProductionModel != null)
            //        {
            //            stretchModel.DateString = DateHelper.DateToString(firstPointProductionModel.Date);
            //            stretchModel.FinalStation = pointProductions.Max(m => m.StationNumber);
            //            stretchModel.InitialStation = pointProductions.Min(m => m.StationNumber);
            //            stretchModel.FinalStationBkp = finalStationBkp == 0 ? pointProductions.Max(m => m.StationNumber) : finalStationBkp;
            //            stretchModel.InitialStationBkp = initialStationBkp == 0 ? pointProductions.Min(m => m.StationNumber) : initialStationBkp;
            //            stretchModel.FrontGroupId = frontGroupId;
            //            stretchModel.FrontGroupLeaderId = frontGroupLeaderId;
            //            stretchModel.Line = firstPointProductionModel.Line;
            //            stretchModel.OperationalFrontId = operationalFrontId;
            //            stretchModel.SurveyId = surveyId;
            //        }

            //        //TRATAR A AÇÂO COMO TRANSACTION, COM POSSIBILIDADE DE ROLLBACK EM CASO DE ERRO
            //        if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Charging
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
            //            holeDal.DeleteHoles(holesToDelete);

            //        var productionsToDelete = new List<PointProductionModel>();
            //        if (stretchModel.InitialStation > stretchModel.InitialStationBkp)
            //        {
            //            var prodsToDelete = await _pointProductionRepository.ListStretchProductions(surveyId,
            //                PreplotPointType.All,
            //                operationalFront.OperationalFrontId, stretchModel.Line,
            //                stretchModel.InitialStationBkp, stretchModel.InitialStation, 1).OrderBy(m => m.StationNumber).ToList();
            //            prodsToDelete.Remove(prodsToDelete.LastOrDefault());
            //            productionsToDelete.AddRange(prodsToDelete);
            //        }
            //        if (stretchModel.FinalStation < stretchModel.FinalStationBkp)
            //        {
            //            var prodsToDelete = await _pointProductionRepository.ListStretchProductions(surveyId,
            //                PreplotPointType.All,
            //                operationalFront.OperationalFrontId, stretchModel.Line,
            //                stretchModel.FinalStation, stretchModel.FinalStationBkp, 1).OrderBy(m => m.StationNumber).ToList();
            //            prodsToDelete.Remove(prodsToDelete.FirstOrDefault());
            //            productionsToDelete.AddRange(prodsToDelete);
            //        }

            //        var deletionResult = DeleteProduction(surveyId, operationalFrontId, productionsToDelete, 1, await _pointProductionRepository, holeDal);
            //        if (!string.IsNullOrWhiteSpace(deletionResult.ErrorMessage))
            //            throw new Exception("Não foi possível editar o trecho selecionado. Verifique as produções lançadas.");

            //        if (operationalFront.OperationalFrontType == OperationalFrontType.Detonation && !savingFromRobFile)
            //        {
            //            var seisRegDal = new SeismicRegisterDAL();
            //            var lastFfid = seisRegDal.GetLastFfid(surveyId);
            //            var seisRegisters = new List<SeismicRegisterModel>();
            //            foreach (var prod in validProductions)
            //            {
            //                prod.LastEditorUserLogin = userLogin;
            //                lastFfid++;
            //                if (!prod.Ffid.HasValue)
            //                    prod.Ffid = lastFfid;
            //                if (prod.Ffid != null)
            //                    seisRegisters.Add(new SeismicRegisterModel()
            //                    {
            //                        Ffid = prod.Ffid.Value,
            //                        SurveyId = surveyId,
            //                        Comment = prod.Observation,
            //                        Date = prod.Date,
            //                        IsNoiseTest = false,
            //                        LineName = prod.Line,
            //                        OperationalFrontId = prod.OperationalFrontId,
            //                        PointNumber = prod.StationNumber,
            //                        PreplotPointId = prod.PreplotPointId,
            //                        PreplotPointType = (int)prod.PreplotPointType,
            //                        PreplotVersionId = prod.PreplotVersionId,
            //                        QualityControlStatus = prod.QualityControlStatus,
            //                        WorkNumber = prod.WorkNumber
            //                    });
            //            }
            //            await _pointProductionRepository.SaveProductions(validProductions);
            //            if (seisRegisters.Any()) seisRegDal.SaveSeismicRegisters(seisRegisters);
            //        }
            //        else
            //            await _pointProductionRepository.SaveProductions(validProductions);

            //        StretchBLL.SaveStretch(stretchModel, false, stretchDal);

            //        if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Charging
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
            //            || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
            //            holeDal.AddHoles(holesToCreate);
            //        //-----------------------------------------------------------------------------
            //        await _pointProductionRepository.CommitTransaction();
            //        holeDal.CommitTransaction();
            //        stretchDal.CommitTransaction();

            //        result.Data = new StretchDAL().GetSurveyProductionData(surveyId, "");
            //    }
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    stretchDal.RollbackTransaction();
            //    holeDal.RollbackTransaction();
            //    await _pointProductionRepository.RollbackTransaction();

            //    result.ErrorMessage = ex.Message;
            //    result.ErrorCode = ErrorCode.DalException;
            //}
            //return result;
        }

        public async Task<List<PointProductionModel>> SaveMultipleStretchesProduction(int surveyId, int operationalFrontId,
           List<List<PointProductionModel>> productionsPerStretch, int frontGroupId, int frontGroupLeaderId)
        {

            await _surveyRepository.GetSurvey(surveyId);
            return new List<PointProductionModel>();
             //await _pointProductionRepository.BeginTransaction();
            //holeDal.BeginTransaction();
            //stretchDal.BeginTransaction();
            //try
            //{
            //    var operationalFront = new OperationalFrontDAL().GetOperationalFront(operationalFrontId);

            //    var invalidProductions = new List<PointProductionModel>();
            //    var validProductions = new List<PointProductionModel>();
            //    var stretchesWithHoles = new List<StretchWithHolesToCreateAndDelete>();
            //    foreach (var stretchProductions in productionsPerStretch)
            //    {
            //        var holesOnStretchToDelete = new List<HoleModel>();
            //        var holesOnStretchToCreate = new List<HoleModel>();
            //        var invalidProductionsOnStretch = new List<PointProductionModel>();
            //        var validProductionsOnStretch = new List<PointProductionModel>();

            //        ValidateProduction(surveyId, stretchProductions, frontGroupId, frontGroupLeaderId,
            //            out validProductionsOnStretch, out invalidProductionsOnStretch, out holesOnStretchToDelete, out holesOnStretchToCreate,
            //            operationalFront);

            //        invalidProductions.AddRange(invalidProductionsOnStretch);
            //        validProductions.AddRange(validProductionsOnStretch);
            //        stretchesWithHoles.Add(new StretchWithHolesToCreateAndDelete()
            //        {
            //            HolesToCreate = holesOnStretchToCreate,
            //            HolesToDelete = holesOnStretchToDelete,
            //            Productions = stretchProductions
            //        });
            //    }
            //    if (invalidProductions.Any())
            //    {
            //        invalidProductions.AddRange(validProductions);
            //        var productionsWithErrors = invalidProductions;
            //        result.Data = productionsWithErrors;
            //        result.ErrorCode = ErrorCode.PassToCallback;
            //        result.ErrorMessage =
            //            "A produção dos trechos selecionados não foi gravada. Existem alguns trechos ou pontos impossibilitados de serem salvos.";
            //    }
            //    else
            //    {
            //        foreach (var stretchProductions in stretchesWithHoles)
            //        {
            //            var firstPointProductionModel = stretchProductions.Productions.ToList().FirstOrDefault();
            //            var stretchModel = new StretchModel();
            //            if (firstPointProductionModel != null)
            //            {
            //                stretchModel.DateString = DateHelper.DateToString(firstPointProductionModel.Date);
            //                stretchModel.FinalStation = stretchProductions.Productions.Max(m => m.StationNumber);
            //                stretchModel.InitialStation = stretchProductions.Productions.Min(m => m.StationNumber);
            //                stretchModel.FrontGroupId = frontGroupId;
            //                stretchModel.FrontGroupLeaderId = frontGroupLeaderId;
            //                stretchModel.Line = firstPointProductionModel.Line;
            //                stretchModel.OperationalFrontId = operationalFrontId;
            //                stretchModel.SurveyId = surveyId;
            //            }

            //            TRATAR A AÇÂO COMO TRANSACTION, COM POSSIBILIDADE DE ROLLBACK EM CASO DE ERRO
            //            if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Charging
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
            //                holeDal.DeleteHoles(stretchProductions.HolesToDelete);

            //            await _pointProductionRepository.SaveProductions(validProductions);
            //            StretchBLL.SaveStretch(stretchModel, false, stretchDal);

            //            if (operationalFront.OperationalFrontType == OperationalFrontType.Drilling
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Charging
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Detonation
            //                || operationalFront.OperationalFrontType == OperationalFrontType.Inspection)
            //                holeDal.AddHoles(stretchProductions.HolesToCreate);
            //            -----------------------------------------------------------------------------
            //        }
            //    }
            //    await _pointProductionRepository.CommitTransaction();
            //    holeDal.CommitTransaction();
            //    stretchDal.CommitTransaction();
            //    result.Data = true;
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    stretchDal.RollbackTransaction();
            //    holeDal.RollbackTransaction();
            //    await _pointProductionRepository.RollbackTransaction();

            //    result.ErrorMessage = ex.Message;
            //    result.ErrorCode = ErrorCode.DalException;
            //}
            //return result;
        }

        public async Task<List<PointProductionModel>> DeleteFrontGroupProduction(int workNumber, IList<StretchModel> stretches)
        {

            try
            {
                if (stretches != null)
                {
                    var allStretchesProductions = new List<PointProductionModel>();
                    var firstStretch = stretches.FirstOrDefault();

                    if (firstStretch != null)
                    {
                        var surveyId = firstStretch.SurveyId;
                        var opFronId = firstStretch.OperationalFrontId;
                        foreach (var stretch in stretches)
                        {
                            var stretchProductions = await _pointProductionRepository.ListStretchProductions(stretch.SurveyId,
                                PreplotPointType.All, stretch.OperationalFrontId, stretch.Line,
                                stretch.InitialStation,
                                stretch.FinalStation, workNumber);

                            allStretchesProductions.AddRange(stretchProductions);
                        }
                        var result = await DeleteProduction(surveyId, opFronId, allStretchesProductions, workNumber);
                       
                        foreach (var stretch in stretches)
                            await _stretchService.DeleteStretch(stretch);

                        return result;

                    }
                    else
                        throw new Exception("Não existem trechos a serem excluídos.");
                }
                return new List<PointProductionModel>();
            }
            catch (Exception ex) { throw ex; }
        }


        public async Task<List<PointProductionModel>> DeleteStretchProduction(int workNumber, StretchModel stretch)
        {

            try
            {  
                var pointProductions = await _pointProductionRepository.ListStretchProductions(stretch.SurveyId, PreplotPointType.All,
                    stretch.OperationalFrontId, stretch.Line, stretch.InitialStation, stretch.FinalStation, workNumber);

               var result =  await DeleteProduction(stretch.SurveyId, stretch.OperationalFrontId, pointProductions, workNumber);
              await _stretchService.DeleteStretch(stretch);
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<PointProductionModel>> DeleteProduction(int surveyId, int operationalFrontId,
           IEnumerable<PointProductionModel> pointProductions, int workNumber)
        {

            try
            {
                var invalidProductions = new List<PointProductionModel>();
                var validProductions = new List<PointProductionModel>();
                var holesToDelete = new List<HoleModel>();
                var opFrontType = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);

                if (pointProductions != null)
                {
                    foreach (var production in pointProductions)
                    {
                        var existingProduction = await _pointProductionRepository.GetProduction(surveyId, production.PreplotPointId,
                            production.PreplotVersionId,
                            workNumber, production.PreplotPointType, operationalFrontId);
                        if (existingProduction == null)
                        {
                            invalidProductions.Add(production);
                            continue;
                        }
                        var hasNextOperationalFrontsProductionsInFutureDates = await _pointProductionRepository.HasNextOperationalFrontsProductionsInFutureDates(production);
                        if (hasNextOperationalFrontsProductionsInFutureDates)
                        {
                            invalidProductions.Add(production);
                            continue;
                        }
                        validProductions.Add(production);
                        var holes = await _holeRepository.ListHoles(production.SurveyId,
                            production.PreplotPointId, production.PreplotVersionId,
                            production.PreplotPointType, operationalFrontId);
                        holesToDelete.AddRange(holes.ToList());
                    }
                }
                if (invalidProductions.Any())
                {
                    invalidProductions.AddRange(validProductions);
                    var productionsWithErrors = invalidProductions;
                    return productionsWithErrors;
                   
                }
                else
                {
                    if (holesToDelete.Any()) await _holeRepository.DeleteHoles(holesToDelete);
                    if (opFrontType == OperationalFrontType.Detonation) await _seismicRegisterRepository.DeleteSeismicRegisters(validProductions);
                    foreach (var production in validProductions)
                       await _pointProductionRepository.DeleteProduction(production);
                   
                }
                return pointProductions.ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        private async Task ValidateProduction(int surveyId,
            IEnumerable<PointProductionModel> pointProductions,
            int frontGroupId, int frontGroupLeaderId,  List<PointProductionModel> validProductions,
             List<PointProductionModel> invalidProductions,  List<HoleModel> holesToDelete,
             List<HoleModel> holesToCreate, OperationalFrontModel operationalFront)
        {
            invalidProductions = new List<PointProductionModel>();
            validProductions = new List<PointProductionModel>();
            holesToDelete = new List<HoleModel>();
            holesToCreate = new List<HoleModel>();
            
            var previousOperationalFront = await _operationalFrontRepository.GetPreviousOperationalFront(operationalFront.OperationalFrontId);
            var holesPerShotPoint = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);

            foreach (var production in pointProductions)
            {
                //Validar se a produção já existe
                var existingProduction = await _pointProductionRepository.GetProduction(surveyId, production.PreplotPointId,
                    production.PreplotVersionId, production.WorkNumber, production.PreplotPointType, operationalFront.OperationalFrontId);
                production.SurveyId = surveyId;
                production.OperationalFrontId = operationalFront.OperationalFrontId;
                production.FrontGroupId = frontGroupId;
                production.FrontGroupLeaderId = frontGroupLeaderId;
                production.IsActive = true;

                //Validar produção em ponto Liberado Somente ER
                if (production.Status == (int)ProductionStatus.Accomplished &&
                    production.PreplotPointType == PreplotPointType.ShotPoint &&
                    await _pointProductionRepository.HasProductionsWithOnlyStationsStatusInPastDates(production))
                {
                    production.SavingErrorAlert = "Produção não liberada pela Permissoria para Pontos de Tiro.";
                    invalidProductions.Add(production);
                    continue;
                }
                //Validar produção de Detonação sem FFID
                if (production.Ffid != null && production.Ffid > 0 &&
                    production.Status != (int)ProductionStatus.Accomplished)
                {
                    production.SavingErrorAlert = "Produção com FFID não pode ter situação 'Não Detonado'.";
                    invalidProductions.Add(production);
                    continue;
                }
                if (existingProduction != null && existingProduction.IsActive) //EDIÇÃO
                {
                    if (production.SavingDate.Date != existingProduction.Date.Date) //EDIÇÃO DE DATA
                    {
                        production.SavingErrorAlert =
                                "A produção desta frente operacional para este ponto já existe na data de " + existingProduction.Date.Date.ToShortDateString() + ". Tente salvar a produção selecionando essa data.";
                        invalidProductions.Add(production);
                        continue;

                    }
                    //SE NÃO É EDIÇÃO DE DATA, MAS É DE TURMA/LIDER
                    if (existingProduction.FrontGroupId != frontGroupId || existingProduction.FrontGroupLeaderId != frontGroupLeaderId)
                    {
                        production.SavingErrorAlert = "A produção deste ponto já foi lançada para outra Turma.";
                        invalidProductions.Add(production);
                        continue;
                    }

                    //SELECIONA OS FUROS A EXCLUIR
                    var existingHoles = await _holeRepository.ListHoles(production.SurveyId,
                        production.PreplotPointId, production.PreplotVersionId,
                        production.PreplotPointType, operationalFront.OperationalFrontId);
                    holesToDelete.AddRange(existingHoles);
                }
                else //INSERÇÃO
                {
                    if (operationalFront.PreviousOperationalFrontId.HasValue)
                    {
                        var previousPastProductions = await
                            _pointProductionRepository.ListPreviousOperationalFrontsProductionsInPastDates(production, operationalFront.PreviousOperationalFrontId.Value) ??
                            new List<PointProductionModel>();
                        var pointProductionModels = previousPastProductions as IList<PointProductionModel> ?? previousPastProductions.ToList();
                        if (operationalFront.OperationalFrontType != OperationalFrontType.Inspection)
                        {
                            if (!pointProductionModels.Any())
                            {
                                production.SavingErrorAlert = "Não existe produção para o ponto, em uma data anterior, na frente predecessora.";
                                invalidProductions.Add(production);
                                continue;
                            }
                            if (pointProductionModels.Any(prod => prod.Status == (int)ProductionStatus.Unaccomplished))
                            {
                                if (production.Status == (int)ProductionStatus.Accomplished)
                                {
                                    production.SavingErrorAlert = "A produção para o ponto na frente anterior tem status igual a " +
                                                                  EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, previousOperationalFront.OperationalFrontType);
                                    invalidProductions.Add(production);
                                    continue;
                                }
                            }
                        }

                    }
                    production.WorkNumber = await _pointProductionRepository.GetMaxWorkNumber(surveyId, production.PreplotPointId,
                        production.PreplotVersionId, production.PreplotPointType, operationalFront.OperationalFrontId) + 1;
                }

                if (operationalFront.OperationalFrontType == OperationalFrontType.Gravimetry && !production.HasOasis)
                {
                    production.SavingErrorAlert = "O dado ainda não foi processado no Oasis montaj.";
                    invalidProductions.Add(production);
                    continue;
                }

                if (production.Status == (int)ProductionStatus.Accomplished)
                {
                    //SELECIONA OS FUROS A INSERIR
                    var newHoles = await InstantiateHoles(production, holesPerShotPoint.Value, false, operationalFront.OperationalFrontType);
                    holesToCreate.AddRange(newHoles);
                }
                production.SavingErrorAlert = "OK";
                validProductions.Add(production);
            }
        }

        public async Task<IEnumerable<HoleModel>> InstantiateHoles(PointProductionModel production, int holesPerShotPoint, bool instantiateAbsentHoles, OperationalFrontType frontType)
        {
            var holes = new List<HoleModel>();

            var finalHolesQuantity = holesPerShotPoint;
            if (production.ReductionRuleId.HasValue)
                finalHolesQuantity = await _reductionRuleRepository.GetReductionFinalHolesQuantity(production.ReductionRuleId.Value);
            if (production.Status != (int)ProductionStatus.Accomplished)
                finalHolesQuantity = 0;

            var centralPosition = holesPerShotPoint / 2;
            var countLeft = 1;
            var countRight = 1;

            for (var i = 0; i < finalHolesQuantity; i++)
            {
                int holeNumber;
                if (i == 0)
                    holeNumber = centralPosition;
                else
                {
                    if (i % 2 != 0)
                    {
                        holeNumber = centralPosition + countRight;
                        countRight++;
                    }
                    else
                    {
                        holeNumber = centralPosition - countLeft;
                        countLeft++;
                    }
                }
                var newHole = new HoleModel()
                {
                    Depth = production.HolesDepth,
                    HoleNumber = holeNumber,
                    IsActive = true,
                    OperationalFrontId = production.OperationalFrontId,
                    PreplotPointId = production.PreplotPointId,
                    PreplotPointType = production.PreplotPointType,
                    PreplotVersionId = production.PreplotVersionId,
                    SurveyId = production.SurveyId,
                    FrontGroupId = production.FrontGroupId,
                    WorkNumber = production.WorkNumber,
                    Date = production.Date,
                };
                if (frontType != OperationalFrontType.Drilling)
                {
                    newHole.ChargesTypeId = production.ChargesType;
                    newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
                    newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
                    newHole.FusesTypeId = production.FusesType;
                }
                holes.Add(newHole);
            }
            if (instantiateAbsentHoles)
            {
                for (var i = finalHolesQuantity; i < holesPerShotPoint; i++)
                {
                    int holeNumber;
                    if (i == 0)
                        holeNumber = centralPosition;
                    else
                    {
                        if (i % 2 != 0)
                        {
                            holeNumber = centralPosition + countRight;
                            countRight++;
                        }
                        else
                        {
                            holeNumber = centralPosition - countLeft;
                            countLeft++;
                        }
                    }
                    holes.Add(new HoleModel { HoleNumber = holeNumber });
                }
            }
            return holes.OrderBy(h => h.HoleNumber).ToList();
        }

        public async Task<IEnumerable<HoleModel>> ListHoles(PointProductionModel production, int holesPerShotPoint, OperationalFrontType frontType)
        {
            var holes = new List<HoleModel>();

            var finalHolesQuantity = holesPerShotPoint;
            if (production.ReductionRuleId.HasValue)
                finalHolesQuantity = await _reductionRuleRepository.GetReductionFinalHolesQuantity(production.ReductionRuleId.Value);
            if (production.Status != (int)ProductionStatus.Accomplished)
                finalHolesQuantity = 0;

            var centralPosition = holesPerShotPoint / 2;
            var countLeft = 1;
            var countRight = 1;
            if (finalHolesQuantity != 2)
            {
                for (var i = 0; i < finalHolesQuantity; i++)
                {
                    int holeNumber;
                    if (i == 0)
                        holeNumber = centralPosition;
                    else
                    {
                        if (i % 2 != 0)
                        {
                            holeNumber = centralPosition + countRight;
                            countRight++;
                        }
                        else
                        {
                            holeNumber = centralPosition - countLeft;
                            countLeft++;
                        }
                    }
                    var newHole = new HoleModel()
                    {
                        Depth = production.HolesDepth,
                        HoleNumber = holeNumber,
                        IsActive = true,
                        OperationalFrontId = production.OperationalFrontId,
                        PreplotPointId = production.PreplotPointId,
                        PreplotPointType = production.PreplotPointType,
                        PreplotVersionId = production.PreplotVersionId,
                        SurveyId = production.SurveyId,
                        FrontGroupId = production.FrontGroupId,
                        WorkNumber = production.WorkNumber,
                        Date = production.Date,
                    };
                    if (frontType != OperationalFrontType.Drilling)
                    {
                        newHole.ChargesTypeId = production.ChargesType;
                        newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
                        newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
                        newHole.FusesTypeId = production.FusesType;
                    }
                    holes.Add(newHole);
                }
                for (var i = finalHolesQuantity; i < holesPerShotPoint; i++)
                {
                    int holeNumber;
                    if (i == 0)
                        holeNumber = centralPosition;
                    else
                    {
                        if (i % 2 != 0)
                        {
                            holeNumber = centralPosition + countRight;
                            countRight++;
                        }
                        else
                        {
                            holeNumber = centralPosition - countLeft;
                            countLeft++;
                        }
                    }
                    holes.Add(new HoleModel { HoleNumber = holeNumber });
                }
            }
            else
            {
                var newHole = new HoleModel()
                {
                    Depth = production.HolesDepth,
                    HoleNumber = 0,
                    IsActive = true,
                    OperationalFrontId = production.OperationalFrontId,
                    PreplotPointId = production.PreplotPointId,
                    PreplotPointType = production.PreplotPointType,
                    PreplotVersionId = production.PreplotVersionId,
                    SurveyId = production.SurveyId,
                    FrontGroupId = production.FrontGroupId,
                    WorkNumber = production.WorkNumber,
                    Date = production.Date,
                };
                if (frontType != OperationalFrontType.Drilling)
                {
                    newHole.ChargesTypeId = production.ChargesType;
                    newHole.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
                    newHole.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
                    newHole.FusesTypeId = production.FusesType;
                }
                holes.Add(newHole);
                holes.Add(new HoleModel { HoleNumber = 1 });
                var newHole2 = new HoleModel()
                {
                    Depth = production.HolesDepth,
                    HoleNumber = 2,
                    IsActive = true,
                    OperationalFrontId = production.OperationalFrontId,
                    PreplotPointId = production.PreplotPointId,
                    PreplotPointType = production.PreplotPointType,
                    PreplotVersionId = production.PreplotVersionId,
                    FrontGroupId = production.FrontGroupId,
                    SurveyId = production.SurveyId,
                    WorkNumber = production.WorkNumber,
                    Date = production.Date,
                };
                if (frontType != OperationalFrontType.Drilling)
                {
                    newHole2.ChargesTypeId = production.ChargesType;
                    newHole2.NumberOfFuses = production.NumberOfFusesInShotPoint / finalHolesQuantity;
                    newHole2.NumberOfCharges = production.NumberOfChargesInShotPoint / finalHolesQuantity;
                    newHole2.FusesTypeId = production.FusesType;
                }
                holes.Add(newHole2);
            }
            return holes.OrderBy(h => h.HoleNumber).ToList();
        }

    }
}
