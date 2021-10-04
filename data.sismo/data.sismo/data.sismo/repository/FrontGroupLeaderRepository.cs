using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data.sismo.repository
{
   
    public class FrontGroupLeaderRepository : IFrontGroupLeaderRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public FrontGroupLeaderRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<FrontGroupLeaderModel> GetFrontGroupLeader(int frontGroupLeaderId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.FrontGroupLeaders
                         where x.FrontGroupLeaderId == frontGroupLeaderId
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();
        }
      
        public async Task<string> GetFrontGroupLeaderName(int frontGroupLeaderId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.FrontGroupLeaders.Where(
                   m => m.FrontGroupLeaderId == frontGroupLeaderId
                   ).FirstOrDefaultAsync();

            return entity.Name;
        }

        public async Task<List<FrontGroupLeaderModel>> ListFrontGroupLeaders(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.FrontGroupLeaders
                         where x.OperationalFrontId == operationalFrontId
                         && x.IsActive == true
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }
        public async Task<List<FrontGroupLeaderModel>> ListAllFrontGroupLeaders()
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.FrontGroupLeaders
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task<FrontGroupLeaderModel> SaveFrontGroupLeader(FrontGroupLeaderModel model)
        {
            //model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.FrontGroupLeaders.Where(
                   m => m.FrontGroupLeaderId == model.FrontGroupLeaderId
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
            model.FrontGroupLeaderId = entity.FrontGroupLeaderId;
            return model;

        }
    }
}
