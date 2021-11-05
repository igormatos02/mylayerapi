using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using LINQExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    public class PreplotPointRepository: IPreplotPointRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PreplotPointRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PreplotPointModel> GetPreplotPoint(int surveyId, PreplotPointType pointType, int preplotVersionId,
          int preplotPointId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotPoints.Where(
                    m =>
                        m.SurveyId == surveyId && m.PreplotPointType == (int)pointType &&
                        m.PreplotVersionId == preplotVersionId && m.PreplotPointId == preplotPointId).FirstOrDefaultAsync();
            return entity.ToModel();
        }



        public async Task<Boolean> ArePointsConnected(int surveyId, string lineNumber,
            decimal stationNumber1, decimal stationNumber2, PreplotPointType consideringPointsType)
        {
            using var context = _contextFactory.CreateDbContext();
            if (stationNumber1 == stationNumber2)
                return false;
            var pointTypes = PreplotTypeResolver.Resolve(consideringPointsType);
            var nextPoint = await context.PreplotPoints.Where(m =>
                     m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.Line == lineNumber
                     && m.StationNumber > stationNumber1
                     ).OrderBy(m => m.StationNumber).FirstOrDefaultAsync();

            return nextPoint != null && stationNumber2 == nextPoint.StationNumber;

        }

        public async Task<PreplotPointModel> GetPreplotPoint(int surveyId, string lineNumber, decimal pointNumber, PreplotPointType pointType, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotPoints.Where(m =>
                        m.SurveyId == surveyId
                        && m.PreplotPointType == (int)pointType
                        && m.Line == lineNumber
                        && m.StationNumber == pointNumber
                        && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == lineNumber ).Max(f => f.PreplotVersionId))).FirstOrDefaultAsync();

            return entity.ToModel(toWkt, toSrid);
        }

        public async Task<List<PreplotPointModel>> GetPreplotPointsByLabel(int surveyId, string lineNumber, decimal pointNumber)
        {
            using var context = _contextFactory.CreateDbContext();
            var points = await context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && m.IsActive
                       && m.StationNumber == pointNumber
                       && m.Line == lineNumber
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                                               ).ToListAsync();

            return points.Select(x => x.ToModel()).ToList();
        }

        public async Task<int?> GetPreplotPointId(int surveyId, string lineNumber, decimal pointNumber, int preplotPointType)
        {
            using var context = _contextFactory.CreateDbContext();
            var point = await context.PreplotPoints.Where(m => m.SurveyId == surveyId
                        && m.Line == lineNumber
                        && m.StationNumber == pointNumber
                        && m.PreplotPointType == preplotPointType).FirstOrDefaultAsync();
            int? id = point.PreplotPointId;
            return id;// > -1 ? id : 
        }

        public async Task<int> GetMaxPreplotPointId(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            int maxId = 0;
            maxId = await context.PreplotPoints.Where(x => x.SurveyId == surveyId).MaxAsync(f => f.PreplotPointId);
            return maxId;

        }
        public async Task<PreplotPointModel> GetPreplotPointWithoutCoordinates(int surveyId, string lineNumber, decimal pointNumber, PreplotPointType pointType)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotPoints.Where(m =>
                        m.SurveyId == surveyId
                        && m.PreplotPointType == (int)pointType
                        && m.Line == lineNumber
                        && m.IsActive
                        && m.StationNumber == pointNumber
                        && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == lineNumber
                                                    ).Max(f => f.PreplotVersionId))).FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId, string line, PaginationModel pagination, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var hasLineFilter = true;
            if (string.IsNullOrWhiteSpace(line)) hasLineFilter = false;
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            Expression<Func<PreplotPoint, bool>> whereClause =
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && (m.Line == line || !hasLineFilter)
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && (x.Line == line || !hasLineFilter)
                                                        && x.PreplotVersionId <= preplotVersionId
                                                    ).Max(f => f.PreplotVersionId));

            if (pagination != null)
            {
                var entities =  string.IsNullOrEmpty(pagination.SortCollumns) ?
                     await context.PreplotPoints.Where(whereClause).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync() :
                     await context.PreplotPoints.Where(whereClause).OrderUsingSortExpression(pagination.SortCollumns).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
                    return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
            }
            else { 
               
                var entities = await context.PreplotPoints.Where(whereClause).ToListAsync();
                return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
            }
        }
       
        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, int preplotVersionId, PreplotPointType pointType, decimal initialShotPoint, decimal finalShotPoint,
            int initialReceiverLine, int finalReceiverLine, PaginationModel pagination, string toWkt, int toSrid, long countTotal)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            Expression<Func<PreplotPoint, bool>> whereClause =
                       m => m.SurveyId == surveyId
                       && ((m.PreplotPointType == (int)PreplotPointType.ShotPoint && m.StationNumber >= initialShotPoint && m.StationNumber <= finalShotPoint) ||
                          (m.PreplotPointType == (int)PreplotPointType.ReceiverStation && Convert.ToInt32(m.Line) >= initialReceiverLine && Convert.ToInt32(m.Line) <= finalReceiverLine))
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.PreplotVersionId <= preplotVersionId
                                                    ).Max(f => f.PreplotVersionId));

           
            if (pagination != null)
            {
                var entities = string.IsNullOrEmpty(pagination.SortCollumns) ?
                     await context.PreplotPoints.Where(whereClause).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync() :
                     await context.PreplotPoints.Where(whereClause).OrderUsingSortExpression(pagination.SortCollumns).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
                countTotal = entities.Count();
                return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
            }
            else
            {
                var entities = await context.PreplotPoints.Where(whereClause).ToListAsync();
                countTotal = entities.Count();
                return entities.Select(x => x.ToModel(toWkt, toSrid)).ToList();
            }
        }

        public async Task<List<int>> ListPreplotPointsIds(int surveyId, PreplotPointType pointType, int preplotVersionId, string line, PaginationModel pagination)
        {
            using var context = _contextFactory.CreateDbContext();
            if (line == null) line = "";
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            Expression<Func<PreplotPoint, bool>> whereClause =
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line == line
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == line
                                                        && x.PreplotVersionId <= preplotVersionId
                                                    ).Max(f => f.PreplotVersionId));

            if (pagination != null)
            {
                var entities = string.IsNullOrEmpty(pagination.SortCollumns) ?
                     await context.PreplotPoints.Where(whereClause).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync() :
                     await context.PreplotPoints.Where(whereClause).OrderUsingSortExpression(pagination.SortCollumns).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
                return entities.Select(x => x.PreplotPointId).ToList();
            }
            else
            {
                var entities = await context.PreplotPoints.Where(whereClause).ToListAsync();
                return entities.Select(x => x.PreplotPointId).ToList();
            }


        }
        public async Task<long> CountPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId, string line)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            Expression<Func<PreplotPoint, bool>> whereClause =
                          m => m.SurveyId == surveyId
                      && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line == line
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == line
                                                        && x.PreplotVersionId <= preplotVersionId
                                                    ).Max(f => f.PreplotVersionId));
            return await context.PreplotPoints.Where(whereClause).CountAsync();
        }

        public async Task<List<String>> ListLines(int surveyId, PreplotPointType pointType)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var qry = from b in  context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive == true
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                                               )
                      select new
                      {
                          b.Line
                      };
            var lines = await qry.Distinct().OrderBy(l => l.Line).ToListAsync();
            return lines.Select(x => x.Line).OrderBy(s => Convert.ToInt32(s)).ToList(); 
        }

        public async Task<List<String>> ListLines(int surveyId, PreplotPointType pointType, string key)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var qry = from b in context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line.StartsWith(key)
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                                               )
                      select new
                      {
                          b.Line
                      };
            var lines = await qry.Distinct().OrderBy(l => l.Line).ToListAsync();
            return lines.Select(x => x.Line).ToList(); 
        }

        public async Task<List<String>> ListLinesByVersion(int surveyId, PreplotPointType pointType, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var qry = from b in  context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive == true
                       && m.PreplotVersionId == preplotVersionId)
                      select new
                      {
                          b.Line
                      };
            var lines = await qry.Distinct().OrderBy(l => l.Line).ToListAsync();
            return lines.Select(x => x.Line).ToList(); 
        }

        public async Task<List<PreplotPointType>> ListExistingPreplotPointTypes(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var qry = from b in  context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && m.IsActive == true
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                                               )
                      select new
                      {
                          b.PreplotPointType
                      };
            var types = await qry.Distinct().Select(x=>(PreplotPointType)x.PreplotPointType).ToListAsync();
            return types.ToList();
        }

        public async Task<List<Decimal>> ListStationNumbers(int surveyId, PreplotPointType pointType, string line)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var qry = from b in  context.PreplotPoints.Where(
                       m => m.SurveyId == surveyId && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line == line
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == line
                                                    ).Max(f => f.PreplotVersionId))
                                               )
                      select new
                      {
                          b.StationNumber
                      };
            var lines = await qry.Distinct().OrderBy(l => l.StationNumber).ToListAsync();
            return lines.Select(x => x.StationNumber).ToList(); ;
        }

        public async Task<List<Decimal>> ListStationNumbers(int surveyId, PreplotPointType pointType, string line, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var qry = from b in  context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line == line
                       && m.StationNumber >= initialStation
                       && m.StationNumber <= finalStation
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == line
                                                    ).Max(f => f.PreplotVersionId))
                                               )
                      select new
                      {
                          b.StationNumber
                      };
            var lines = await qry.Distinct().OrderBy(l => l.StationNumber).ToListAsync();
            return lines.Select(x => x.StationNumber).ToList(); 
        }

        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                                               ).ToListAsync();

            return points.Select(x=>x.ToModel( toWkt, toSrid)).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, int landId, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var points = await context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && m.LandId == landId
                       && m.IsActive
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                    ).Max(f => f.PreplotVersionId))
                                               ).ToListAsync();

            return points.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string line, decimal initialStation, decimal finalStation, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.IsActive == true
                     && m.Line == line
                     && m.StationNumber >= initialStation
                     && m.StationNumber <= finalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                                  && x.Line == line
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).ToListAsync();

            return points.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType, string line, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && (pointType == PreplotPointType.All || m.PreplotPointType == (int)pointType)
                     && m.IsActive == true
                     && m.Line == line
                     && m.StationNumber >= initialStation
                     && m.StationNumber <= finalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                                  && x.Line == m.Line
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).ToListAsync();

            return points.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType, List<LineStretchModel> lineStretches)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = new List<PreplotPoint>();
            var entities = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.IsActive).ToListAsync();

            var lines = lineStretches.Select(stretch => entities.Where(
                m =>
                     m.Line == stretch.Line
                     && m.StationNumber >= stretch.InitialStation
                     && m.StationNumber <= stretch.FinalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                                  && x.Line == stretch.Line
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).ToList());

            foreach (var stretchPoints in lines)
            {
                points.AddRange(stretchPoints);
            }
            return points.Select(x => x.ToModel()).ToList();
        }
            public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, decimal initialStation, decimal finalStation, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.IsActive
                     && m.StationNumber >= initialStation
                     && m.StationNumber <= finalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).ToListAsync();


            return points.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPointsWithoutCoordinates(int surveyId, PreplotPointType pointType, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.IsActive
                     && m.StationNumber >= initialStation
                     && m.StationNumber <= finalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).ToListAsync();


            return points.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PreplotPointModel>> ListPreplotPoints(int surveyId, PreplotPointType pointType, string line, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                          m => m.SurveyId == surveyId
                       && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                       && m.IsActive
                       && m.Line == line
                       && m.PreplotVersionId == (
                                                 context.PreplotPoints.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.Line == line
                                                    ).Max(f => f.PreplotVersionId))
                                               ).ToListAsync();

            return points.Select(x => x.ToModel(toWkt, toSrid)).ToList();
        }

        //public async Task<string> InsertPreplotPoints(int surveyId, string user, string comment, PreplotPointType inputType)
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    var conection = new CPEntities().Database.Connection.ConnectionString;
        //    using (SqlConnection con = new SqlConnection(conection))
        //    {
        //        string error = "";
        //        con.Open();
        //        SqlCommand com = new SqlCommand("InsertGPSeismicPreplot", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@SurveyId", surveyId);
        //        com.Parameters.AddWithValue("@user", user);
        //        com.Parameters.AddWithValue("@comment", comment);
        //        com.Parameters.AddWithValue("@inputType", (int)inputType);
        //        com.Parameters.AddWithValue("@error", error);
        //        com.Parameters["@error"].SqlDbType = SqlDbType.VarChar;
        //        com.Parameters["@error"].Size = 255;
        //        com.Parameters["@error"].Direction = ParameterDirection.Output;

        //        com.CommandTimeout = 0;
        //        com.ExecuteNonQuery();
        //        error = com.Parameters["@error"].Value.ToString();

        //        con.Close();
        //        return error;
        //    }

        //}
        //public async Task<String> InsertLineFromLastPreplotVersion(int surveyId)
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    var conection = new CPEntities().Database.Connection.ConnectionString;
        //    using (SqlConnection con = new SqlConnection(conection))
        //    {
        //        string error = "";
        //        con.Open();
        //        SqlCommand com = new SqlCommand("InsertLineFromLastPreplotVersion", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@SurveyId", surveyId);
        //        com.Parameters.AddWithValue("@maxVersion", 1);
        //        com.Parameters.AddWithValue("@error", error);
        //        com.Parameters["@error"].SqlDbType = SqlDbType.VarChar;
        //        com.Parameters["@error"].Size = 255;
        //        com.Parameters["@error"].Direction = ParameterDirection.Output;

        //        com.CommandTimeout = 0;
        //        com.ExecuteNonQuery();
        //        error = com.Parameters["@error"].Value.ToString();

        //        con.Close();
        //        return error;
        //    }

        //}
        public async Task<List<PointDetailModel>> GetPointDetail(int surveyId, int preplotVersionId, int preplotPointId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                   new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@preplotVersionId", preplotVersionId),
                    new SqlParameter("@preplotPointId", preplotPointId)
            };
            var items = await context.Set<PointDetailModel>().FromSqlRaw("execute [GetPointDetail] @surveyId,@preplotVersionId,@preplotPointId", parameters).ToListAsync();
           
            foreach (var item in items)
            {
                if (items.Count > 0)
                {

                    item.ProductionDateStr = item.ProductionDate.ToString("dd/MM/yyyy");
                    item.StatusName = EnumHelper.GetEnumDescription((ProductionStatus)item.Status);
                }
            }


            return items;
            
        }
        public async Task<List<LineStretchModel>> ListStretchesFromSwath(int surveyId, PreplotPointType pointType, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(pointType);
            var points = await context.PreplotPoints.Where(
                m => m.SurveyId == surveyId
                     && pointTypes.Contains((PreplotPointType)m.PreplotPointType)
                     && m.IsActive == true
                     && m.StationNumber >= initialStation
                     && m.StationNumber <= finalStation
                     && m.PreplotVersionId == (
                          context.PreplotPoints.Where(
                             x => x.PreplotPointId == m.PreplotPointId
                                  && x.SurveyId == m.SurveyId
                                  && x.PreplotPointType == m.PreplotPointType
                                  && x.Line == m.Line
                             ).Max(f => f.PreplotVersionId))
                ).GroupBy(m => m.PreplotPointId).Select(g => g.FirstOrDefault()).GroupBy(n => n.Line).ToListAsync();

            return points.Select(pointsOnLine => new LineStretchModel()
            {
                Line = pointsOnLine.Key,
                LinePointsType = pointType,
                InitialStation = pointsOnLine.Min(p => p.StationNumber),
                FinalStation = pointsOnLine.Max(p => p.StationNumber)
            }).ToList();

        }


        public async Task<List<PreplotPointModel>> ListSurveyPreplotPoints(int surveyId, PreplotPointType pointType, String line, Int32 preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@line", line),
                    new SqlParameter("@preplotPointType", (int)pointType),
                    new SqlParameter("preplotVersionId", preplotVersionId)
            };
            var models = await context.Set<PreplotPointModel>().FromSqlRaw("execute [ListPreplotPoints] @surveyId,@line,@preplotPointType,@preplotVersionId", parameters).ToListAsync();
            return models;
            
        }
      
        public async Task SavePreplotPoint(PreplotPointModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.PreplotPoints.Where(
                  m => m.SurveyId == model.SurveyId && m.PreplotPointId == model.PreplotVersionId
                  && m.PreplotVersionId == model.PreplotVersionId
                   ).FirstOrDefault();
            if (entity == null)
            {
                context.Add(model.ToEntity());
            }
            else
            {
                model.Copy(entity);
            }
            await context.SaveChangesAsync();
           

        }

        public Task<PreplotPointType> GetPreplotPointTypeByOpFrontType(OperationalFrontType operationalFront)
        {
            throw new NotImplementedException();
        }
    }
}
