using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace data.sismo.repository
{
    public class SurveyRepository : ISurveyRepository
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SurveyRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SurveyModel> GetSurvey(int surveyId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var query = (from x in context.Surveys
                             where x.SurveyId == surveyId
                             select x);

                var entity = await query.FirstOrDefaultAsync();
                return entity.ToModel();
            }
        }
    }
}
