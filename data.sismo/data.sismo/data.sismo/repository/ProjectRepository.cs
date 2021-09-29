using common.sismo.interfaces.repositories;
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

        public async Task<SeismicProjectModel> GetSeismicProject(Int32 projectId)
        {
            using (var context = new SeismicContext())
            {
                var query = (from x in context.Projects
                             where x.ProjectId == projectId
                             select x);

                var entity =  await query.FirstOrDefaultAsync();
                return entity.ToModel();

            }
        }
    }
}
