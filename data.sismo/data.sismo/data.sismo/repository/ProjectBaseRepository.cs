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
    public class ProjectBaseRepository : IProjectBaseRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ProjectBaseRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProjectBaseModel> GetBase(int baseId)
        {
            using var context = _contextFactory.CreateDbContext();
           var entity = await  context.ProjectBases.FirstOrDefaultAsync((m => m.BaseId == baseId));
            return entity.ToModel();
        }

        public async Task<List<ProjectBaseModel>> ListBases(int projectId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities =  await context.ProjectBases.Where(w => w.ProjectId == projectId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }



        public async Task<int> AddBase(ProjectBaseModel Base)
        {
            using var context = _contextFactory.CreateDbContext();
            Base.LastUpdate = DateTime.Now;
            var temp = Base.ToEntity();
            context.ProjectBases.Add(temp);
            await context.SaveChangesAsync();
            return temp.BaseId;
        }

        public async Task UpdateBase(ProjectBaseModel modifiedBase)
        {
            using var context = _contextFactory.CreateDbContext();
            modifiedBase.LastUpdate = DateTime.Now;
            var entity = await context.ProjectBases.FirstOrDefaultAsync((m => m.BaseId == modifiedBase.BaseId));
            modifiedBase.Copy(entity);
            await context.SaveChangesAsync();
        }
        public async Task DeleteBase(ProjectBaseModel modifiedBase)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.ProjectBases.FirstOrDefaultAsync((m => m.BaseId == modifiedBase.BaseId));
            context.ProjectBases.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddFile(String fileName, Int32 baseId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.ProjectBases.FirstOrDefaultAsync((m => m.BaseId == baseId));
            entity.ProjectBaseImages.Add(new ProjectBaseImage() { ProjectBaseId = baseId, Image = fileName });
            await context.SaveChangesAsync();
        }
    }
}
