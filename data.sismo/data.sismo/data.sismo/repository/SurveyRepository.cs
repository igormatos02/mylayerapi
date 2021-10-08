using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace data.sismo.repository
{
    public class SurveyRepository : ISurveyRepository
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SurveyRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SurveyModel> GetSurvey(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
           
                var query = (from x in context.Surveys
                             where x.SurveyId == surveyId
                             select x);

                var entity = await query.FirstOrDefaultAsync();
                var model =  entity.ToFullModel(context);
                return model;
           
        }

        public async Task<Geometry> GetSurveyPolygonGeometry(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return entity.Polygon;
        }

        public async Task<int> GetSurveyDatumId(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return entity.DatumId;
        }

        public async Task<decimal?> GetSurveyDefaultHolesDepth(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return entity.HolesDepth;
        }

        public async Task<DateTime?> GetSurveyEndDate(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return entity.DateEnd;
        }

        public async Task<int?> GetSurveyDefaultHolesQuantityPerShotPoint(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return (entity != null && entity.HolesPerShotPoint.HasValue) ? entity.HolesPerShotPoint.Value : 0;
        }

        public async Task<decimal> GetSurveyDefaultDistanceBetweenHoles(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return (entity != null && entity.DistanceBetweenHoles.HasValue) ? entity.DistanceBetweenHoles.Value : 0;
        }

        public async Task<decimal> GetSurveyHoleBufferSizeFactor(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Surveys.Where(m => m.SurveyId == surveyId).FirstOrDefaultAsync();
            return (entity != null && entity.HolesBufferSizeFactor.HasValue) ? entity.HolesBufferSizeFactor.Value : 0;
        }
        public async Task<List<SurveyModel>> ListSurveys(bool? isActive = true)
        {

            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.Surveys
                         where isActive == null || isActive == x.IsActive
                         select x);

            var entities = await query.Select(x => x.ToSimplifiedModel()).ToListAsync();
            return entities;
        }

        public async Task<List<ProjectBaseModel>> ListSurveyProjectBases(int surveyId)
        {   
            using var context = _contextFactory.CreateDbContext();

            var query = await(from x in context.Surveys
                              where x.SurveyId == surveyId
                              select x).FirstOrDefaultAsync();

            var entities = query.SurveyProjectBases.Select(x => x.ProjectBase.ToModel()).ToList();
            return entities;
        }
        public async Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = await (from x in context.Surveys
                               where x.SurveyId == surveyId
                               select x).FirstOrDefaultAsync();

            var entities = query.SurveyParameters.Select(x => new SurveyParameterModel() { SurveyId = surveyId, ParameterId = x.ParameterId, Value = x.Value }).ToList();
            return entities;


        }

        public async Task<List<SurveyModel>> ListProjectSurveys(Int32 projectId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.Surveys
                         where x.ProjectId == projectId
                         select x);

            var entities = await query.Select(x => x.ToSimplifiedModel()).ToListAsync();
            return entities;
        }
       
        public async Task<int> AddSurvey(SurveyModel survey)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = survey.ToEntity();
            context.Add(entity);
            await context.SaveChangesAsync();
            return entity.SurveyId;
           
        }

        public async Task UpdateSurvey(SurveyModel modifiedSurvey)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Surveys.Where(m => m.SurveyId == modifiedSurvey.SurveyId).FirstOrDefault();
            entity.LastUpdate = DateTime.Now;
            //entity.GPSDatabaseName = modifiedSurvey.GPSDatabaseName;
            modifiedSurvey.Copy(entity);
            await context.SaveChangesAsync();
            return;
        }

        //public void AddSurveyOperationalFront(int surveyId, int operationalFrontId)
        //{
        //    using (var context = new CPEntities())
        //    {
        //        var survey = (from o in context.Surveys
        //                      select o).FirstOrDefault(o => o.SurveyId == surveyId);
        //        var operationalFront = (from o in context.OperationalFronts
        //                                select o).FirstOrDefault(c => c.OperationalFrontId == operationalFrontId);

        //        if (survey != null)
        //        {
        //            survey.OperationalFronts.Add(operationalFront);
        //            context.SaveChanges();
        //        }
        //    }
        //}
        //public void AddSurveyProjectBase(int surveyId, int projectBaseId)
        //{
        //    using (var context = new CPEntities())
        //    {
        //        var survey = (from o in context.Surveys
        //                      select o).FirstOrDefault(o => o.SurveyId == surveyId);
        //        var projectBase = (from o in context.ProjectBases
        //                           select o).FirstOrDefault(c => c.BaseId == projectBaseId);

        //        if (survey != null)
        //        {
        //            survey.ProjectBases.Add(projectBase);
        //            context.SaveChanges();
        //        }
        //    }
        //}

        //public void DeleteSurveyOperationalFront(int surveyId, int operationalFrontId)
        //{
        //    using (var context = new CPEntities())
        //    {
        //        var survey = (from o in context.Surveys
        //                      select o).FirstOrDefault(o => o.SurveyId == surveyId);
        //        var operationalFront = (from o in context.OperationalFronts
        //                                select o).FirstOrDefault(c => c.OperationalFrontId == operationalFrontId);

        //        if (survey != null)
        //        {
        //            survey.OperationalFronts.Remove(operationalFront);
        //            context.SaveChanges();
        //        }
        //    }
        //}

        //public void DeleteSurveyProjectBase(int surveyId, int projectBaseId)
        //{
        //    using (var context = new CPEntities())
        //    {
        //        var survey = (from o in context.Surveys
        //                      select o).FirstOrDefault(o => o.SurveyId == surveyId);
        //        var projectBase = (from o in context.ProjectBases
        //                           select o).FirstOrDefault(c => c.BaseId == projectBaseId);

        //        if (survey != null)
        //        {
        //            survey.ProjectBases.Remove(projectBase);
        //            context.SaveChanges();
        //        }
        //    }
        //}
        //public void AddGPSLinkedServer(string linkedServerName, string geosystemCommon)
        //{
        //    using (var DatabaseContext = new CPEntities())
        //    {
        //        var surveyIdParameter = new SqlParameter("@linkedServerName", linkedServerName);
        //        var pathParameter = new SqlParameter("@path", geosystemCommon + "GpSeismicDatabases");
        //        DatabaseContext.Database.CommandTimeout = 0;

        //        DatabaseContext.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC AddGPSLinkedServer @linkedServerName, @path", surveyIdParameter, pathParameter);
        //    }
        //}
        //public List<RDOMetaDataTO> GetMetaDataFromDate(int surveyId, int operationalfrontId, String selectedDate)
        //{
        //    using (var context = new CPEntities())
        //    {
        //        var items = context.Database.SqlQuery<RDOMetaDataTO>(
        //            "execute [GetMetaDataFromDate] @surveyId,@OperationalFrontId, @selectedDate",
        //            new SqlParameter("@surveyId", surveyId),
        //            new SqlParameter("@OperationalFrontId", operationalfrontId),
        //            new SqlParameter("@selectedDate", selectedDate == "" ? DateTime.Now : GeneralParsers.GetDBValue(selectedDate, "NormalDate"))
        //            ).ToList();

        //        foreach (var item in items)
        //        {
        //            if (items.Count > 0)
        //            {

        //                item.FrontGroupStartDateStr = item.FrontGroupStartDate.ToString("dd/MM/yyyy");
        //                item.TotalKmTotal = item.TotalKmER + item.TotalKmPT;
        //                item.RemainingKmPT = Math.Abs(item.SumKmProjectPT - item.TotalKmPT);
        //                item.RemainingKmER = Math.Abs(item.SumKmProjectER - item.TotalKmER);

        //                item.RemainingPT = Math.Abs(item.TotalRealizedProjectPT - item.TotalPT);
        //                item.RemainingER = Math.Abs(item.TotalRealizedProjectER - item.TotalER);

        //                item.TotalRealized = Math.Abs(item.TotalRealizedProjectER + item.TotalRealizedProjectPT);
        //                item.TotalNotRealized = Math.Abs(item.TotalRealized - (item.TotalPT + item.TotalER));



        //                /*
                       
        //                opFrontProductions.PercentRealized = (decimal)opFrontProductions.TotalRealized / (decimal)opFrontProductions.TotalPoints * 100;
        //                opFrontProductions.PercentNotRealized = (decimal)opFrontProductions.TotalNotRealized / (decimal)opFrontProductions.TotalPoints * 100;
        //                opFrontProductions.PercentMissing = (decimal)opFrontProductions.TotalMissing / (decimal)opFrontProductions.TotalPoints * 100;

        //                var kmRealizedBkp = opFrontProductions.KmRealized;
        //                opFrontProductions.KmRealized = kmRealizedBkp * opFrontProductions.TotalRealized /
        //                    ((opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized) <= 0 ?
        //                    1 : (opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized));
        //                opFrontProductions.KmNotRealized = kmRealizedBkp * opFrontProductions.TotalNotRealized /
        //                    ((opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized) <= 0 ?
        //                    1 : (opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized));
        //                opFrontProductions.KmMissing = opFrontProductions.KmTotal - kmRealizedBkp;
        //                opFrontProductions.AreaTotal = context.Database.SqlQuery<decimal>(
        //                    "select " +
        //                    "  case " +
        //                    "    when DatumId = 4326 then CONVERT(decimal(30, 15), 0)" +
        //                    "    when DatumId <> 4326 then CONVERT(decimal(30, 15), dbo.GeometryToGeometry(Polygon, DatumId).STArea() / 1000000) " +
        //                    "  end " +
        //                    "from survey " +
        //                    "where surveyId =" + surveyId).ToList().FirstOrDefault();
        //                opFrontProductions.AreaRealized = opFrontProductions.TotalRealized * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);
        //                opFrontProductions.AreaNotRealized = opFrontProductions.TotalNotRealized * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);
        //                opFrontProductions.AreaMissing = opFrontProductions.TotalMissing * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);
        //                    */
        //            }

        //            // opFrontProductions.RealizedStatusName = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, (OperationalFrontType)opFrontProductions.OperationalFrontType);
        //            //opFrontProductions.NotRealizedStatusName = Enumerations.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, (OperationalFrontType)opFrontProductions.OperationalFrontType);

        //            //opFrontProductions.MissingStatusName = Enumerations.GetEnumDescription(ProductionStatus.Pending);
        //        }
        //        return items;
        //    }
        //    //public SurveyDTO GetSurveyWithoutDetails(int surveyId)
        //    //{
        //    //    return SurveyParser.StGetInstance().CreateDtoWithoutDetails(GetSingle(m => m.SurveyId == surveyId)) as SurveyDTO;
        //    //}
        //}
    }
}
