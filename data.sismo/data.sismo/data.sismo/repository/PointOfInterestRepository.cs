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
   

    public class PointOfInterestRepository : IPointOfInterestRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PointOfInterestRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PointOfInterestModel> SavePointOfInterest(PointOfInterestModel model)
        {
            model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.SurveyPointsOfInterests.Where(
                   m => m.PointOfInterestId == model.PointOfInterestId
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
            model.PointOfInterestId = entity.PointOfInterestId;
            return model;

          
        }
        public async Task<PointOfInterestModel> GetPointOfInterest(int? surveyId, int pointOfInterestId)
        {
            using var context = _contextFactory.CreateDbContext();
            
            var query = (from x in context.SurveyPointsOfInterests
                         where x.SurveyId == surveyId
                        && x.PointOfInterestId == pointOfInterestId
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();
          
        }
        public async Task<List<PointOfInterestModel>> ListPointsOfInterest(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.SurveyPointsOfInterests
                         where x.SurveyId == surveyId
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
         }
        public async Task<List<PointOfInterestModel>> ListAllPointsOfInterest()
        {

            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.SurveyPointsOfInterests
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }
        public async Task<bool> DeletePointOfInterest(Int32 pointOfInterestId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.SurveyPointsOfInterests.Where(
                   m => m.PointOfInterestId == pointOfInterestId
                   ).FirstOrDefault();
            if (entity == null)
                return false;
            context.SurveyPointsOfInterests.Remove(entity);
            await context.SaveChangesAsync();
            return true;

          
        }
    }
}
