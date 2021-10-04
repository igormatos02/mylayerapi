using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using common.sismo.models;
using common.sismo.enums;
using System.Threading.Tasks;
using data.sismo.mapping;
using common.sismo.helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using common.sismo.interfaces.repositories;

namespace data.sismo.repository
{
    public class PointProductionRepository : IPointProductionRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PointProductionRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PointProductionModel> GetProduction(int surveyId, int preplotPointId, int preplotVersionId, int workNumber, PreplotPointType pointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =  await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                        && m.PreplotPointId == preplotPointId
                        && m.PreplotVersionId == preplotVersionId
                        && m.WorkNumber == workNumber
                        && m.PreplotPointType == (int)pointType
                        && m.OperationalFrontId == operationalFrontId
                    ).FirstOrDefaultAsync();
            return entity.ToModel();
        }


        public async Task<DateTime> GetFirstProductionDate(int surveyId, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                         x => x.PreplotPointId == m.PreplotPointId
                              && x.SurveyId == m.SurveyId
                              && x.PreplotPointType == m.PreplotPointType
                    ).Max(f => f.PreplotVersionId))
                ).OrderBy(m => m.Date).FirstOrDefaultAsync();
            

            return entity != null ? entity.Date : new DateTime();
        }

        public async Task<DateTime> GetLastProductionDate(int surveyId, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PointProductions.Where(
                 m => m.SurveyId == surveyId
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                         x => x.PreplotPointId == m.PreplotPointId
                              && x.SurveyId == m.SurveyId
                              && x.PreplotPointType == m.PreplotPointType
                 ).Max(f => f.PreplotVersionId))
             ).OrderByDescending(m => m.Date).FirstOrDefaultAsync();

            return entity != null ? entity.Date : new DateTime();
        }

        public async Task<List<PointProductionModel>> ListLandProductions(int surveyId, int operationalFrontId, int landId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var productionData = DateHelper.StringToDate(date);
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == 1
                     && m.PreplotPoint.LandId.HasValue
                     && m.PreplotPoint.LandId.Value == landId
                     && m.Date == productionData
                ).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListStretchProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation,
            decimal finalStation, int workNumber)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotPoint.Line == line
                     && m.PreplotPoint.IsActive
                     && m.PreplotPoint.StationNumber >= initialStation
                     && m.PreplotPoint.StationNumber <= finalStation
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == workNumber
                ).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListStretchLastWorks(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, decimal initialStation,
            decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotPoint.Line == line
                     && m.PreplotPoint.StationNumber >= initialStation
                     && m.PreplotPoint.StationNumber <= finalStation
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.PreplotVersionId == m.PreplotVersionId
                                                        && x.OperationalFrontId == m.OperationalFrontId
                                                    ).Max(f => f.WorkNumber))
                ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListStretchLastWorks(int surveyId, int operationalFrontId, PreplotPointType preplotPointType, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotPoint.StationNumber >= initialStation
                     && m.PreplotPoint.StationNumber <= finalStation
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                        && x.PreplotVersionId == m.PreplotVersionId
                                                        && x.OperationalFrontId == m.OperationalFrontId
                                                    ).Max(f => f.WorkNumber))
                ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListStretchReworks(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string line, int initialStation,
            int finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotPoint.Line == line
                     && m.PreplotPoint.StationNumber >= initialStation
                     && m.PreplotPoint.StationNumber <= finalStation
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber > 1
                ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<DailyProductionModel>> ListDailyProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId)
        {

            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.Int },
                new SqlParameter("@preplotPointType", preplotPointType) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
                new SqlParameter("@operationalFrontId", operationalFrontId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int}
            };
            var models = await context.Set<DailyProductionModel>().FromSqlRaw("execute [ListDailyProductions] @surveyId,@preplotPointType,@operationalFrontId", parameters).ToListAsync();
            return models;
        }

       
        public async Task<List<StretchModel>> ListDailyStretchesProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, DateTime date)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.Int },
                new SqlParameter("@preplotPointType", preplotPointType) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
                new SqlParameter("@operationalFrontId", operationalFrontId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
                new SqlParameter("@productionDate", date) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.DateTime},
                new SqlParameter("@roundValue", CpConstants.KmDecimalPlaces) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Decimal}
            };
            var models = await context.Set<StretchModel>().FromSqlRaw("execute [ListDailyStretchesProductions] @surveyId,@preplotPointType,@operationalFrontId,@productionDate,@roundValue", parameters).ToListAsync();
            return models;
           
           
        }

        public async Task<List<LandStretchModel>> ListDailyLandStretchesProductions(int surveyId, int operationalFrontId, DateTime date)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.Int },
               
                    new SqlParameter("@operationalFrontId", operationalFrontId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
                    new SqlParameter("@productionDate", date) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.DateTime},
                    new SqlParameter("@roundValue", CpConstants.KmDecimalPlaces) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Decimal}
                };
            var models = await context.Set<LandStretchModel>().FromSqlRaw("execute [ListDailyLandStretchesProductions] @surveyId,@operationalFrontId,@productionDate,@roundValue", parameters).ToListAsync();
            return models;
        }

      
        public async Task<List<PointProductionModel>> ListProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var productionDate = DateHelper.StringToDate(date);
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.Date == productionDate
                     && m.WorkNumber == 1).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListProductions(int surveyId, PreplotPointType preplotPointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == 1).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListProductions(int surveyId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var formattedDate = DateHelper.StringToDate(date).Date;
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                    && m.Date == formattedDate
                     && m.PreplotVersionId == (context.PointProductions.Where(
                                                        x => x.PreplotPointId == m.PreplotPointId
                                                        && x.SurveyId == m.SurveyId
                                                        && x.PreplotPointType == m.PreplotPointType
                                                    ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == 1).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<DateTime>> ListProductionsDates(int surveyId, PreplotPointType preplotPointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                         x => x.PreplotPointId == m.PreplotPointId
                              && x.SurveyId == m.SurveyId
                              && x.PreplotPointType == m.PreplotPointType
                         ).Max(f => f.PreplotVersionId))
                     && m.WorkNumber == 1).Select(m => m.Date).Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<int, int>>> ListProductionsFrontGroupAndLeaders(int surveyId,
            PreplotPointType preplotPointType, int operationalFrontId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var productionDate = DateHelper.StringToDate(date);
            var entities = await  context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId == (context.PointProductions.Where(
                         x => x.PreplotPointId == m.PreplotPointId
                              && x.SurveyId == m.SurveyId
                              && x.PreplotPointType == m.PreplotPointType
                         ).Max(f => f.PreplotVersionId))
                     && m.Date == productionDate
                     && m.WorkNumber == 1)
                .Select(m => new KeyValuePair<int, int>(m.FrontGroupId, m.FrontGroupLeaderId))
                .Distinct().ToListAsync();

            return entities.ToList();
        }

        public async Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var productionDate = DateHelper.StringToDate(date);
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId ==
                     (context.PointProductions.Where
                         (x => x.PreplotPointId == m.PreplotPointId
                               && x.SurveyId == m.SurveyId
                               && x.PreplotPointType == m.PreplotPointType
                         ).Max(f => f.PreplotVersionId)
                         )
                     && m.Date == productionDate
                     && m.WorkNumber == 1
                ).Select(p => p.PreplotPoint).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date, int landId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.OperationalFrontId == operationalFrontId
                     && m.PreplotVersionId ==
                     (context.PointProductions.Where
                         (x => x.PreplotPointId == m.PreplotPointId
                               && x.SurveyId == m.SurveyId
                               && x.PreplotPointType == m.PreplotPointType
                         ).Max(f => f.PreplotVersionId)
                         )
                     && m.Date == DateHelper.StringToDate(date)
                     && m.WorkNumber == 1
                     && m.PreplotPoint.LandId == landId
                ).Select(p => p.PreplotPoint).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PreplotPointModel>> ListProductionsPoints(int surveyId, PreplotPointType preplotPointType, int operationalFrontId, string date, KeyValuePair<int, int> frontGroupAndLeader)
        {
            using var context = _contextFactory.CreateDbContext();
            var productionDate = DateHelper.StringToDate(date);
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == surveyId
                  && m.OperationalFrontId == operationalFrontId
                     && (preplotPointType == PreplotPointType.All || m.PreplotPointType == (int)preplotPointType)
                     && m.PreplotVersionId ==
                     (context.PointProductions.Where
                         (x => x.PreplotPointId == m.PreplotPointId
                               && x.SurveyId == m.SurveyId
                               && x.PreplotPointType == m.PreplotPointType
                             
                         ).Max(f => f.PreplotVersionId)
                         )
                     && m.Date == productionDate
                     && m.WorkNumber == 1
                     && m.FrontGroupId == frontGroupAndLeader.Key
                     && m.FrontGroupLeaderId == frontGroupAndLeader.Value
                ).Select(p => p.PreplotPoint).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListProductionsAndReworks(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                         && m.PreplotPointId == preplotPointId
                         && m.PreplotVersionId == preplotVersionId
                         && m.PreplotPointType == (int)pointType
                         && m.OperationalFrontId == operationalFrontId
                     ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<PointProductionModel>> ListProductionsWithDisplacementOrReduction(int surveyId, OperationalFrontType operationalFrontType)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                        && m.PreplotPointType == (int)PreplotPointType.ShotPoint
                        && m.OperationalFront.OperationalFrontType == (int)operationalFrontType
                        && (m.DisplacementRuleId.HasValue || m.ReductionRuleId.HasValue)
                    ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<int> CountProductions(int surveyId, string line, decimal initialStation, decimal finalStation, PreplotPointType pointType,
            int status, int operatonalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                         && m.PreplotPoint.Line == line
                         && m.PreplotPoint.StationNumber >= initialStation
                         && m.PreplotPoint.StationNumber <= finalStation
                         && m.OperationalFrontId == operatonalFrontId
                         && (pointType == PreplotPointType.All || m.PreplotPointType == (int)pointType)
                         && m.Status == status
                         && m.IsActive
                    ).CountAsync();
        }

        public async Task<PointProductionModel> GetPreviousFrontLastProductionOrRework(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PointProductions.Where(m => m.SurveyId == surveyId
                                                                               && m.PreplotPointId == preplotPointId
                                                                               && m.PreplotVersionId == preplotVersionId
                                                                               && m.PreplotPointType == (int)pointType
                                                                               && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<string> GetPreviousFrontsLastProductionsOrReworksObservations(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int opFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return String.Join(" / ",
               await  context.PointProductions.Where
                (m => m.SurveyId == surveyId
                    && m.PreplotPointId == preplotPointId
                    && m.PreplotVersionId == preplotVersionId
                    && m.PreplotPointType == (int)pointType
                    && m.OperationalFrontId != opFrontId
                ).Where(m => !String.IsNullOrWhiteSpace(m.Observation)).OrderBy(n => n.Date).Select(m => m.Observation).ToListAsync()
            );

        }

        public async Task<int?> GetPreviousFrontLastProductionOrReworkDisplacementRuleId(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var production = await context.PointProductions.Where(m => m.SurveyId == surveyId
                                        && m.PreplotPointId == preplotPointId
                                        && m.PreplotVersionId == preplotVersionId
                                        && m.PreplotPointType == (int)pointType
                                        && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return production != null ? production.DisplacementRuleId : null;
        }

        public async Task<int?> GetPreviousFrontLastProductionOrReworkReductionRuleId(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var production =  await context.PointProductions.Where(m => m.SurveyId == surveyId
                                        && m.PreplotPointId == preplotPointId
                                        && m.PreplotVersionId == preplotVersionId
                                        && m.PreplotPointType == (int)pointType
                                        && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return production != null ? production.ReductionRuleId : null;
        }

        public async Task<decimal> GetPreviousFrontLastProductionOrReworkHolesDepth(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var production = await context.PointProductions.Where(m => m.SurveyId == surveyId
                                        && m.PreplotPointId == preplotPointId
                                        && m.PreplotVersionId == preplotVersionId
                                        && m.PreplotPointType == (int)pointType
                                        && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return production != null ? (production.Holes != null ? (production.Holes.FirstOrDefault() != null ? production.Holes.FirstOrDefault().Depth : -1) : -1) : -1;
        }

        public async Task<int?> GetPreviousFrontLastProductionOrReworkFusesInSp(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var production = await context.PointProductions.Where(m => m.SurveyId == surveyId
                                        && m.PreplotPointId == preplotPointId
                                        && m.PreplotVersionId == preplotVersionId
                                        && m.PreplotPointType == (int)pointType
                                        && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return production != null ? (production.Holes != null ? (int?)production.Holes.Sum(h => h.NumberOfFuses) : 0) : 0;
        }

        public async Task<int?> GetPreviousFrontLastProductionOrReworkChargesInSp(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int previousOpFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var production =  await context.PointProductions.Where(m => m.SurveyId == surveyId
                                        && m.PreplotPointId == preplotPointId
                                        && m.PreplotVersionId == preplotVersionId
                                        && m.PreplotPointType == (int)pointType
                                        && m.OperationalFrontId == previousOpFrontId
                ).OrderByDescending(n => n.Date).FirstOrDefaultAsync();
            return production != null ? (production.Holes != null ? (int?)production.Holes.Sum(h => h.NumberOfCharges) : 0) : 0;
        }

        public async Task<List<PointProductionModel>> ListReworks(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                    m => m.SurveyId == surveyId
                        && m.PreplotPointId == preplotPointId
                        && m.PreplotVersionId == preplotVersionId
                        && m.PreplotPointType == (int)pointType
                        && m.OperationalFrontId == operationalFrontId
                        && m.WorkNumber > 1
                    ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();       
        }

        public async Task<int> CountNextOperationalFrontsProductions(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFront.PreviousOperationalFrontId == production.OperationalFrontId
                    ).CountAsync();
        }

        public async Task<bool> HasNextOperationalFrontsProductionsInFutureDates(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFront.PreviousOperationalFrontId == production.OperationalFrontId
                        && m.Date >= production.Date
                    ).AnyAsync();
        }

        public async Task<bool> HasNextOperationalFrontsProductionsInFutureDatesWithAccomplishedStatus(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFront.PreviousOperationalFrontId == production.OperationalFrontId
                        && m.Date >= production.Date
                        && m.Status == (int)ProductionStatus.Accomplished
                    ).AnyAsync();
        }

        public async Task<bool> HasNextOperationalFrontsProductionsInPastDates(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFront.PreviousOperationalFrontId == production.OperationalFrontId
                        && m.Date < production.Date
                    ).AnyAsync();
        }

        public async Task<int> CountPreviousOperationalFrontsProductions(PointProductionModel production, int previousOperationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFrontId == previousOperationalFrontId
                    ).CountAsync();
        }

        public async Task<List<PointProductionModel>> ListPreviousOperationalFrontsProductionsInPastDates(PointProductionModel production, int previousOperationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PointProductions.Where(
                m => m.SurveyId == production.SurveyId
                     && m.PreplotPointId == production.PreplotPointId
                     && m.PreplotVersionId == production.PreplotVersionId
                     && m.PreplotPointType == (int)production.PreplotPointType
                     && m.OperationalFrontId == previousOperationalFrontId
                     && m.Date < production.Date
                ).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<bool> HasPreviousOperationalFrontsProductionsInFutureDates(PointProductionModel production, int previousOperationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.Where(
                    m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFrontId == previousOperationalFrontId
                        && m.Date > production.Date
                    ).AnyAsync();
        }

        public async Task<bool> HasProductionsWithOnlyStationsStatusInPastDates(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PointProductions.AnyAsync(m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.Date <= production.Date
                        && m.Status == (int)ProductionStatus.OnlyReceptorStations);
        }

        public async Task<int> GetMaxWorkNumber(int surveyId, int preplotPointId, int preplotVersionId, PreplotPointType pointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var productions = await context.PointProductions.Where(m => m.SurveyId == surveyId
                        && m.PreplotPointId == preplotPointId
                        && m.PreplotVersionId == preplotVersionId
                        && (pointType == PreplotPointType.All || m.PreplotPointType == (int)pointType)
                        && m.OperationalFrontId == operationalFrontId).ToListAsync();
            if (productions == null || !productions.Any()) return 0;
            return productions.Max(n => n.WorkNumber);
        }

        public async Task SaveProduction(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.PointProductions.Where(
                   m => m.SurveyId == production.SurveyId
                        && m.PreplotPointId == production.PreplotPointId
                        && m.PreplotVersionId == production.PreplotVersionId
                        && m.WorkNumber == production.WorkNumber
                        && m.PreplotPointType == (int)production.PreplotPointType
                        && m.OperationalFrontId == production.OperationalFrontId
                   ).FirstOrDefault();
            if (entity == null)
            {
                context.Add(production.ToEntity());
            }
            else
            {
                context.PointProductions.Remove(entity);
                await context.SaveChangesAsync();
                var newEntity = production.ToEntity();
                context.PointProductions.Add(newEntity);
            }
            await context.SaveChangesAsync();
           
        }

        public async Task SaveProductions(IEnumerable<PointProductionModel> productions)
        {
            foreach (var production in productions)
                await SaveProduction(production);
        }

        public async Task<bool> DeleteProduction(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.PointProductions.Where(
            m => m.SurveyId == production.SurveyId
                     && m.PreplotPointId == production.PreplotPointId
                     && m.PreplotVersionId == production.PreplotVersionId
                     && m.WorkNumber == production.WorkNumber
                     && m.PreplotPointType == (int)production.PreplotPointType
                     && m.OperationalFrontId == production.OperationalFrontId
                ).FirstOrDefault();
            if (entity == null) return false;

            context.PointProductions.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteProductionDate(string date, int operationalFrontId, int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                new SqlParameter("@date", DateHelper.GetDBValue(date, "NormalDate")) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.VarChar },
                new SqlParameter("@operationalFrontId", operationalFrontId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
                 new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
            };
            //DatabaseContext.Database.CommandTimeout = 0;
            var models = await context.Set<LineModel>().FromSqlRaw("EXEC DeleteProductionDate @date,@operationalFrontId,@surveyId", parameters).ToListAsync();
           
           
        }
       
    }
}
