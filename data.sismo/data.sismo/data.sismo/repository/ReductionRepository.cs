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
   
    public class ReductionRuleRepository: IReductionRuleRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ReductionRuleRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<ReductionRuleModel> GetReductionRule(int reductionRuleId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await  context.ReductionRules.Where(m => m.ReductionRuleId == reductionRuleId).FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<int> GetReductionFinalHolesQuantity(int reductionRuleId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.ReductionRules.Where(m => m.ReductionRuleId == reductionRuleId).FirstOrDefaultAsync();
            return entity != null ? entity.FinalHolesQuantity : 0;
        }

        public async Task<decimal> GetGreatestDistanceBetweenEndHoles(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var rule = await context.ReductionRules.Where(m => m.SurveyId == surveyId).OrderBy(m => (m.FinalHolesQuantity - 1) * m.DistanceBetweenHoles / 2).FirstOrDefaultAsync();
            return (rule.FinalHolesQuantity - 1) * rule.DistanceBetweenHoles / 2;
        }

        public async Task<List<ReductionRuleModel>> ListReductionRules(int idSurvey)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities =  await context.ReductionRules.Where(m => m.SurveyId == idSurvey).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<ReductionRuleModel>> ListReductionRules(int idSurvey, bool isActive)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.ReductionRules.Where(m => m.SurveyId == idSurvey && m.IsActive == isActive).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }
        public async Task<List<ReductionRuleModel>> ListAllReductionRules()
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.ReductionRules.ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<ReductionRuleModel> SaveReductionRules(ReductionRuleModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            model.LastUpdate = DateTime.Now;
            var entity = context.ReductionRules.Where(
                   m => m.SurveyId == model.SurveyId
                       && m.ReductionRuleId == model.ReductionRuleId
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
            model.ReductionRuleId = entity.ReductionRuleId;
            return model;
        }

        public async Task UpdateReductionRule(ReductionRuleModel model)
        {
            model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.ReductionRules.Where(
                   m => m.SurveyId == model.SurveyId
                       && m.ReductionRuleId == model.ReductionRuleId
                   ).FirstOrDefault();
            model.Copy(entity);

            await context.SaveChangesAsync();
        }


    }
}
