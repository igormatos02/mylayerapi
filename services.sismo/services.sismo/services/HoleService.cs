using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
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
    public class HoleService : IHoleService
    {
        private readonly ISRSService _ISRSService;
        private readonly IReductionRuleRepository _reductionRuleRepository;
        private readonly IHolesCoordinatesFileRepository _holesCoordinatesFileRepository;
        private readonly IPointProductionRepository _pointProductionRepository;
        private readonly IPosplotCoordinateRepository _posplotCoordinateRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISRSRepository _ISRSRepository;
        private readonly IHoleCoordinateRepository _holeCoordinateRepository;
        private readonly IConfiguration _configuration;

        public HoleService(
            ISRSService ISRSService,
            IReductionRuleRepository reductionRuleRepository,
            IHolesCoordinatesFileRepository holesCoordinatesFileRepository,
            IPointProductionRepository pointProductionRepository,
            IPosplotCoordinateRepository posplotCoordinateRepository,
            ISurveyRepository surveyRepository,
            ISRSRepository ISRSRepository,
            IHoleCoordinateRepository holeCoordinateRepository,
            IConfiguration configuration)
        {
            _ISRSService = ISRSService;
            _reductionRuleRepository = reductionRuleRepository;
            _holesCoordinatesFileRepository = holesCoordinatesFileRepository;
            _pointProductionRepository = pointProductionRepository;
            _posplotCoordinateRepository = posplotCoordinateRepository;
            _surveyRepository = surveyRepository;
            _ISRSRepository = ISRSRepository;
            _holeCoordinateRepository = holeCoordinateRepository;
            _configuration = configuration;
        }

      
        public async Task<PointsWithHoleModel> ListPointsWithHolesCoordinates(int surveyId, string line, string date, bool onlyWithAlert, PaginationModel pagination)
        {
            
            try
            {
                var datumId = await _surveyRepository.GetSurveyDatumId(surveyId);
                var toSrs = await _ISRSRepository.GetSRS(datumId);
                var holesPerShotPoint = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);

                var dateFormated = string.IsNullOrEmpty(date) ? DateTime.MinValue : DateHelper.StringToDate(date);

                var holesCoordinates = await _holeCoordinateRepository.ListHolesCoordinates(surveyId, dateFormated, line, toSrs.WKT, toSrs.SRSId);
                var groupedHolesCoords = holesCoordinates.GroupBy(m => m.PreplotPointId).ToList();

                var preplotPointsIds = groupedHolesCoords.Where(m => m.Key.HasValue).Select(m => m.Key.Value).ToList();
                var posplotCoords = await _posplotCoordinateRepository.ListPosplotCoordinates(surveyId, preplotPointsIds, toSrs.WKT, toSrs.SRSId);

                var preplotWithPosplotAndHoles = new List<PointWithHolesCoordinatesModel>();

                var prodsWithDisplacementOrReduction =
                    await _pointProductionRepository.ListProductionsWithDisplacementOrReduction(surveyId,
                        OperationalFrontType.Charging);

                foreach (var ghc in groupedHolesCoords)
                {
                    var posplotCoord = posplotCoords.FirstOrDefault(m => m.PreplotPointId == ghc.Key) ?? new PosplotCoordinateModel();
                    var chargingProduction = ghc.Key.HasValue
                        ? prodsWithDisplacementOrReduction.FirstOrDefault(m => m.PreplotPointId == ghc.Key.Value)
                        : new PointProductionModel();
                    var holesCoords = ghc.Select(m => m).ToList();
                    foreach (var s in holesCoords)
                        s.Coordinate = null;
                    preplotWithPosplotAndHoles.Add(new PointWithHolesCoordinatesModel
                    {
                        PreplotPointId = posplotCoord.PreplotPointId,
                        Line = posplotCoord.Line,
                        StationNumber = posplotCoord.StationNumber,
                        DisplacementRule = chargingProduction != null ? chargingProduction.DisplacementRuleName : "",
                        DisplacementRuleImagePath = chargingProduction != null ? chargingProduction.DisplacementRuleImagePath : "",
                        ReductionRule = chargingProduction != null ? chargingProduction.ReductionRuleName : "",
                        ReductionRuleImagePath = chargingProduction != null ? chargingProduction.ReductionRuleImagePath : "",
                        ChargingFrontObservation = chargingProduction != null ? chargingProduction.Observation : "",
                        PosplotCoordinateX = posplotCoord.PosplotCoordinateX,
                        PosplotCoordinateY = posplotCoord.PosplotCoordinateY,
                        PosplotCoordinateZ = posplotCoord.PosplotCoordinateZ,
                        PosplotRegistrationTime = posplotCoord.RegistrationTimeString,
                        HolesCoordinates = holesCoords,
                        Alert = !posplotCoord.PreplotPointId.HasValue
                                ? "PT não encontrado"
                                : holesCoords.Count > holesPerShotPoint
                                    ? "Quantidade de cargas não condiz com os parâmetros do levantamento"
                                    : ""
                    });
                }

                preplotWithPosplotAndHoles = preplotWithPosplotAndHoles.OrderBy(m => m.Line).ThenBy(m => m.StationNumber).ToList();

                if (onlyWithAlert)
                    preplotWithPosplotAndHoles =
                        preplotWithPosplotAndHoles.Where(m => !string.IsNullOrWhiteSpace(m.Alert)).ToList();

                if (pagination != null)
                    preplotWithPosplotAndHoles =
                        preplotWithPosplotAndHoles.Skip((pagination.Page - 1) * pagination.PageSize)
                            .Take(pagination.PageSize)
                            .ToList();

               return new PointsWithHoleModel
               {
                    Points = preplotWithPosplotAndHoles,
                    HolesPerShotPoint = holesPerShotPoint,
                    Datum = toSrs.SRSName,
                    Count = preplotWithPosplotAndHoles.Count
               };
              
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<HolesCoordinatesFileModel>> ListHolesCoordinatesFiles(int surveyId)
        {   
            try
            {  
               return await _holesCoordinatesFileRepository.ListFiles(surveyId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<int>> UpdateHolesCoordinatesWithPosplotPoint(int surveyId, int fileId)
        {
            
            try
            {
                double defaultBufferSize;
                try
                {
                    var numOfHolesPerShotPoint = await _surveyRepository.GetSurveyDefaultHolesQuantityPerShotPoint(surveyId);
                    var distanceBetweenHoles = await _surveyRepository.GetSurveyDefaultDistanceBetweenHoles(surveyId);
                    defaultBufferSize = Convert.ToDouble((numOfHolesPerShotPoint - 1) * distanceBetweenHoles / 2);
                    var distance = await _reductionRuleRepository.GetGreatestDistanceBetweenEndHoles(surveyId);
                    var greatestPossibleBufferSize =Convert.ToDouble(distance);
                    defaultBufferSize = greatestPossibleBufferSize > defaultBufferSize
                        ? greatestPossibleBufferSize
                        : defaultBufferSize;
                    defaultBufferSize = defaultBufferSize *
                                        (1 + (double) await _surveyRepository.GetSurveyHoleBufferSizeFactor(surveyId));
                }
                catch (Exception) { defaultBufferSize = 40; }

                var surveyPoligon = await _surveyRepository.GetSurveyPolygonGeometry(surveyId);
                var polygonCentroidLong = surveyPoligon.Centroid.Coordinate.X;
                SRSModel utmSrs = null;
                if (surveyPoligon.Centroid.Coordinate.CoordinateValue != null)
                    utmSrs = await _ISRSService.GetUTMSirgasSRS(polygonCentroidLong);

              
                var holesEnvelopeBuffer = await _holeCoordinateRepository.GetHolesEnvelopeBufferGeometry(surveyId, fileId, 0.000952098);

                var _surveyShotPointsPosplot = await _posplotCoordinateRepository.ListPosplotCoordinates(
                    surveyId,
                    PreplotPointType.ShotPoint,
                    holesEnvelopeBuffer,
                    utmSrs.WKT, utmSrs.SRSId);

                var surveyShotPointsPosplot = _surveyShotPointsPosplot.Where(m => m.Coordinate != null).ToList();

                var holesCoords = await _holeCoordinateRepository.ListHolesCoordinates(surveyId, fileId, utmSrs.WKT, utmSrs.SRSId);
                var impactedShotPointsIds = new List<int>();
                foreach (var holeCoord in holesCoords)
                {
                    if (holeCoord.Coordinate != null)
                    {
                        var buffer = holeCoord.Coordinate.Buffer(defaultBufferSize);
                        var nearestPoint = surveyShotPointsPosplot
                            .Where(p => p.Coordinate.Within(buffer))
                            .OrderBy(p => p.Coordinate.Distance(holeCoord.Coordinate))
                            .FirstOrDefault();
                        if (nearestPoint != null)
                        {
                            holeCoord.PreplotPointId = nearestPoint.PreplotPointId;
                            holeCoord.Line = nearestPoint.Line;
                            holeCoord.StationNumber = nearestPoint.StationNumber;
                            if (nearestPoint.PreplotPointId.HasValue)
                                impactedShotPointsIds.Add(nearestPoint.PreplotPointId.Value);
                        }
                    }
                }

                await _holeCoordinateRepository.UpdateHolesCoordinates(holesCoords);
                return impactedShotPointsIds;
            }
            catch (Exception ex) { throw ex; }
        }
      


        public  async Task<int> UploadHolesCoordinatesFile(byte[] byteStream, string fileName, int surveyId, int srsId, int userId)
        {
            
            try
            {
                Stream stream = new MemoryStream(byteStream);
                var srs = await _ISRSRepository.GetSRS(srsId);
                var fileSrsWkt = srs.WKT;
                var _dto = new HolesCoordinatesFileModel()
                {
                    FileName = fileName,
                    IsActive = true,
                    SurveyId = surveyId,
                    UploadTime = DateTime.Now,
                    SrsId = srsId
                };

             
                var _fileId = await _holesCoordinatesFileRepository.InsertFile(_dto);

                //using (StreamReader sr = new StreamReader(stream))
                //{
                //    while (sr.Peek() >= 0)
                //    {
                //        try
                //        {
                //            var lin = sr.ReadLine();
                //            if (lin == null) continue;
                //            var fileData = lin.Split(';');

                //            var x = fileData[0];
                //            var y = fileData[1];
                //            var z = fileData[2];
                //            var time = fileData[3];
                //            DateTime timeconverted = _dto.UploadTime;
                //            try
                //            {
                //                timeconverted = Convert.ToDateTime(time);
                //            }
                //            catch { }

                //            await _holeCoordinateRepository.AddHoleCoordinate(new HoleCoordinateModel()
                //            {
                //                SurveyId = surveyId,
                //                AcquisitionTime = timeconverted,
                //                CreatorUserId = userId,
                //                FileId = _fileId,
                //                Coordinate = srsId != 4326 ? //Sempre salva em WGS84 - srid 4326
                //                    Functions.ConvertPointCoordinateToWgs84(DbGeometry.PointFromText(string.Format("POINT({0} {1} {2})", x.Replace(",", "."), y.Replace(",", "."), z.Replace(",", ".")), srsId), fileSrsWkt) :
                //                    DbGeometry.PointFromText(string.Format("POINT({0} {1} {2})", x.Replace(",", "."), y.Replace(",", "."), z.Replace(",", ".")), srsId),
                //                //{ z.Replace(",", ".") }
                //                IsActive = true
                //            });

                //            result.Count++;
                //        }
                //        catch (Exception e)
                //        {
                //            _holeCoordDal.DeleteHolesCoordinates(surveyId, _fileId);
                //            _holeCoordFileDal.DeleteFile(surveyId, _fileId); //SE falhou, deleta a entrada na tabela HoleCoordinatesFile
                //            throw new Exception("Erro ao ler ou converter os dados do arquivo de coordenadas.");
                //        }
                //    }
                //}
                return _fileId;
            }
    catch (Exception ex) { throw ex; }
}

        public async Task<bool> DeleteHolesCoordinatesFile(int surveyId, int fileId)
        {
            
            try
            {
                await _holeCoordinateRepository.DeleteHolesCoordinates(surveyId, fileId);
                await _holesCoordinatesFileRepository.DeleteFile(surveyId, fileId);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> DeleteHoleCoordinate(int surveyId, int holeCoordinateId)
        {
            
            try
            {
                var deletionResult = await _holeCoordinateRepository.DeleteHoleCoordinate(surveyId, holeCoordinateId);
                if (deletionResult) return true;
                else throw new Exception("As coordenadas a serem excluídas não foram encontradas na base de dados. Tente filtrar a busca novamente, por favor.");
            }
            catch (Exception ex) { throw ex; }
        }
       
    }
}
