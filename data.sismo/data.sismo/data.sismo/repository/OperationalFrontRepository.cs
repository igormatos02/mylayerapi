using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace data.sismo.repository
{

   
    public class OperationalFrontRepository : IOperationalFrontRepository
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public OperationalFrontRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
      
        public async Task<bool> HasAnyProduction(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.OperationalFronts.AnyAsync(m => m.OperationalFrontId == operationalFrontId && m.PointProductions.Count > 0);
        }

        public async Task<OperationalFrontModel> GetOperationalFront(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.OperationalFronts.Where(
                 m => m.OperationalFrontId == operationalFrontId
                   ).FirstOrDefaultAsync();

            return entity.ToModel();

        }
        public async Task<List<OperationalFrontProductionModel>> GetOperationalFrontProduction(int surveyId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var dateValue = DateHelper.GetDBValue(date, "NormalDate");
            var parameters = new[] {
                new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.Int },
                new SqlParameter("@date", date) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.DateTime},
            };
            var models = await context.Set<OperationalFrontProductionModel>().FromSqlRaw("execute [GetOperationalFrontProduction] @surveyId, @date", parameters).ToListAsync();
            return models; 
           
        }
        public async Task<string> GetOperationalFrontName(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.OperationalFronts.Where(
                 m => m.OperationalFrontId == operationalFrontId
                   ).FirstOrDefaultAsync();

            return entity.Name;
        }

        public async Task<OperationalFrontType> GetOperationalFrontType(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.OperationalFronts.Where(
                 m => m.OperationalFrontId == operationalFrontId
                   ).FirstOrDefaultAsync();

            return (OperationalFrontType)entity.OperationalFrontType;
        }

        public async Task<OperationalFrontModel> GetPreviousOperationalFront(int operationalFrontId)
        {
            var previousFrontId = await GetPreviousOperationalFrontId(operationalFrontId);
            if(!previousFrontId.HasValue) return null;
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.OperationalFronts
                         where x.OperationalFrontId == previousFrontId.Value
                         select x);

            var entities = await query.Select(x => x.ToModel()).FirstOrDefaultAsync();
            return entities;

            
          
        }

        public async Task<int?> GetPreviousOperationalFrontId(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.OperationalFronts
                         where x.OperationalFrontId == operationalFrontId
                         select x);

            var entity = await query.Select(x => x.PreviousOperationalFrontId).FirstOrDefaultAsync();
            return entity;
          
        }

        public async Task<List<OperationalFrontModel>> ListNextOperationalFronts(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.OperationalFronts
                         where x.PreviousOperationalFrontId == operationalFrontId
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task<OperationalFrontModel> AddOperationalFront(OperationalFrontModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            model.LastUpdate = DateTime.Now;
            var entity = model.ToEntity();
            context.Add(entity);
            await context.SaveChangesAsync();
            model.OperationalFrontId = entity.OperationalFrontId;
            return model;
        }
        public async Task UpdateOperationalFront(OperationalFrontModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            model.LastUpdate = DateTime.Now;
           
            var entity = context.OperationalFronts.Where(m => m.OperationalFrontId == model.OperationalFrontId).FirstOrDefault();
            if (entity != null)
                model.Copy(entity);

            await context.SaveChangesAsync();
        }

        public async Task DeleteOperationalFront(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.OperationalFronts.Where(
                   m => m.OperationalFrontId == operationalFrontId 
                   ).FirstOrDefault();
            
            context.OperationalFronts.Remove(entity);
            await context.SaveChangesAsync();
            return;
        }

        public async Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = await (from x in context.Surveys
                               where x.SurveyId == surveyId
                               select x).FirstOrDefaultAsync();

            var entities = query.SurveyOperationalFronts.Select(x => x.OperationalFront.ToModel());
            return entities.OrderBy(m => m.OperationalFrontType).ThenBy(m => m.OperationalFrontId).ToList();
        }

        public async Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId, int frontType)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = await (from x in context.Surveys
                               where x.SurveyId == surveyId
                               select x).FirstOrDefaultAsync();

            var entities = query.SurveyOperationalFronts.Select(x => x.OperationalFront.ToModel());
            return entities.Where(x => x.OperationalFrontType == (OperationalFrontType)frontType).OrderBy(m => m.OperationalFrontType).ThenBy(m => m.OperationalFrontId).ToList();

        }

        public async Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = await (from x in context.OperationalFronts
                               where x.ProjectId == projectId
                               select x).OrderBy(m => m.OperationalFrontType).ThenBy(m => m.OperationalFrontId).ToListAsync();

            return query.Select(x => x.ToModel()).ToList();
        }
    }
}
