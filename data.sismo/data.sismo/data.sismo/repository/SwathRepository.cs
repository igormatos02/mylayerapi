using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    
    public class SwathRepository : ISwathRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SwathRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<SwathModel> GetSwath(int surveyId, int swathNumber)
        {
            using var context = _contextFactory.CreateDbContext();
            var versionedSwath = await context.Swaths.Where(m => m.SurveyId == surveyId && m.SwathNumber == swathNumber).ToListAsync();
            if (!versionedSwath.Any()) return new SwathModel();

            var swathLastVersion = versionedSwath.Max(m => m.PreplotVersionId);
            var entity = await context.Swaths.Where(m => m.SurveyId == surveyId && m.SwathNumber == swathNumber && m.PreplotVersionId == swathLastVersion).FirstOrDefaultAsync();

            return entity.ToModel();
        }

        public async Task<SwathModel> GetSwath(int surveyId, int swathNumber, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =  await context.Swaths.Where(m => m.SurveyId == surveyId && m.SwathNumber == swathNumber && m.PreplotVersionId == preplotVersionId).FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<List<SwathModel>> ListSwaths(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var versionedSwaths = await context.Swaths.Where(m => m.SurveyId == surveyId).ToListAsync();
            if (!versionedSwaths.Any()) return new List<SwathModel>();

            var swathLastVersion = versionedSwaths.Max(m => m.PreplotVersionId);
            var entities = await context .Swaths.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == swathLastVersion).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<int>> ListSwathsNumbers(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var versionedSwaths = await context .Swaths.Where(m => m.SurveyId == surveyId).ToListAsync();
            if (!versionedSwaths.Any()) return new List<int>();

            var swathLastVersion = versionedSwaths.Max(m => m.PreplotVersionId);
            var entities =  await context.Swaths.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == swathLastVersion).Select(m => m.SwathNumber).ToListAsync();
            return entities.ToList();
        }

        public async Task<List<SwathModel>> ListSwaths(int surveyId, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.Swaths.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == preplotVersionId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task SaveSwath(SwathModel model)
        {
            using var context = _contextFactory.CreateDbContext();
          
            var entity = context.Swaths.Where(
                  m => m.SurveyId == model.SurveyId
                && m.SwathNumber == model.SwathNumber
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

        public async Task SaveSwaths(IEnumerable<SwathModel> swaths)
        {
           
            foreach (var swath in swaths)
                await SaveSwath(swath);
        }


        public async Task UpdateSwathPolygonSalva(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId)
                    
            };
            context.Database.SetCommandTimeout(0);
            await context.Database.ExecuteSqlRawAsync("pdateSwathPolygonSalva", parameters);

        }
    }
}
