using common.sismo.enums;
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
    public class HoleRepository : IHoleRepository   
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public HoleRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<HoleModel> GetHole(int surveyId, int preplotPointId, int preplotVersionId,
             PreplotPointType preplotPointType, int workNumber, int operationalFrontId, int holeNumber)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await GetHoleEntity(surveyId, preplotPointId, preplotVersionId,
                    preplotPointType, workNumber, operationalFrontId, holeNumber);
            return entity.ToModel();
        }

        private async Task<Hole> GetHoleEntity(int surveyId, int preplotPointId, int preplotVersionId,
            PreplotPointType preplotPointType, int workNumber, int operationalFrontId, int holeNumber)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Holes.FirstOrDefaultAsync(m => m.SurveyId == surveyId && m.PreplotPointId == preplotPointId &&
                                  m.PreplotVersionId == preplotVersionId &&
                                  m.PreplotPointType == (int)preplotPointType &&
                                  m.WorkNumber == workNumber && m.OperationalFrontId == operationalFrontId &&
                                  m.HoleNumber == holeNumber);
            return entity;
        }

        /// <summary>
        /// List the holes of a preplot point
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="preplotPointId"></param>
        /// <param name="preplotVersionId"></param>
        /// <param name="preplotPointType"></param>
        /// <param name="operationalFrontId"></param>
        /// <returns></returns>
        public async Task<List<HoleModel>> ListHoles(int surveyId, int preplotPointId, int preplotVersionId,
            PreplotPointType preplotPointType, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.Holes.Where(m => m.SurveyId == surveyId && m.PreplotPointId == preplotPointId &&
                                            m.PreplotVersionId == preplotVersionId &&
                                            m.PreplotPointType == (int)preplotPointType &&
                                            m.OperationalFrontId == operationalFrontId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task DeleteHoles(PointProductionModel production)
        {
            using var context = _contextFactory.CreateDbContext();
            var holes = await context.Holes.Where(m => m.SurveyId == production.SurveyId && m.PreplotPointId == production.PreplotPointId &&
                                            m.PreplotVersionId == production.PreplotVersionId &&
                                            m.PreplotPointType == (int)production.PreplotPointType &&
                                            m.OperationalFrontId == production.OperationalFrontId).ToListAsync();

            var filteredHoles = holes.Where(holeEntity => holeEntity != null);
            await DeleteHoles(filteredHoles.Select(x=>x.ToModel()));
        }

        public async Task DeleteHoles(IEnumerable<HoleModel> holes)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var hole in holes) {
                var holeEntity = await GetHoleEntity(hole.SurveyId,hole.PreplotPointId,hole.PreplotVersionId,hole.PreplotPointType,hole.WorkNumber,hole.OperationalFrontId,hole.HoleNumber);
                context.Holes.Remove(holeEntity);
            }
            await context.SaveChangesAsync();

        }

        public async Task AddHoles(IEnumerable<HoleModel> holes)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var holeModel in holes)
            {
                context.Holes.Add(holeModel.ToEntity());
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateHoles(IEnumerable<HoleModel> holes)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var holeModel in holes)
            {
                var holeEntity = await GetHoleEntity(holeModel.SurveyId, holeModel.PreplotPointId, holeModel.PreplotVersionId, holeModel.PreplotPointType,
                    holeModel.WorkNumber, holeModel.OperationalFrontId, holeModel.HoleNumber);
                if (holeEntity != null)
                {
                    holeModel.Copy(holeEntity);
                    await context.SaveChangesAsync();
                   
                }
            }
        }
    }
}
