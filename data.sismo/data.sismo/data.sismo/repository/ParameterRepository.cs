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
    public class ParameterRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ParameterRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }
        public async Task<ParameterModel> GetParameter(int ParameterId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query =  await context.Parameters.Where(m => m.ParameterId == ParameterId).FirstOrDefaultAsync();
            return query.ToModel(); 
        }

        public async Task<List<ParameterModel>> ListParameters()
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.Parameters
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }
    }
    
}
