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
    public class ProjectExplosiveMaterialRepository: IProjectExplosiveMaterialRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ProjectExplosiveMaterialRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProjectExplosiveMaterialModel> GetProjectExplosiveMaterial(int entryId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await  context.ProjectExplosiveMaterials.FirstOrDefaultAsync (m => m.EntryId == entryId);
            return entity.ToModel();
        }

        public async Task<List<ProjectExplosiveMaterialModel>> ListProjectExplosiveMaterials(int ProjectId, Int32 entryType)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.ProjectExplosiveMaterials.Where(m => m.ProjectId == ProjectId && m.EntryType == entryType && m.IsActive == true).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }
        public async Task<List<ProjectExplosiveMaterialModel>> ListDeletedProjectExplosiveMaterials(int ProjectId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.ProjectExplosiveMaterials.Where(m => m.ProjectId == ProjectId && m.IsActive == false).ToListAsync(); ;
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task SaveProjectExplosiveMaterials(ProjectExplosiveMaterialModel dto)
        {
            using var context = _contextFactory.CreateDbContext();
            //dto.LastUpdate = DateTime.Now;
            if (dto.SurveyId == 0)
                dto.SurveyId = null;
            var entity = await context.ProjectExplosiveMaterials.Where(
                   m => m.EntryId == dto.EntryId
                   ).FirstOrDefaultAsync();
            if (entity != null)
            {

                dto.Copy(entity);
               await  context.SaveChangesAsync();
            }
            else
            {
                context.ProjectExplosiveMaterials.Add(dto.ToEntity());
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteProjectExplosiveMaterial(ProjectExplosiveMaterialModel dto)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.ProjectExplosiveMaterials.Where(
                   m => m.EntryId == dto.EntryId
                   ).FirstOrDefaultAsync();
            if (entity == null)
                return;
            context.ProjectExplosiveMaterials.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
