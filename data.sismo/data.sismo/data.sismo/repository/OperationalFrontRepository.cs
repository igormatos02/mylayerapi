using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace data.sismo.repository
{
    public class OperationalFrontRepository : IOperationalFrontRepository
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public OperationalFrontRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<OperationalFrontModel> GetOperationalFront(int operationalFrontId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = (from x in context.OperationalFronts
                             where x.OperationalFrontId == operationalFrontId
                             select x);

                var entity = await query.FirstOrDefaultAsync();
                return entity.ToModel();
            }
        }
    }
}
