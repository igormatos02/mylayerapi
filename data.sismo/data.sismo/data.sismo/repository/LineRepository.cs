using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace data.sismo.repository
{

   
    public class LineRepository : ILineRepository
    {

        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public LineRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<LineModel> GetLine(int surveyId, string lineName)
        {
            using var context = _contextFactory.CreateDbContext();
            var versionedLine = context.Lines.Where(m => m.SurveyId == surveyId && m.LineName == lineName);
            if (!versionedLine.Any()) return new LineModel();

            var lineLastVersion = versionedLine.Max(m => m.PreplotVersionId);
            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.LineName == lineName && x.PreplotVersionId == lineLastVersion
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<LineModel> GetLine(int surveyId, string lineName, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();
         
            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.LineName == lineName && x.PreplotVersionId == preplotVersionId
                         select x);

            var entity = await query.FirstOrDefaultAsync();
            return entity.ToModel();
        }

        public async Task<List<LineModel>> ListLines(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();

            var versionedLines = await context.Lines.Where(m => m.SurveyId == surveyId).ToListAsync();
            if (!versionedLines.Any()) return new List<LineModel>();

            var lineLastVersion = versionedLines.Max(m => m.PreplotVersionId);

            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.PreplotVersionId == lineLastVersion
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;
        }

        public async Task<List<LineModel>> ListLines(int surveyId, PreplotPointType linesPointType)
        {
            using var context = _contextFactory.CreateDbContext();

            var versionedLines = await context.Lines.Where(m => m.SurveyId == surveyId).ToListAsync(); 
            if (!versionedLines.Any()) return new List<LineModel>();

            var lineLastVersion = versionedLines.Max(m => m.PreplotVersionId);
            var entities = context.Lines.Where(m=> m.SurveyId == surveyId && m.PreplotVersionId == lineLastVersion && m.LinePointsType == (int)linesPointType);
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<LineModel>> ListLines(int surveyId, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.PreplotVersionId == preplotVersionId
                         select x);

            var entities = await query.Select(x => x.ToModel()).ToListAsync();
            return entities;

        }

        public async Task<List<LineModel>> ListLines(int surveyId, int preplotVersionId, int linesPointType)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve((PreplotPointType)linesPointType);
           
            var entities = await context.Lines.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == preplotVersionId && pointTypes.Contains((PreplotPointType)m.LinePointsType)).ToListAsync();
            return entities.Select(x => x.ToModel()).OrderBy(s => Convert.ToInt32(s.LineName)).ToList();

        }

        public async Task<List<LineModel>> ListSummarizedLines(int surveyId, int operationalfrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                new SqlParameter("@surveyId", surveyId) { Direction = ParameterDirection.Input,SqlDbType = SqlDbType.Int },
                new SqlParameter("@OperationalFrontId", operationalfrontId) { Direction = ParameterDirection.Input, SqlDbType = SqlDbType.Int},
            };
            var models = await context.Set<LineModel>().FromSqlRaw("execute [ListSummarizedLines] @surveyId,@OperationalFrontId",parameters).ToListAsync();
            return models;
        }

        public async Task<List<string>> ListLinesNames(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();

            var versionedLines = await context.Lines.Where(m => m.SurveyId == surveyId).ToListAsync();
            if (!versionedLines.Any()) return new List<string>();

            var lineLastVersion = versionedLines.Max(m => m.PreplotVersionId);

            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.PreplotVersionId == lineLastVersion
                         select x);

            var entities = await query.Select(x => x.LineName).ToListAsync();
            return entities;

        }

        public async Task<List<string>> ListLinesNames(int surveyId, PreplotPointType linePointsType)
        {
            using var context = _contextFactory.CreateDbContext();

            var versionedLines = await context.Lines.Where(m => m.SurveyId == surveyId).ToListAsync();
            if (!versionedLines.Any()) return new List<string>();

            var lineLastVersion = versionedLines.Max(m => m.PreplotVersionId);
            var entities = context.Lines.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == lineLastVersion && m.LinePointsType == (int)linePointsType);
            return entities.Select(x => x.LineName).ToList();

        }

        public async Task<List<string>> ListLinesNames(int surveyId, int preplotVersionId)
        {
            using var context = _contextFactory.CreateDbContext();

            var query = (from x in context.Lines
                         where x.SurveyId == surveyId && x.PreplotVersionId == preplotVersionId
                         select x);

            var entities = await query.Select(x => x.LineName).ToListAsync();
            return entities;

        }

        public async Task<List<string>> ListLinesNames(int surveyId, int preplotVersionId, PreplotPointType linePointsType)
        {
            using var context = _contextFactory.CreateDbContext();
            var pointTypes = PreplotTypeResolver.Resolve(linePointsType);

            var entities = await context.Lines.Where(m => m.SurveyId == surveyId && m.PreplotVersionId == preplotVersionId && pointTypes.Contains((PreplotPointType)m.LinePointsType)).OrderBy(s => Convert.ToInt32(s.LineName)).ToListAsync();
            return entities.Select(x=>x.LineName).ToList();
        }

        public async Task<LineModel> SaveLine(LineModel line)
        {

            using var context = _contextFactory.CreateDbContext();
            var entity = context.Lines.Where(m=>
                       m.SurveyId == line.SurveyId
                    && m.LineName == line.LineName
                    && m.PreplotVersionId == line.PreplotVersionId
                   ).FirstOrDefault();
            if (entity == null)
            {
                context.Add(line.ToEntity());
            }
            else
            {
                line.Copy(entity);
            }
            await context.SaveChangesAsync();
            return line;

        }

        public async Task SaveLines(IEnumerable<LineModel> lines)
        {
            foreach (var line in lines)
                await SaveLine(line);
        }
    }
}
