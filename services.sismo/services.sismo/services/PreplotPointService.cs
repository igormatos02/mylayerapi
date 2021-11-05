using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
   

    public class PreplotPointService : IPreplotPointService
    {
        private readonly IPosplotCoordinateRepository _posplotCoordinateRepository;
        private readonly ISwathRepository _swathRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISRSRepository _SRSRepository;
        private readonly IOperationalFrontRepository _operationalFrontRepository;
        private readonly IPreplotVersionService _preplotVersionService;
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly IConfiguration _configuration;

        public PreplotPointService(
            IPosplotCoordinateRepository posplotCoordinateRepositor,
            ISwathRepository swathRepository,
            ISurveyRepository surveyRepository,
            ISRSRepository SRSRepository,
            IOperationalFrontRepository operationalFrontRepository,
            IPreplotVersionService preplotVersionService,
            IPreplotPointRepository preplotPointRepository,
            IConfiguration configuration)
        {
            _posplotCoordinateRepository = posplotCoordinateRepositor;
            _swathRepository = swathRepository;
            _surveyRepository = surveyRepository;
            _SRSRepository = SRSRepository;
            _operationalFrontRepository = operationalFrontRepository;
            _preplotVersionService = preplotVersionService;
            _preplotPointRepository = preplotPointRepository;
            _configuration = configuration;
        }

        //public async Task<string> SavePreplotGPSeismic(int surveyId, string userLogin, string comment, int inputType)
        //{ 
        //    try
        //    {
        //        var error = await _preplotPointRepository.InsertPreplotPoints(surveyId, userLogin, comment, (PreplotPointType)inputType);
        //        if (error == "")
        //            return "Pre-plot Inserido com sucesso!";
        //        else throw new Exception(error);
        //    }
        //    catch (Exception ex) { throw ex; }
        //}
        public Task<string> SavePreplotGPSeismic(int surveyId, string userLogin, string comment, int inputType)
        {
            throw new NotImplementedException();
        }

        public async Task<PreplotPointModel> GetPreplotPoint(int surveyId, PreplotPointType preplotPointType, int preplotPointVersionId,
            int preplotPointId)
        {   
            try
            {
                return await _preplotPointRepository.GetPreplotPoint(surveyId, preplotPointType, preplotPointVersionId, preplotPointId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<PointDetailModel>> GetPointDetail(int surveyId, int preplotVersionId, int preplotPointId)
        {
            
            try
            {

                return await _preplotPointRepository.GetPointDetail(surveyId, preplotVersionId, preplotPointId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<VersionAndTypesModel> ListVersionsTypesLines(int surveyId, int pointType)
        {
            

            var existingTypes =  await _preplotPointRepository.ListExistingPreplotPointTypes(surveyId);
            var preplotPointTypes = from PreplotPointType type in existingTypes.ToList()
                                    where type != 0
                                    select new VersionTypeModel{ Id = (int)type, Name = EnumHelper.GetEnumDescription(type) };

            var versions = await _preplotVersionService.ListPreplotVersions(surveyId);

            return new VersionAndTypesModel
            {
                Versions = versions.ToList(),
                PreplotPointTypes = preplotPointTypes.ToList()
            };

           
        }

        public async Task<List<PreplotVersionModel>>ListPreplotVersions(int surveyId)
        {
            
            try
            {
                return await _preplotVersionService.ListPreplotVersions(surveyId);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<List<string>> ListLines(Int32 surveyId, Int32 preplotPointType)
        {
            
            try
            {
                return await _preplotPointRepository.ListLines(surveyId, (PreplotPointType)preplotPointType);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<List<string>> ListLinesForOperationalFront(Int32 surveyId, Int32 operationalFrontId)
        {
            
            try
            {
                var type = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);
                var pointType = GetPreplotPointTypeByOpFrontType(type);
                return await _preplotPointRepository.ListLines(surveyId, pointType);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<object> ListLinesForAutocomplete(Int32 surveyId, Int32 preplotPointType, string key)
        {
            
            try
            {
                var lines = await _preplotPointRepository.ListLines(surveyId, (PreplotPointType)preplotPointType, key);
                var lineObjects = new List<object>();
                foreach (var l in lines)
                    lineObjects.Add(new { LineName = l });

                return lineObjects;
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<List<string>> ListLinesByVersion(Int32 surveyId, Int32 preplotPointType, int preplotVersionId)
        {
            
            try
            {
                return await _preplotPointRepository.ListLinesByVersion(surveyId, (PreplotPointType)preplotPointType, preplotVersionId);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<List<decimal>> ListPointsFromLine(Int32 surveyId, Int32 preplotPointType, String line)
        {
            
            try
            {
              return await _preplotPointRepository.ListStationNumbers(surveyId, (PreplotPointType)preplotPointType,
                    line);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<decimal>> ListPointsFromLineForOperationalFront(Int32 surveyId, Int32 operationalFrontId, String line)
        {
            
            try
            {
                var type = await _operationalFrontRepository.GetOperationalFrontType(operationalFrontId);
                var pointType = GetPreplotPointTypeByOpFrontType(type);
                return await _preplotPointRepository.ListStationNumbers(surveyId, pointType, line);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<PreplotPointListWithSrs> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId,
            string line, PaginationModel pagination, bool exportCsv)
        {
            
            try
            {
                if (preplotVersionId == -1)
                    preplotVersionId = await _preplotVersionService.GetLastPreplotVersion(surveyId);

                if (string.IsNullOrWhiteSpace(line)) line = "";
                if (pagination != null)
                {
                    if (string.IsNullOrWhiteSpace(pagination.SortCollumns)) pagination.SortCollumns = "Line ASC";
                }

                var datum = await _surveyRepository.GetSurveyDatumId(surveyId);
                var toSrs = await _SRSRepository.GetSRS(datum);


                var _preplotPoints = await _preplotPointRepository.ListPreplotPoints(surveyId, pointType, preplotVersionId, line, pagination, toSrs.WKT, toSrs.SRSId);
                var preplotPoints = _preplotPoints.OrderBy(m => m.LineName).ThenBy(m => m.StationNumber).ToList();
                var posplotCoordinates = await ListAndCalculatePreplotWithPosplotCoordinates(pagination, surveyId, preplotPoints, toSrs);
                List<PreplotAndPosplotDataModel> preplotWithPosplotList = posplotCoordinates.ToList();

                if (!exportCsv)
                {
                    var count = await _preplotPointRepository.CountPreplotPoints(surveyId, pointType, preplotVersionId, line);
                    return new PreplotPointListWithSrs { Points = preplotWithPosplotList, SrsName = toSrs.SRSName, Count = count };

                }
                else
                {
                    return new PreplotPointListWithSrs { Coordinates =  BuildCoordinatesReportCsvString(preplotWithPosplotList) };
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<PreplotPointListWithSrs> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId,
            int swathNumber, PaginationModel pagination, bool exportCsv)
        {
            
            try
            {
                if (preplotVersionId == -1)
                    preplotVersionId = await _preplotVersionService.GetLastPreplotVersion(surveyId);

                if (string.IsNullOrWhiteSpace(pagination.SortCollumns)) pagination.SortCollumns = "Line ASC";

                var datum = await _surveyRepository.GetSurveyDatumId(surveyId);
                var toSrs = await _SRSRepository.GetSRS(datum);

                var swath = await _swathRepository.GetSwath(surveyId, swathNumber);

                long countTotal = 0;
                var _preplotPoints = await _preplotPointRepository.ListPreplotPoints(surveyId, preplotVersionId, pointType, swath.InitialShotPoint, swath.FinalShotPoint,
                    Convert.ToInt32(swath.InitialReceiverLine), Convert.ToInt32(swath.FinalReceiverLine), pagination, toSrs.WKT, toSrs.SRSId, countTotal);

                var preplotPoints = _preplotPoints.OrderBy(m => m.LineName).ThenBy(m => m.StationNumber).ToList();

                var preplotWithPosplotList = await ListAndCalculatePreplotWithPosplotCoordinates(pagination, surveyId, preplotPoints, toSrs);

                if (!exportCsv)
                {
                    return new PreplotPointListWithSrs { Points = preplotWithPosplotList.ToList(), SrsName = toSrs.SRSName, Count = countTotal };
                   
                }
                else
                {
                    return new PreplotPointListWithSrs { Coordinates = BuildCoordinatesReportCsvString(preplotWithPosplotList) };
                }
            }
             catch (Exception ex) { throw ex; }
        }

        private async Task<IList<PreplotAndPosplotDataModel>> ListAndCalculatePreplotWithPosplotCoordinates(PaginationModel pagination, int surveyId, IList<PreplotPointModel> preplotPoints, SRSModel toSrs)
        {
            if (pagination != null)
                if (string.IsNullOrEmpty(pagination.SortCollumns)) pagination.SortCollumns = "Line ASC";

            var posplotCoords = await _posplotCoordinateRepository.ListPosplotCoordinates(surveyId, preplotPoints.Select(m => m.PreplotPointId).ToList(), toSrs.WKT, toSrs.SRSId);

            var preplotWithPosplotList = new List<PreplotAndPosplotDataModel>();


            for (var i = 0; i < preplotPoints.Count; i++)
            {
                var posplotCoord =
                    posplotCoords.Where(m => m.PreplotPointId == preplotPoints[i].PreplotPointId).OrderByDescending(m => m.RegistrationTime).FirstOrDefault();
                if (posplotCoord != null)
                {
                    try
                    {
                        posplotCoord.LocationError = Math.Sqrt(
                            Math.Pow(
                                Convert.ToDouble(posplotCoord.PosplotCoordinateX) -
                                Convert.ToDouble(preplotPoints[i].PreplotCoordinateX), 2)
                            +
                            Math.Pow(
                                Convert.ToDouble(posplotCoord.PosplotCoordinateY) -
                                Convert.ToDouble(preplotPoints[i].PreplotCoordinateY), 2)
                            );
                    }
                    catch
                    {
                    }
                    //posplotCoord.Coordinate.Distance(preplotPoints[i].PreplotCoordinate);
                    if (i > 0 && preplotPoints[i - 1] != null)
                    {
                        try
                        {
                            var zCoord = Convert.ToDouble(posplotCoord.PosplotCoordinateZ);
                            posplotCoord.AdjacentPointNumber = preplotPoints[i - 1].StationNumber;
                            var AdjacentCoord =
                                posplotCoords.Where(m => m.PreplotPointId == preplotPoints[i - 1].PreplotPointId)
                                    .FirstOrDefault();
                            var adjZCoord = Convert.ToDouble(AdjacentCoord.PosplotCoordinateZ);
                            posplotCoord.AltimetryVariationMeters = zCoord - adjZCoord;
                            posplotCoord.AltimetryVariationDegrees = Math.Abs(Math.Asin(
                                posplotCoord.AltimetryVariationMeters /
                                Math.Sqrt(Math.Pow(
                                    Convert.ToDouble(posplotCoord.PosplotCoordinateX) -
                                    Convert.ToDouble(AdjacentCoord.PosplotCoordinateX), 2)
                                          +
                                          Math.Pow(
                                              Convert.ToDouble(posplotCoord.PosplotCoordinateY) -
                                              Convert.ToDouble(AdjacentCoord.PosplotCoordinateY), 2)
                                    )
                                ));
                        }
                        catch
                        {
                        }
                    }
                    posplotCoord.Coordinate = null;
                }
                preplotWithPosplotList.Add(new PreplotAndPosplotDataModel { PreplotPoint = preplotPoints[i], PosplotCoord = posplotCoord });
            }
            return preplotWithPosplotList;
        }

        private string BuildCoordinatesReportCsvString(IList<PreplotAndPosplotDataModel> preplotWithPosplotList)
        {
            string csv = string.Empty;
            //Add the Header row for CSV file.
            csv += "\uFEFFLinha;Estaca;Tipo;Pré-plote Coord. X;Pré-plote Coord. Y;Pós-plote Coord. X;Pós-plote Coord. Y;Altimetria (m); Erro de Locação (m); Variação Altimétrica (m); Variação Altimétrica (°);Regra Espalhamento Recomendada";

            foreach (var point in preplotWithPosplotList)
            {
                //Add new line
                csv += "\r\n";
                csv += point.PreplotPoint.LineName + ';' +
                    point.PreplotPoint.StationNumber + ';' +
                    point.PreplotPoint.TypeName + ';' +
                    point.PreplotPoint.PreplotCoordinateX + ';' +
                    point.PreplotPoint.PreplotCoordinateY + ';' +
                    point.PosplotCoord.PosplotCoordinateX + ';' +
                    point.PosplotCoord.PosplotCoordinateY + ';' +
                    point.PosplotCoord.PosplotCoordinateZ + ';' +
                    point.PosplotCoord.LocationError + ';' +
                    point.PosplotCoord.AltimetryVariationMeters + ';' +
                    point.PosplotCoord.AltimetryVariationDegrees + ';' +
                    point.PosplotCoord.ReceiversArrangement;
            }

            return csv;
        }

        public PreplotPointType GetPreplotPointTypeByOpFrontType(OperationalFrontType operationalFrontType)
        {
            switch (operationalFrontType)
            {
                case OperationalFrontType.Permit:
                    return PreplotPointType.All;
                case OperationalFrontType.Topography:
                    return PreplotPointType.All;
                case OperationalFrontType.Drilling:
                    return PreplotPointType.ShotPoint;
                case OperationalFrontType.Charging:
                    return PreplotPointType.ShotPoint;
                case OperationalFrontType.SeismoA:
                    return PreplotPointType.ReceiverStation;
                case OperationalFrontType.Detonation:
                    return PreplotPointType.ShotPoint;
                case OperationalFrontType.SeismoB:
                    return PreplotPointType.ReceiverStation;
                case OperationalFrontType.Inspection:
                    return PreplotPointType.All;
                case OperationalFrontType.Magnetometry:
                    return PreplotPointType.MagnometricStation;
                case OperationalFrontType.Gravimetry:
                    return PreplotPointType.GravimetricStation;
                default:
                    return PreplotPointType.All;
            }
        }

      
    }
}
