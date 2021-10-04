using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    public class HoleCoordinateRepository : IHoleCoordinateRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public HoleCoordinateRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<HoleCoordinateModel> AddHoleCoordinate(HoleCoordinateModel holeCoord)
        {
            using var context = _contextFactory.CreateDbContext();
            var surveyCoords = context.HoleCoordinates.Where(m => m.SurveyId == holeCoord.SurveyId);
            holeCoord.HoleCoordinateId = !surveyCoords.Any() ? 1 : surveyCoords.Max(m => m.HoleCoordinateId) + 1;
            var entity = holeCoord.ToEntity();
            context.Add(entity);
            await context.SaveChangesAsync();
            holeCoord.HoleCoordinateId = entity.HoleCoordinateId;
            return holeCoord;
        }

        public async Task UpdateHolesCoordinates(IEnumerable<HoleCoordinateModel> holesCoordinates)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var hCoord in holesCoordinates)
            {
                var entity = context.HoleCoordinates.Where(m => m.HoleCoordinateId == hCoord.HoleCoordinateId && m.SurveyId == hCoord.SurveyId).FirstOrDefault();
                if (entity != null)
                    hCoord.Copy(entity);
                   
            }
            await context.SaveChangesAsync();
        }

        public async Task UpdateAllHoleCoordinates(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            await context.Database.ExecuteSqlRawAsync("UpdateAllHoleCoordinate "+ surveyId);
            context.Database.SetCommandTimeout(new TimeSpan(0));
                
        }

        public async Task<bool> DeleteHoleCoordinate(int surveyId, int holeCoordinateId)
        {
            //model.LastUpdate = DateTime.Now;
            using var context = _contextFactory.CreateDbContext();
            var entity = context.HoleCoordinates.Where(
                   m => m.SurveyId == surveyId && m.HoleCoordinateId == holeCoordinateId
                   ).FirstOrDefault();
            if (entity == null)
                return false;
            context.HoleCoordinates.Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteHolesCoordinates(int surveyId, int fileId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.HoleCoordinates.Where(
                   m => m.SurveyId == surveyId && m.FileId == fileId
                   ).FirstOrDefault();
            if (entity == null)
                return false;
            context.HoleCoordinates.Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from m in context.HoleCoordinates
                         where m.SurveyId == surveyId
                         select m);

            var entities = await query.Select(x => x.ToModel(toWkt, toSrid)).ToListAsync();
            return entities;
          
        }

        public async Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, int fileId, string toWkt, int toSrid)
        {

            using var context = _contextFactory.CreateDbContext();
            var query = (from m in context.HoleCoordinates
                         where m.SurveyId == surveyId && m.FileId == fileId
                         select m);

            var entities = await query.Select(x => x.ToModel(toWkt, toSrid)).ToListAsync();
            return entities;
         
        }

        public async Task<Geometry> GetHolesEnvelopeBufferGeometry(int surveyId, int fileId, double bufferSizeInDegrees)
        {
         
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.HoleCoordinates.Where(
                   m => m.SurveyId == surveyId && m.FileId == fileId && m.Coordinate!=null
                   ).Select(m => m.Coordinate).ToListAsync();
           var aggregation =  entity.Aggregate((hole, nextHole) => hole.Union(nextHole));
            return aggregation.Envelope.Buffer(bufferSizeInDegrees);
        }

        public async Task<Geometry> GetUnitedHolesCoordinatesGeometry(int surveyId, int fileId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.HoleCoordinates.Where(
                   m => m.SurveyId == surveyId && m.FileId == fileId
                   ).Select(m => m.Coordinate).ToListAsync();
                return entity.Aggregate((hole, nextHole) => hole.Union(nextHole));
        }

        public async Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, string line, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from m in context.HoleCoordinates
                         where m.SurveyId == surveyId && m.Line.Equals(line)
                         select m);

            var entities = await query.Select(x => x.ToModel(toWkt, toSrid)).ToListAsync();
            return entities;
        }

        public async Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, IEnumerable<int> preplotPointsIds, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from m in context.HoleCoordinates
                         where m.SurveyId == surveyId && m.PreplotPointId.HasValue && preplotPointsIds.Contains(m.PreplotPointId.Value)
                         select m);


            var entities = await query.Select(x => x.ToModel(toWkt, toSrid)).ToListAsync();
            return entities;
           
        }

        public async Task<List<HoleCoordinateModel>> ListHolesCoordinates(int surveyId, DateTime date, string line, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = (from m in context.HoleCoordinates
                         where m.SurveyId == surveyId &&
                                         (date == DateTime.MinValue ||
                                         new DateTime(m.File.UploadTime.Year,
                                                      m.File.UploadTime.Month,
                                                      m.File.UploadTime.Day, 0, 0, 0) == date.Date) &&
                                         (line == "" || line == null || line == m.Line)
                         select m);


            var entities = await query.Select(x => x.ToModel(toWkt, toSrid)).ToListAsync();
            return entities;
        }

        public async Task<HoleCoordinateModel> GetHoleCoordinate(int surveyId, int preplotPointId, int holeNumber, DateTime acquisitionTime, string toWkt, int toSrid)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.HoleCoordinates.Where(
                    m => m.SurveyId == surveyId && m.PreplotPointId == preplotPointId &&
                    m.HoleNumber == holeNumber && m.AcquisitionTime == acquisitionTime
                   ).FirstOrDefaultAsync();

            return entity.ToModel(toWkt, toSrid);
          
        }
    }
}
