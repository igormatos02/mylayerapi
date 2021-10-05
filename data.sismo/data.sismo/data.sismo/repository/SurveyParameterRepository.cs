using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    
    public class SurveyParameterRepository : ISurveyParameterRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SurveyParameterRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<SurveyParameterModel>> GetSurveyParameters(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.SurveyParameters.Where(m => m.SurveyId == surveyId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }
        public async Task<SurveyParameterModel> GetSurveyParameter(int surveyId, int parameterId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =await  context.SurveyParameters.Where(m => m.SurveyId == surveyId && m.ParameterId == parameterId).FirstOrDefaultAsync();
            return entity.ToModel();
        }
        public async Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.SurveyParameters.Where(m => m.SurveyId == surveyId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<SurveyParameterModel> AddSurveyParameter(SurveyParameterModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = model.ToEntity();
            context.Add(entity);
            await context.SaveChangesAsync();
            model.ParameterId = entity.ParameterId;
            return model;
        }

        public async Task UpdateSurveyParameter(SurveyParameterModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            model.LastUpdate = DateTime.Now;
            var entity = context.SurveyParameters.Where(m=>
                   m.SurveyId == model.SurveyId && m.ParameterId == model.ParameterId
                   ).FirstOrDefault();
            model.Copy(entity);

            await context.SaveChangesAsync();
        }
    }
}
