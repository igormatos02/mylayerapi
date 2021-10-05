using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    
    public class SRSRepository : ISRSRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SRSRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }

        public async Task<SRSModel> GetSRS(int srId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
              new SqlParameter("@Srid", srId)
            };
            var temp = await context.ProspatialReferenceSystems.FromSqlRaw<ProspatialReferenceSystem>("execute ListSpatialReferences @Srid", parameters).FirstOrDefaultAsync();
            return new SRSModel { SRSId = temp.SpatialReferenceId,WKT = temp.WellKnownText, SRSName = temp.Geogcs};
            
        }

        public async Task<List<SRSModel>> ListSRS(CoordinateSystem coordinateSystemId)
        {
            using var context = _contextFactory.CreateDbContext();
            
            var models = await context.Set<ProspatialReferenceSystem>().FromSqlRaw("execute ListSpatialReferences").ToListAsync();
          
            return models.Where(t => coordinateSystemId == (CoordinateSystem)t.SpatialReferenceId ? t.WellKnownText.StartsWith("GEOGCS") : !t.WellKnownText.StartsWith("GEOGCS")).Select(x => new SRSModel { SRSId = x.SpatialReferenceId, WKT = x.WellKnownText, SRSName = x.Geogcs }).ToList();

        }

        public async Task<List<SRSModel>> ListSpatialReferences()
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.Set<ProspatialReferenceSystem>().FromSqlRaw("execute ListSpatialReferences").ToListAsync();
            return entities.Select(x => new SRSModel() { SRSId = x.SpatialReferenceId, WKT = x.WellKnownText, SRSName = x.Geogcs }).ToList();
        }


    }
}
