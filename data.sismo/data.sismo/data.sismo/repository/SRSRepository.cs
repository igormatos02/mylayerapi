using common.sismo.enums;
using common.sismo.models;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    public class SRSRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SRSRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;

        }

        public async Task<SRSModel> GetSRS(int srId)
        {
            using var context = _contextFactory.CreateDbContext();
            var srsPar = new SqlParameter("@Srid", srId);
            var temp = await context.ProspatialReferenceSystems.FromSqlRaw<ProspatialReferenceSystem>("execute ListSpatialReferences @Srid", srsPar).FirstOrDefaultAsync();
            return new SRSModel { SRSId = temp.SpatialReferenceId,WKT = temp.WellKnownText, SRSName = temp.Geogcs};
            
        }

        //public async Task<List<SRSModel>> ListSRS(CoordinateSystem coordinateSystemId)
        //{
        //    using var context = _contextFactory.CreateDbContext();

        //    var temp = await context.ProspatialReferenceSystems.FromSqlRaw<ProspatialReferenceSystem>("execute ListSpatialReferences").ToListAsync();
          
        //    return temp.Where(t => coordinateSystemId == (int)t.SpatialReferenceId ? t.WellKnownText.StartsWith("GEOGCS") : !t.WellKnownText.StartsWith("GEOGCS")).Select(x=> new SRSModel { SRSId = x.SpatialReferenceId, WKT = x.WellKnownText, SRSName = x.Geogcs }).ToList();

        //}

        //public async Task<List<SRSModel>> ListSpatialReferences()
        //{
        //    using (var context = new CPEntities())
        //    {
        //        IList<SRSDTO> result = context.Database.SqlQuery<SRSDTO>("execute ListSpatialReferences").ToList();
        //        return result;
        //    }
        //}

       
    }
}
