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
    public class ProjectExplosiveMaterialTypeRepository : IProjectExplosiveMaterialTypeRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ProjectExplosiveMaterialTypeRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task<ProjectExplosiveMaterialTypeModel> GetProjectExplosiveMaterialType(int idProjectExplosiveMaterialType)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.ProjectExplosiveMaterialTypes.FirstOrDefaultAsync(m => m.ProjectExplosiveMaterialTypeId == idProjectExplosiveMaterialType);
            return entity.ToModel();
        }

        public async Task<List<ProjectExplosiveMaterialTypeModel>> ListProjectExplosiveMaterialTypes(int projectId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.ProjectExplosiveMaterialTypes.Where(m => m.ProjectId == projectId).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task SaveProjectExplosiveMaterialTypes(ProjectExplosiveMaterialTypeModel dto)
        {
            using var context = _contextFactory.CreateDbContext();
            // dto.LastUpdate = DateTime.Now;
            var entity = await context.ProjectExplosiveMaterialTypes.FirstOrDefaultAsync(
                   m => m.ProjectExplosiveMaterialTypeId == dto.ProjectExplosiveMaterialTypeId
                   );
            if (entity != null)
            {

                dto.Copy(entity);
                await context.SaveChangesAsync();
            }
            else
            {

                context.ProjectExplosiveMaterialTypes.Add(dto.ToEntity());
                await context.SaveChangesAsync();
            }
        }
    }
}
