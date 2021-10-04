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
    public class FrontGroupRepository : IFrontGroupRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public FrontGroupRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<FrontGroupModel> GetFrontGroup(int frontGroupId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.FrontGroups.Where(
                   m => m.FrontGroupId == frontGroupId
                   ).FirstOrDefaultAsync();

            return entity.ToModel();
        }

        public async Task<string> GetFrontGroupName(int frontGroupId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.FrontGroups.Where(
                   m => m.FrontGroupId == frontGroupId
                   ).FirstOrDefaultAsync();

            return entity.Name;
        }

        public async Task<List<FrontGroupModel>> ListFrontGroups(int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.FrontGroups
                         where x.OperationalFrontId == operationalFrontId
                         && x.IsActive == true
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }
        public async Task<List<FrontGroupModel>> ListAllFrontGroups()
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from x in context.FrontGroups
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task<FrontGroupModel> SaveFrontGroup(FrontGroupModel model)
        {
            //model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.FrontGroups.Where(
                   m => m.FrontGroupId == model.FrontGroupId
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
            model.FrontGroupId = entity.FrontGroupId;
            return model;
        }
    }
}
