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
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ProjectRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<SeismicProjectModel> GetSeismicProject(Int32 projectId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.Projects
                         where x.ProjectId == projectId
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();

        }

        public async Task<List<SeismicProjectModel>> ListProjects(bool? isActive=true)
        {

            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.Projects
                         where isActive == null || isActive == x.IsActive
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task AddProject(SeismicProjectModel project)
        {
            using var context = _contextFactory.CreateDbContext();
            project.LastUpdate = DateTime.Now;
            context.Add(project.ToEntity());
            await context.SaveChangesAsync();
        }

        public async Task UpdateProject(SeismicProjectModel modifiedProject)
        {
            using var context = _contextFactory.CreateDbContext();
            modifiedProject.LastUpdate = DateTime.Now;
            var entity = context.Projects.Where(m => m.ProjectId == modifiedProject.ProjectId).FirstOrDefault();
            modifiedProject.Copy(entity);
            await context.SaveChangesAsync();
        }
    }
}
