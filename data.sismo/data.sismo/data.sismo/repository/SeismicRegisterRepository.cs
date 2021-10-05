using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
   

    public class SeismicRegisterRepository: ISeismicRegisterRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public SeismicRegisterRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> GetLastFfid(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var regs = await context.SeismicRegisters.Where(m => m.SurveyId == surveyId).ToListAsync() ;
            if (!regs.Any()) return 0;
            var lastFfid = regs.Max(m => m.Ffid);
            return lastFfid > 0 ? lastFfid : 0;
        }

        public async Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.SeismicRegisters.Where(
                m =>
                    m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId &&
                    m.PointProduction.PreplotPoint.Line == line &&
                    m.PointProduction.PreplotPoint.StationNumber >= initialStation &&
                    m.PointProduction.PreplotPoint.StationNumber <= finalStation).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, List<LineStretchModel> swathStretches)
        {
            using var context = _contextFactory.CreateDbContext();
            var registers = new List<SeismicRegister>();
            foreach (var stretch in swathStretches)
            {
                var stretch1 = stretch;
                var data = await context.SeismicRegisters.Where(
                    m =>
                        m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId &&
                        m.PointProduction.PreplotPoint.Line == stretch1.Line &&
                        m.PointProduction.PreplotPoint.StationNumber >= stretch1.InitialStation &&
                        m.PointProduction.PreplotPoint.StationNumber <= stretch1.FinalStation).ToListAsync();
                registers.AddRange(data);
            }
            return registers.Select(x => x.ToModel()).ToList();
        }

        public async Task<List<SeismicRegisterModel>> ListSeismicRegisters(int surveyId, int operationalFrontId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.SeismicRegisters.Where(
                m =>
                    m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId &&
                    m.Date == DateHelper.StringToDate(date)).ToListAsync();

            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task SaveSeismicRegister(SeismicRegisterModel model)
        {  
            using var context = _contextFactory.CreateDbContext();
            var entity = context.SeismicRegisters.Where(
                  m => m.SurveyId == model.SurveyId
                        && m.Ffid == model.Ffid
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
            
        }

        public async Task<List<int>> ListExistingFfids(List<int> ffids)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.SeismicRegisters.Where(m => ffids.Contains(m.Ffid)).Select(m => m.Ffid).ToListAsync();
        }

        public async Task SaveSeismicRegisters(IEnumerable<SeismicRegisterModel> registers)
        {
          
            foreach (var reg in registers)
                await SaveSeismicRegister(reg);
        }

        public async Task DeleteSeismicRegisters(IEnumerable<PointProductionModel> productions)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var regEntity in productions.Select(prodDto => context.SeismicRegisters.Where(m => m.SurveyId == prodDto.SurveyId && m.Ffid == prodDto.Ffid).FirstOrDefault()))
            {

                context.SeismicRegisters.Remove(regEntity);
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteSeismicRegisters(IEnumerable<SeismicRegisterModel> registers)
        {
            using var context = _contextFactory.CreateDbContext();
            foreach (var regEntity in registers.Select(regDto => context.SeismicRegisters.Where(m => m.SurveyId == regDto.SurveyId && m.Ffid == regDto.Ffid).FirstOrDefault()))
            {
                context.SeismicRegisters.Remove(regEntity);
            }
            await context.SaveChangesAsync();
        }
    }
}
