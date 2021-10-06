using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    public class DisplacementRuleRepository : IDisplacementRuleRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;

        public DisplacementRuleRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DisplacementRuleModel> GetDisplacementRule(int displacementRuelId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.DisplacementRules
                         where x.DisplacementRuleId == displacementRuelId
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<List<DisplacementRuleModel>> ListDisplacementRules(int surveyId, bool onlyActives, int type)
        {   
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.DisplacementRules
                         where x.SurveyId == surveyId
                         && (onlyActives == false || x.IsActive==true)
                         && ((type !=0 && (int)x.DisplacementRuleType == type) ||(type == 0))
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }
        public async Task<List<DisplacementRuleModel>> ListDisplcementRules()
        {
           using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.DisplacementRules
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task<DisplacementRuleModel> SaveDisplacementRules(DisplacementRuleModel model)
        {
            model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.DisplacementRules.Where(
                   m => m.SurveyId == model.SurveyId
                       && m.DisplacementRuleId == model.DisplacementRuleId
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
            model.DisplacementRuleId = entity.DisplacementRuleId;
            return model;
        }

        public async Task UpdateDisplacementRule(DisplacementRuleModel model)
        {
            model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.DisplacementRules.Where(
                   m => m.SurveyId == model.SurveyId
                       && m.DisplacementRuleId == model.DisplacementRuleId
                   ).FirstOrDefault();
            model.Copy(entity);

            await context.SaveChangesAsync();
        }
    }
}
