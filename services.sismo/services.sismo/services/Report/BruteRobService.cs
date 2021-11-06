using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services.Report;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services.Report
{
    public class BruteRobImportHelperService : IBruteRobImportHelperService
    {


        private readonly IPointProductionRepository _pointProductionRepository;
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly IOperationalFrontRepository _operationalFrontRepository;
        private readonly IConfiguration _configuration;

        public BruteRobImportHelperService(
            IOperationalFrontRepository operationalFrontRepository,
           IPreplotPointRepository preplotPointRepository,
            IPointProductionRepository pointProductionRepository,
            IConfiguration configuration)
        {
            _operationalFrontRepository = operationalFrontRepository;
            _preplotPointRepository = preplotPointRepository;
            _pointProductionRepository = pointProductionRepository;
            _configuration = configuration;
        }
        public static bool HasValidExtension(string fileName)
        {
            return fileName.Split('.').Last().ToUpper() == "CSV";
        }

        public async Task<List<KeyValuePair<SeismicRegisterModel, PointProductionModel>>> ExtractSeismicRegistersAndProductions(Stream myStream, BruteRobParametersModel robParameters, List<PreplotPointModel> consideringPoints)
        {
            var registersAndProductionsFromFile = new List<KeyValuePair<SeismicRegisterModel, PointProductionModel>>();
            var lines = new List<string>();
            var extractionStarted = false;
            var columnsIndexes = new BruteRobColumnsIndexes();
            var fullDateFormat = "";
           
            using (var sr = new StreamReader(myStream))
            {
                while (sr.Peek() >= 0)
                {
                    try
                    {
                        var line = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line) && line.TrimStart(' ').StartsWith("DateString format: "))
                        {
                            var splitLine = line.Split(';');
                            fullDateFormat = splitLine[0].TrimStart(' ').Substring(("DateString format: ").Length);
                        }
                        if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("File #"))
                        {
                            var splitLine = line.Split(';');
                            for (var i = 0; i < splitLine.Length; i++)
                            {
                                switch (splitLine[i])
                                {
                                    case "File #":
                                        columnsIndexes.FfidIndex = i;
                                        break;
                                    case "Shot #":
                                        columnsIndexes.ShotNumberIndex = i;
                                        break;
                                    case "Swath":
                                        columnsIndexes.SwathIndex = i;
                                        break;
                                    case "ITB":
                                        columnsIndexes.ItbIndex = i;
                                        break;
                                    case "Date":
                                        columnsIndexes.DateIndex = i;
                                        break;
                                    case "Point Number":
                                        columnsIndexes.PointNumberIndex = i; ;
                                        break;
                                    case "Nb Of Dead Seis":
                                        columnsIndexes.NbOfDeadSeisIndex = i;
                                        break;
                                    case "Comment":
                                        columnsIndexes.CommentIndex = i;
                                        break;
                                    case "Line Name":
                                        columnsIndexes.LineNameIndex = i;
                                        break;
                                    case "Blaster Id":
                                        columnsIndexes.BlasterIdIndex = i;
                                        break;
                                    case "Blaster Shot Status":
                                        columnsIndexes.BlasterShotStatusIndex = i;
                                        break;
                                    case "Uphole Time":
                                        columnsIndexes.UpholeTimeIndex = i;
                                        break;
                                    case "Nb Of Live Seis":
                                        columnsIndexes.NbOfLiveSeisIndex = i;
                                        break;
                                    case "Is Noise Test":
                                        columnsIndexes.IsNoiseTestIndex = i;
                                        break;
                                }
                            }
                            extractionStarted = true;
                            if (columnsIndexes.GetType().GetProperties().Any(index => index.GetValue(columnsIndexes).Equals(-1)))
                                throw new Exception("O arquivo importado não contém todas as colunas necessárias, ou o nome de uma das colunas não está conforme o padrão. Favor verificar novamente.");
                        }
                        else
                            if (extractionStarted)
                            lines.Add(line);
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        if (string.IsNullOrWhiteSpace(msg))
                            msg = "Erro ao extrair os dados no arquivo de dump.";
                        throw new Exception(msg);
                    }
                }
                myStream.Flush();
                var previousOpFrontId = await _operationalFrontRepository.GetPreviousOperationalFrontId(robParameters.OperationalFrontId) ?? 0;

                var dataFiel = lines.Select(line => line.Split(';'));
                foreach(var columnsFields in dataFiel)
                {
                    var pr = await BuildSeismicRegisterAndProduction(columnsFields, columnsIndexes, robParameters, fullDateFormat, previousOpFrontId);
                    registersAndProductionsFromFile.Add(pr);
                }
               
               

                var registersAndProductions = new List<KeyValuePair<SeismicRegisterModel, PointProductionModel>>();
                foreach (var point in consideringPoints)
                {
                    var item =
                        registersAndProductionsFromFile.FirstOrDefault(
                            m => m.Value != null && m.Value.PreplotPointId == point.PreplotPointId);
                    if (item.Value != null && item.Key != null)
                        registersAndProductions.Add(item);
                    else
                    {
                        var previousProductionReductionRuleId = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkReductionRuleId(robParameters.SurveyId,
                            point.PreplotPointId, point.PreplotVersionId, point.PreplotPointType, previousOpFrontId);
                        var previousProductionDisplacementRuleId = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkDisplacementRuleId(robParameters.SurveyId,
                                point.PreplotPointId, point.PreplotVersionId, point.PreplotPointType, previousOpFrontId);

                        registersAndProductions.Add(new KeyValuePair<SeismicRegisterModel, PointProductionModel>(null,
                            new PointProductionModel
                            {
                                PreplotPointId = point.PreplotPointId,
                                Line = point.LineName,
                                StationNumber = point.StationNumber,
                                PreplotVersionId = point.PreplotVersionId,
                                SurveyId = robParameters.SurveyId,
                                PreplotPointType = point.PreplotPointType,
                                OperationalFrontId = robParameters.OperationalFrontId,
                                IsActive = true,
                                Status = (int)ProductionStatus.Unaccomplished,
                                FrontGroupId = robParameters.FrontGroupId,
                                FrontGroupLeaderId = robParameters.FrontGroupLeaderId,
                                Date = DateHelper.StringToDate(robParameters.Date),
                                WorkNumber = await _pointProductionRepository.GetMaxWorkNumber(robParameters.SurveyId,
                                    point.PreplotPointId, point.PreplotVersionId, PreplotPointType.ShotPoint,
                                    robParameters.OperationalFrontId) + 1,
                                ReductionRuleId = previousProductionReductionRuleId,
                                DisplacementRuleId = previousProductionDisplacementRuleId
                            }));
                    }
                }
                return registersAndProductions;
            }
        }

        public async Task<KeyValuePair<SeismicRegisterModel, PointProductionModel>> BuildSeismicRegisterAndProduction(string[] robLineFields, BruteRobColumnsIndexes columnsIndexes, BruteRobParametersModel robParameters, string fullDateFormat, int previousOpFrontId)
        {
            try
            {
            
                var seisRegister = new SeismicRegisterModel
                {
                    Ffid = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.FfidIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.FfidIndex]),
                    ShotNumber = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.ShotNumberIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.ShotNumberIndex]),
                    Swath = robLineFields[columnsIndexes.SwathIndex],
                    Itb = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.ItbIndex]) ? new bool?() : Convert.ToBoolean(robLineFields[columnsIndexes.ItbIndex]),
                    Date = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.DateIndex]) ? new DateTime() : DateHelper.StringToDate(robLineFields[columnsIndexes.DateIndex], fullDateFormat),
                    BlasterId = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.BlasterIdIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.BlasterIdIndex]),
                    BlasterShotStatus = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.BlasterShotStatusIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.BlasterShotStatusIndex]),
                    Comment = robLineFields[columnsIndexes.CommentIndex],
                    LineName = robLineFields[columnsIndexes.LineNameIndex],
                    PointNumber = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.PointNumberIndex]) ? 0 : Convert.ToDecimal(robLineFields[columnsIndexes.PointNumberIndex]),
                    NumberOfDeadSeis = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.NbOfDeadSeisIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.NbOfDeadSeisIndex]),
                    UpholeTime = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.UpholeTimeIndex]) ? 0 : Convert.ToDecimal(robLineFields[columnsIndexes.UpholeTimeIndex]),
                    NumberOfLiveSeis = string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.NbOfLiveSeisIndex]) ? 0 : Convert.ToInt32(robLineFields[columnsIndexes.NbOfLiveSeisIndex]),
                    IsActive = true,
                    PreplotPointType = (int)PreplotPointType.ShotPoint,
                    SurveyId = robParameters.SurveyId,
                    OperationalFrontId = robParameters.OperationalFrontId,
                    IsNoiseTest = !string.IsNullOrWhiteSpace(robLineFields[columnsIndexes.IsNoiseTestIndex]) && Convert.ToBoolean(robLineFields[columnsIndexes.IsNoiseTestIndex])
                };
                var preplotPoint = await _preplotPointRepository.GetPreplotPointWithoutCoordinates(robParameters.SurveyId, seisRegister.LineName,
                        seisRegister.PointNumber,
                        PreplotPointType.ShotPoint);
                if (preplotPoint == null) throw new Exception("O ponto " + seisRegister.LineName + "-" + seisRegister.PointNumber + " não existe no preplot atual.");

                seisRegister.PreplotPointId = preplotPoint.PreplotPointId;
                seisRegister.PreplotVersionId = preplotPoint.PreplotVersionId;

                if (seisRegister.IsNoiseTest)
                    return new KeyValuePair<SeismicRegisterModel, PointProductionModel>(seisRegister, null);

                var previousProductionReductionRuleId = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkReductionRuleId(robParameters.SurveyId,
                        preplotPoint.PreplotPointId, preplotPoint.PreplotVersionId, preplotPoint.PreplotPointType, previousOpFrontId);
                var previousProductionDisplacementRuleId = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkDisplacementRuleId(robParameters.SurveyId,
                        preplotPoint.PreplotPointId, preplotPoint.PreplotVersionId, preplotPoint.PreplotPointType, previousOpFrontId);

                var previousProductionHolesDepth = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkHolesDepth(robParameters.SurveyId,
                        preplotPoint.PreplotPointId, preplotPoint.PreplotVersionId, preplotPoint.PreplotPointType, previousOpFrontId);
                if (previousProductionHolesDepth == -1)
                    throw new Exception("A produção de carregamento do PT " + preplotPoint.StationNumber +
                        ", linha " + preplotPoint.LineName + ", não possui a profundidade dos furos. Favor salvar novamente o trecho do carregamento a qual ele pertence.");

                var previousProductionCharges = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkChargesInSp(robParameters.SurveyId,
                        preplotPoint.PreplotPointId, preplotPoint.PreplotVersionId, preplotPoint.PreplotPointType, previousOpFrontId);
                var previousProductionFuses = await _pointProductionRepository.GetPreviousFrontLastProductionOrReworkFusesInSp(robParameters.SurveyId,
                        preplotPoint.PreplotPointId, preplotPoint.PreplotVersionId, preplotPoint.PreplotPointType, previousOpFrontId);

                var production = new PointProductionModel
                {
                    PreplotPointId = preplotPoint.PreplotPointId,
                    Line = preplotPoint.LineName,
                    StationNumber = preplotPoint.StationNumber,
                    PreplotVersionId = preplotPoint.PreplotVersionId,
                    SurveyId = robParameters.SurveyId,
                    PreplotPointType = PreplotPointType.ShotPoint,
                    OperationalFrontId = robParameters.OperationalFrontId,
                    Observation = seisRegister.Comment,
                    Ffid = seisRegister.Ffid,
                    LastEditorUserLogin = "",
                    IsActive = true,
                    Status = (int)ProductionStatus.Accomplished,
                    FrontGroupId = robParameters.FrontGroupId,
                    FrontGroupLeaderId = robParameters.FrontGroupLeaderId,
                    Date = DateHelper.StringToDate(robParameters.Date),
                    ReductionRuleId = previousProductionReductionRuleId,
                    DisplacementRuleId = previousProductionDisplacementRuleId,
                    HolesDepth = previousProductionHolesDepth,
                    NumberOfChargesInShotPoint = previousProductionCharges,
                    NumberOfFusesInShotPoint = previousProductionFuses
                };
                production.WorkNumber = await _pointProductionRepository.GetMaxWorkNumber(robParameters.SurveyId,
                    production.PreplotPointId, production.PreplotVersionId, PreplotPointType.ShotPoint,
                    robParameters.OperationalFrontId) + 1;
                seisRegister.WorkNumber = production.WorkNumber;
                return new KeyValuePair<SeismicRegisterModel, PointProductionModel>(seisRegister, production);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (string.IsNullOrWhiteSpace(msg))
                    msg = "Existem dados inválidos no arquivo importado. Favor verificar novamente.";
                throw new Exception(msg);
            }
        }
    }
}
