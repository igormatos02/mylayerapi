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
    public class ParameterGroupRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ParameterGroupRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }
        public async Task<ParameterGroupModel> GetParameter(int ParameterGroupId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = await context.ParameterGroups.Where(m => m.ParameterGroupId == ParameterGroupId).FirstOrDefaultAsync();
            return query.ToModel();
        }

        public async Task<List<ParameterGroupModel>> ListParameters()
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.ParameterGroups
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

    }
}
