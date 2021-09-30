﻿using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
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
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = (from x in context.Projects
                             where x.ProjectId == projectId
                             select x);

                var entity = await query.FirstOrDefaultAsync();
                return entity.ToModel();
            }
           
        }
    }
}
