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
    public class PreplotVersionRepository : IPreplotVersionRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PreplotVersionRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PreplotVersionModel> GetPreplotVersion(int surveyId, int versionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotVersions.FirstOrDefaultAsync(m => m.SurveyId == surveyId && m.PreplotVersionId == versionId);
            return entity.ToModel();
        }

        public async Task<PreplotVersionModel> GetPreplotVersion(int surveyId, DateTime versionDate)
        {
            using var context = _contextFactory.CreateDbContext();
            var lastVersionBeforeDate = await context.PreplotVersions.Where(m => m.SurveyId == surveyId && m.CreationDate < versionDate).OrderBy(m => m.CreationDate).LastOrDefaultAsync();
            return lastVersionBeforeDate.ToModel();
        }

        public async Task<List<PreplotVersionModel>> ListPreplotVersions(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.PreplotVersions.Where(m => m.SurveyId == surveyId).OrderBy(m => m.CreationDate).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<int> InsertPreplotVersion(PreplotVersionModel preplotVersion)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = preplotVersion.ToEntity();
            context.PreplotVersions.Add(entity);
            await context.SaveChangesAsync();
            return entity.PreplotVersionId;
        }

        public async Task _Delete(PreplotVersionModel version)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotVersions.FirstOrDefaultAsync(x => x.PreplotVersionId == version.PreplotVersionId);
            context.PreplotVersions.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePreplotDate(Int32 surveyId, Int32 currentVersion)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PreplotVersions.FirstOrDefaultAsync(x => x.SurveyId == surveyId &&  x.PreplotVersionId == currentVersion);
            entity.CreationDate = DateTime.Now;
            await context.SaveChangesAsync();
        }

    }
}
