using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
   
    public class PosplotCoordinateRepository : IPosplotCoordinateRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PosplotCoordinateRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task InsertPosplotCoordinate(PosplotCoordinateModel posplotCoordinate)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PosplotCoordinates.Where(
                    m => m.SurveyId == posplotCoordinate.SurveyId
                        && m.OperationalFrontId == posplotCoordinate.OperationalFrontId
                        && m.LinePreplot == posplotCoordinate.LinePreplot
                        && m.StationNumber == posplotCoordinate.StationNumber
                        && m.PreplotPointId == posplotCoordinate.PreplotPointId
                    ).FirstOrDefaultAsync();
            if (entity != null)
            {
                if (entity.Coordinate.Coordinate.X != posplotCoordinate.Coordinate.Coordinate.X
                        || entity.Coordinate.Coordinate.Y != posplotCoordinate.Coordinate.Coordinate.Y
                        || entity.Coordinate.Coordinate.Y != posplotCoordinate.Coordinate.Coordinate.Z
                        || entity.Line != posplotCoordinate.Line
                        )
                {
                    entity.Coordinate = posplotCoordinate.Coordinate;
                    entity.Line = posplotCoordinate.Line;
                    entity.StationNumberPreplot = posplotCoordinate.StationNumberPreplot;

                   
                }
            }
            else {
                context.PosplotCoordinates.Add(posplotCoordinate.ToEntity());
            }

            context.SaveChanges();
        }

        public async Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, IList<int> preplotPointsIds, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PosplotCoordinates.Where(m => m.SurveyId == surveyId && m.PreplotPointId.HasValue &&
               preplotPointsIds.Contains(m.PreplotPointId.Value)).ToListAsync();

            return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
                
        }

        public async Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, PreplotPointType pointsType, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = (pointsType == PreplotPointType.All) ?
               await context.PosplotCoordinates.Where(m => m.SurveyId == surveyId).ToListAsync() :
               await context.PosplotCoordinates.Where(m => m.SurveyId == surveyId && m.PreplotPointId.HasValue &&
                    m.Survey.PreplotPoints.Where(p => p.PreplotPointType == (int)pointsType).Select(p => p.PreplotPointId).Contains(m.PreplotPointId.Value)).ToListAsync();
            
            return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }

        public async Task<List<PosplotCoordinateModel>> ListPosplotCoordinates(int surveyId, PreplotPointType pointsType, Geometry containerBuffer, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = (pointsType == PreplotPointType.All) ?
                 await context.PosplotCoordinates.Where(m => m.SurveyId == surveyId &&
                    m.Coordinate.Within(containerBuffer)).ToListAsync() :
                await context.PosplotCoordinates.Where(m => m.SurveyId == surveyId && m.PreplotPointId.HasValue &&
                    m.Survey.PreplotPoints.Where(p => p.PreplotPointType == (int)pointsType).Select(p => p.PreplotPointId).Contains(m.PreplotPointId.Value) &&
                    m.Coordinate.Within(containerBuffer)).ToListAsync();

            return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }
        public async Task<List<PosplotCoordinateModel>> ListPosplotCompare(Int32 surveyId, Int32 type, String line, Int32 operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            
            List<PosplotCoordinateModel> items = new List<PosplotCoordinateModel>();
            try
            {
                var parameters = new[] {
                      new SqlParameter("@surveyId", surveyId),
                      new SqlParameter("@type", type),
                      new SqlParameter("@line", line),
                      new SqlParameter("@operationalFrontId", operationalFrontId)
                };
               items = await context.Set<PosplotCoordinateModel>().FromSqlRaw("execute [ListPosplotCompare] @surveyId, @type, @line, @operationalFrontId", parameters).ToListAsync();
              

                for (Int32 i = 0; i < items.Count(); i++)
                {
                    var item = items[i];

                    item.PreplotCoordinateX = item.PreplotCoordinate.Coordinate.X.ToString();
                    item.PreplotCoordinateY = item.PreplotCoordinate.Coordinate.Y.ToString();
                    item.PosplotCoordinateX = item.PosplotCoordinate.Coordinate.X.ToString();
                    item.PosplotCoordinateY = item.PosplotCoordinate.Coordinate.Y.ToString();
                    item.PosplotCoordinateZ = item.PosplotCoordinate.Coordinate.Z.ToString();
                    double? deltaN = 0;
                    double? deltaE = 0;
                    double? distance = 0;
                    double? locationError = 0;
                    if (item.PosplotCoordinateX != "0")
                    {
                        locationError = Math.Sqrt(
                                    Math.Pow(
                                        Convert.ToDouble(item.PosplotCoordinateX) -
                                        Convert.ToDouble(item.PreplotCoordinateX), 2)
                                    +
                                    Math.Pow(
                                        Convert.ToDouble(item.PosplotCoordinateY) -
                                        Convert.ToDouble(item.PreplotCoordinateY), 2)
                                    );

                        deltaN = item.PreplotCoordinate.Coordinate.Y - item.PosplotCoordinate.Coordinate.Y;
                        deltaE = item.PreplotCoordinate.Coordinate.X - item.PosplotCoordinate.Coordinate.X;
                    }

                    if (i > 0 && items[i - 1] != null && items[i - 1].PosplotCoordinateZ != "" && item.PosplotCoordinateZ != "")
                    {
                        var itemAnterior = items[i - 1];
                        var zCoord = Convert.ToDouble(item.PosplotCoordinateZ);
                        item.AdjacentPointNumber = itemAnterior.StationNumber;
                        var adjZCoord = Convert.ToDouble(itemAnterior.PosplotCoordinateZ);
                        item.AltimetryVariationMeters = zCoord - adjZCoord;
                        item.AltimetryVariationDegrees = Math.Abs(Math.Asin(
                        item.AltimetryVariationMeters /
                        Math.Sqrt(Math.Pow(
                            Convert.ToDouble(item.PosplotCoordinateX) -
                            Convert.ToDouble(itemAnterior.PosplotCoordinateX), 2)
                                    +
                                    Math.Pow(
                                        Convert.ToDouble(item.PosplotCoordinateY) -
                                        Convert.ToDouble(itemAnterior.PosplotCoordinateY), 2)
                            )
                        ));

                        distance = Math.Sqrt(
                                   Math.Pow(
                                       Convert.ToDouble(item.PosplotCoordinateX) -
                                       Convert.ToDouble(itemAnterior.PosplotCoordinateX), 2)
                                   +
                                   Math.Pow(
                                       Convert.ToDouble(item.PosplotCoordinateY) -
                                       Convert.ToDouble(itemAnterior.PosplotCoordinateY), 2)
                                   );
                    }
                    if (item.PosplotCoordinateZ != "")
                    {
                        if (item.AltimetryVariationDegrees <= 12)
                            item.ReceiversArrangement = "Arranjo Normal";
                        else if (item.AltimetryVariationDegrees > 12 && item.AltimetryVariationDegrees <= 24)
                            item.ReceiversArrangement = "Arranjo Encurtado";
                        else item.ReceiversArrangement = "Arranjo Agrupado";
                    }
                    if (item.StatusId == null)
                        item.Status = "Não Produzido";
                    else if (item.StatusId == -1)
                        item.Status = "Não Materializado";
                    else item.Status = "Materializado";

                    item.PreplotCoordinateX = string.Format("{0:0.00}", item.PreplotCoordinate.Coordinate.X);
                    item.PreplotCoordinateY = string.Format("{0:0.00}", item.PreplotCoordinate.Coordinate.Y);
                    item.PosplotCoordinateX = string.Format("{0:0.00}", item.PosplotCoordinate.Coordinate.X);
                    item.PosplotCoordinateY = string.Format("{0:0.00}", item.PosplotCoordinate.Coordinate.Y);
                    item.PosplotCoordinateZ = string.Format("{0:0.00}", item.PosplotCoordinate.Coordinate.Z);
                    item.Distance = string.Format("{0:0.00}", distance);
                    item.DeltaN = string.Format("{0:0.00}", deltaN);
                    item.DeltaE = string.Format("{0:0.00}", deltaE);
                    item.PosplotCoordinateZ = item.PosplotCoordinateZ == "" ? "" : item.PosplotCoordinateZ;
                    item.LocationDistance = string.Format("{0:0.00000}", locationError);
                    item.AltimetryVariationMetersStr = string.Format("{0:0.00000}", item.AltimetryVariationMeters);
                    item.AltimetryVariationDegreesStr = string.Format("{0:0.00000}", item.AltimetryVariationDegrees);
                    if (item.PosplotDate != null)
                        item.Date = item.PosplotDate.Value.Day.ToString() + "/" + item.PosplotDate.Value.Month.ToString() + "/" + item.PosplotDate.Value.Year.ToString();
                    else item.Date = "";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return items;
        }
        public async Task DropPosplot(Int32 surveyId, Int32 preplotPointType)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@preplotPointType", preplotPointType)
                };
                context.Database.SetCommandTimeout(0);
                var models = await context.Database.ExecuteSqlRawAsync("EXEC DropPosplot @surveyId,@preplotPointType", parameters);
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
