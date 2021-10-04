using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace data.sismo.repository
{
  
    public class PlannedStretchRepository: IPlannedStretchRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public PlannedStretchRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<PlannedStretchModel> GetStretch(int surveyId, int operationalFrontId, string date, string line, decimal initialStation, decimal finalStation)
        {
            var newDate = DateHelper.StringToDate(date).Date;
            using var context = _contextFactory.CreateDbContext();
            var query = await context.PlannedStretches.Where(m => m.SurveyId == surveyId &&
                m.OperationalFrontId == operationalFrontId &&
                m.Line == line &&
                m.InitialStation == initialStation &&
                m.FinalStation == finalStation &&
                m.ExecutionDate == newDate).FirstOrDefaultAsync();
            return query.ToModel();
           
        }
        public async Task<bool> DeletePlannedStretch(PlannedStretchModel stretch)
        {
            var newDate = DateHelper.StringToDate(stretch.ExecutionDateString).Date;
            using var context = _contextFactory.CreateDbContext();
            var entity =  context.PlannedStretches.Where(m => m.SurveyId == stretch.SurveyId &&
                m.OperationalFrontId == stretch.OperationalFrontId &&
                m.Line == stretch.Line &&
                m.InitialStation == stretch.InitialStation &&
                m.FinalStation == stretch.FinalStation &&
                m.ExecutionDate == newDate).FirstOrDefault();

            if (entity == null)
                return false;

            context.PlannedStretches.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<PlannedStretchModel> GetPreviousStationt(PlannedStretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PlannedStretches.Where(
                m => m.SurveyId == model.SurveyId
                    && m.OperationalFrontId == model.OperationalFrontId
                    && m.Line == model.Line && m.FinalStation < model.InitialStation
                    ).OrderByDescending(m => m.InitialStation).FirstOrDefaultAsync();
            if (entity != null)
                return entity.ToModel();

            return null;
        }
        public async Task<PlannedStretchModel> GetNextStation(PlannedStretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.PlannedStretches.Where(
               m => m.SurveyId == model.SurveyId
                    && m.OperationalFrontId == model.OperationalFrontId
                    && m.Line == model.Line && m.InitialStation > model.FinalStation
                    ).OrderBy(m => m.InitialStation).FirstOrDefaultAsync();
            if (entity != null)
                return entity.ToModel();

            return null;
        }
        public async Task<List<PlannedStretchModel>> ListPlannedStretches(int surveyId,
            [Optional] int operationalFrontId,
            [Optional] string date,
            [Optional] string line,
            [Optional] decimal initialStation,
            [Optional] decimal finalStation,
            [Optional] int frontGroupLeaderId,
            [Optional] int frontGroupId)
        {
            using var context = _contextFactory.CreateDbContext();
            var convertedDate = !string.IsNullOrWhiteSpace(date) ? DateHelper.StringToDate(date).Date : DateTime.MinValue;
            var lineFilterExists = !string.IsNullOrWhiteSpace(line);
            var list = await context.PlannedStretches.Where(
                            m => m.SurveyId == surveyId &&
                                (operationalFrontId == 0 || m.OperationalFrontId == operationalFrontId) &&
                                (convertedDate == DateTime.MinValue || m.ExecutionDate == convertedDate) &&
                                (!lineFilterExists || line.Equals(m.Line)) &&
                                (initialStation == 0M || m.InitialStation == initialStation) &&
                                (finalStation == 0M || m.FinalStation == finalStation) &&
                                (frontGroupLeaderId == 0 || m.FrontGroupLeaderId == frontGroupLeaderId) &&
                                (frontGroupId == 0 || m.FrontGroupId == frontGroupId)).ToListAsync();

            return list.Any() ? list.Select(x=>x.ToModel()).OrderBy(m => m.FrontGroupName).ThenBy(m => m.Line).ToList() : new List<PlannedStretchModel>();
        }

        public async Task<bool> HasIntersectionedStretches(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.PlannedStretches.Where(m => m.SurveyId == surveyId
                               && m.OperationalFrontId == operationalFrontId
                               && m.Line == line
                               && !m.NotRealized &&
                               ((m.InitialStation <= initialStation && m.FinalStation >= initialStation)
                                   //left Border Intersection
                                   ||
                                   (m.InitialStation <= finalStation && m.FinalStation >= finalStation)
                                   //right Border Intersection
                                   ||
                                   (m.FinalStation <= finalStation && m.InitialStation >= initialStation)
                                   //inside intersection
                                   ||
                                   (m.FinalStation >= finalStation && m.InitialStation <= initialStation)
                                   //outside intersection
                                   )).AnyAsync();
        }

        public async Task AddStretch(PlannedStretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
           // model.LastUpdate = DateTime.Now;
            var entity = model.ToEntity();
            context.Add(entity);
            await context.SaveChangesAsync();
        }
    }
}
