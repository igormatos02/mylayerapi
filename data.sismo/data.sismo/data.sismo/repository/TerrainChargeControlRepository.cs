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
   

    public class TerrainChargeControlRepository : ITerrainChargeControlRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public TerrainChargeControlRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TerrainChargeControlModel> GetTerrainChargeControl(int terrainChargeControlId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.TerrainChargeControls.Where(m => m.TerrainChargeControlId == terrainChargeControlId).FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<List<TerrainChargeControlModel>> ListTerrainChargeControls(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.TerrainChargeControls.Where(m => m.SurveyId == surveyId).ToListAsync(); 
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task SaveTerrainChargeControls(TerrainChargeControlModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.TerrainChargeControls.Where(
                      m => m.TerrainChargeControlId == model.TerrainChargeControlId
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

        public async Task DeleteTerrainChargeControl(TerrainChargeControlModel dto)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.TerrainChargeControls.Where(m => m.TerrainChargeControlId == dto.TerrainChargeControlId).FirstOrDefaultAsync();
            if (entity == null)
                return;
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
