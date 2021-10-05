using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    
    public class StretchRepository: IStretchRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public StretchRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<StretchModel> GetStretch(int surveyId, int operationalFrontId, int frontGroupId, int frontGroupLeaderId, string line, string date, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var newDate = DateHelper.StringToDate(date).Date;
            var entity = await context.Stretches.Where(m => m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId && m.FrontGroupId == frontGroupId && m.FrontGroupLeaderId == frontGroupLeaderId && m.Line == line && m.InitialStation == initialStation && m.FinalStation == finalStation && m.Date == newDate).FirstOrDefaultAsync();
            return entity.ToModel();
        }
        public async Task<StretchModel> GetStretchByInitialStation(int surveyId, int operationalFrontId, string line, decimal initialStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =  await context.Stretches.Where(m => m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId && m.Line == line && m.InitialStation == initialStation).FirstOrDefaultAsync();
            return entity.ToModel();
        }
        public async Task<StretchModel> GetStretchByFinalStation(int surveyId, int operationalFrontId, string line, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Stretches.Where(m => m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId && m.Line == line && m.FinalStation == finalStation).FirstOrDefaultAsync();
            return entity.ToModel();
        }
        public async Task<bool> DeleteStretch(StretchModel stretch)
        {
            using var context = _contextFactory.CreateDbContext();
            var newDate = DateHelper.StringToDate(stretch.DateString).Date;
            var entity = await context.Stretches.Where(m => m.SurveyId == stretch.SurveyId &&
            m.OperationalFrontId == stretch.OperationalFrontId && m.FrontGroupId == stretch.FrontGroupId && 
            m.FrontGroupLeaderId == stretch.FrontGroupLeaderId && m.Line == stretch.Line &&
            m.InitialStation == stretch.InitialStationBkp && m.FinalStation == stretch.FinalStationBkp && 
            m.Date == newDate).FirstOrDefaultAsync();

            if (entity == null) return false;
            context.Stretches.Remove(entity);
            return true;
        }
        public async Task<String> GetFirstProductionDate(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            DateTime date = await context.Stretches.Where(s => s.SurveyId == surveyId).MinAsync(m => m.Date);
            if (date == null)
                return "";
            else return date.ToString("dd/MM/yyyy");
        }
        public async Task<String> GetLastProductionDate(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            DateTime date = await context.Stretches.Where(s => s.SurveyId == surveyId).MaxAsync(m => m.Date);
            if (date == null)
                return "";
            else return date.ToString("dd/MM/yyyy");
        }
        public async Task<StretchModel> GetPreviousStationt(StretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var stretch = await context.Stretches.Where(
                m => m.SurveyId == model.SurveyId
                    && m.OperationalFrontId == model.OperationalFrontId
                    && m.Line == model.Line && m.FinalStation < model.InitialStation
                    ).OrderByDescending(m => m.InitialStation).FirstOrDefaultAsync();

            if (stretch != null)
            {
                return stretch.ToModel();
            }
            return model;

        }
        public async Task<decimal> CalculateKm(int surveyId, PreplotPointType preplotPointType, string line, decimal initialStation, decimal finalStation,  Geometry geomLine,  int countPoints)
        {
            using var context = _contextFactory.CreateDbContext();
            decimal totalKm = 0;
            countPoints = 0;
           
            var totalKmParameter = new SqlParameter("@totalKm", totalKm);
                totalKmParameter.DbType = DbType.Decimal;
                totalKmParameter.Direction = ParameterDirection.Output;
                totalKmParameter.Size = 18;
                totalKmParameter.Scale = 4;

                var geomLineParameter = new SqlParameter("@geomLine", SqlDbType.Udt);
                geomLineParameter.Direction = ParameterDirection.Output;
                geomLineParameter.UdtTypeName = "geometry";

                var countPointsParameter = new SqlParameter("@countPoints", countPoints);
                countPointsParameter.DbType = DbType.Int32;
                countPointsParameter.Direction = ParameterDirection.Output;

            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@preplotPointType", preplotPointType),
                    new SqlParameter("@line", line),
                    new SqlParameter("@initialStation", initialStation),
                    new SqlParameter("@finalStation", finalStation),
                    totalKmParameter,
                    geomLineParameter,
                    countPointsParameter
            };
            await context.Database.ExecuteSqlRawAsync("execute [CalculateKm] @surveyId,@preplotPointType,@line,@initialStation,@finalStation,@totalKm output,@geomLine output, @countPoints output", parameters);

           
                totalKm = Convert.ToDecimal(totalKmParameter.Value);
                countPoints = Convert.ToInt32(countPointsParameter.Value);
                var geomWkt = Convert.ToString(geomLineParameter.Value);
                geomLine = null;
           

            if (geomLineParameter.Value.ToString() == "Null")
                    geomLine = null;
                else
                {
                    try
                    {
                    WKTReader reader = new WKTReader(GeometryFactory.Default);
                    var geom = reader.Read(geomWkt);
                    geomLine = geom;
                    //geomLine = new DBGeometry() Geometry.LineFromText(geomWkt, 4326);
                }
                    catch (Exception ex) { }
                }
                return totalKm;
           
        }
        public async Task<StretchModel> GetNextStation(StretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var stretch = await context.Stretches.Where(
                m => m.SurveyId == model.SurveyId
                    && m.OperationalFrontId == model.OperationalFrontId
                    && m.Line == model.Line && m.InitialStation > model.FinalStation
                    ).OrderBy(m => m.InitialStation).FirstOrDefaultAsync();

            if (stretch != null)
            {
                return stretch.ToModel();
            }
            return model;

        }
        public async Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId, string date)
        {
            using var context = _contextFactory.CreateDbContext();
            var newDate = DateHelper.StringToDate(date).Date;
            var list = await context.Stretches.Where(
                            m => m.SurveyId == idSurvey && m.OperationalFrontId == operationalFrontId && m.Date == newDate).ToListAsync();

            var entities = list.OrderBy(m => m.FrontGroup==null?"":m.FrontGroup.Name).ThenBy(m => m.Line);

            return entities.Any() ? entities.Select(x=>x.ToModel()).ToList() : new List<StretchModel>();
        }
        public async Task<List<StretchModel>> ListStretches(int idSurvey, string lastDate)
        {
            using var context = _contextFactory.CreateDbContext();
            var lastDateFormatted = DateHelper.StringToDate(lastDate).Date;
            var list = await context.Stretches.Where(
                            m =>
                                m.SurveyId == idSurvey && m.Date <= lastDateFormatted).ToListAsync();

            var entities = list.OrderBy(m => m.FrontGroup == null ? "" : m.FrontGroup.Name).ThenBy(m => m.Line);
            return entities.Any() ? entities.Select(x => x.ToModel()).ToList() : new List<StretchModel>();
        }
        public async Task<List<StretchModel>> ListStretches(int idSurvey)
        {
            using var context = _contextFactory.CreateDbContext();
            var list = await context.Stretches.Where(m => m.SurveyId == idSurvey).ToListAsync();

            var entities = list.OrderBy(m => m.FrontGroup == null ? "" : m.FrontGroup.Name).ThenBy(m => m.Line);
            return entities.Any() ? entities.Select(x => x.ToModel()).ToList() : new List<StretchModel>();
        }

        public async Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var list = await context.Stretches.Where(m => m.SurveyId == idSurvey 
            && m.OperationalFrontId == operationalFrontId).ToListAsync();

            var entities = list.OrderBy(m => m.FrontGroup == null ? "" : m.FrontGroup.Name).ThenBy(m => m.Line);
            return entities.Any() ? entities.Select(x => x.ToModel()).ToList() : new List<StretchModel>();
        }

        public async Task<List<StretchModel>> ListStretches(int idSurvey, int operationalFrontId, string initialDate, string finalDate)
        {
            using var context = _contextFactory.CreateDbContext();
            var iniDate = DateHelper.StringToDate(initialDate).Date;
            var finDate = DateHelper.StringToDate(finalDate).Date;
            var list = await context.Stretches.Where(m => m.SurveyId == idSurvey && m.OperationalFrontId == operationalFrontId
                    && m.Date >= iniDate && m.Date <= finDate).ToListAsync();

            var entities = list.OrderBy(m => m.FrontGroup == null ? "" : m.FrontGroup.Name).ThenBy(m => m.Line);
            return entities.Any() ? entities.Select(x => x.ToModel()).ToList() : new List<StretchModel>();
        }

        public async Task<List<StretchModel>> ListStretchesWithIntersection(int surveyId, int operationalFrontId, int frontGroupId, int frontGroupLeaderId, string line, string date, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            var newDate = DateHelper.StringToDate(date).Date;
            var entities = await context.Stretches.Where(m => m.SurveyId == surveyId
                && m.OperationalFrontId == operationalFrontId && m.FrontGroupId == frontGroupId
                && m.FrontGroupLeaderId == frontGroupLeaderId && m.Line == line &&
                (
                     (m.InitialStation <= initialStation && m.FinalStation >= initialStation) //left Border Intersection
                     ||
                     (m.InitialStation <= finalStation && m.FinalStation >= finalStation) //right Border Intersection
                     ||
                     (m.FinalStation <= finalStation && m.InitialStation >= initialStation) //inside intersection
                     ||
                     (m.FinalStation >= finalStation && m.InitialStation <= initialStation) //outside intersection
                )
                && m.Date == newDate).ToListAsync();
            return entities.Any() ? entities.Select(x => x.ToModel()).ToList() : new List<StretchModel>();
        }
        public async Task<bool> HasAnyIntersectionedStretches(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Stretches.Where(m => m.SurveyId == surveyId
                               && m.OperationalFrontId == operationalFrontId
                               && m.Line == line &&
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

        public async Task SaveStretch(StretchModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var newDate = DateHelper.StringToDate(model.DateString).Date;
            var entity = await context.Stretches.Where(m => m.SurveyId == model.SurveyId && m.OperationalFrontId == model.OperationalFrontId
            && m.FrontGroupId == model.FrontGroupId && m.FrontGroupLeaderId == model.FrontGroupLeaderId && m.Line == model.Line 
            && m.InitialStation == model.InitialStation && m.FinalStation == model.FinalStation && m.Date == newDate).FirstOrDefaultAsync();
            if (entity != null)
            {
                model.Copy(entity);
                await context.SaveChangesAsync();
            }
            else
            {
                context.Stretches.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TotalDailyProductionModel>> ListTotalDailyProductions(int surveyId, int operationalFrontId, string line, decimal? point, bool hasUnaccomplished)
        {
            using var context = _contextFactory.CreateDbContext();
            List<TotalDailyProductionModel> totalDailyProductionList = new List<TotalDailyProductionModel>();
           
                Expression<Func<Stretch, bool>> predicate = m => m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId && (!hasUnaccomplished || m.TotalNotRealized > 0);
                if (!string.IsNullOrWhiteSpace(line))
                    predicate = m => m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId && (!hasUnaccomplished || m.TotalNotRealized > 0) &&
                        m.Line == line;


            var dailyProduction = await context.Stretches.Where(predicate).Where(m => (m.InitialStation <= point.Value && m.FinalStation >= point.Value) || point.Value == -1).Select(m => new { m.Date, m.Km }).ToListAsync(); 
                var dates = dailyProduction.Select(m => m.Date).Distinct();

                foreach (var date in dates)
                {

                    var stretches = context.Stretches.Where(m => m.Date == date && m.SurveyId == surveyId && m.OperationalFrontId == operationalFrontId);
                    var StrechesDtos = new List<StretchModel>();
                    foreach (Stretch s in stretches)
                    {
                        StrechesDtos.Add(new StretchModel() { Line = s.Line, InitialStation = s.InitialStation, FinalStation = s.FinalStation });

                    }

                    totalDailyProductionList.Add(new TotalDailyProductionModel()
                    {
                        TotalRealized = stretches.Sum(m => m.TotalRealized) ?? 0 + stretches.Sum(m => m.TotalRealizedOnlyStations) ?? 0,
                        TotalNotRealized = stretches.Sum(m => m.TotalNotRealized) ?? 0 + stretches.Sum(m => m.TotalPending) ?? 0,
                        TotalKm = stretches.Sum(m => m.Km) ?? 0,
                        Date = DateHelper.DateToString(date),
                        DateTime = date,
                        Stretches = StrechesDtos
                    });
                }
           
            return totalDailyProductionList.OrderByDescending(m => m.DateTime).ToList();
        }

        public async Task<List<OpFrontTotalProductionGraphModel>> GetSurveyProductionData(int surveyId, String date)
        {
            using var context = _contextFactory.CreateDbContext();

            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@lastDate", date == "" ? DateTime.Now : DateHelper.GetDBValue(date, "NormalDate"))
            };
            var opFrontProductionsList = await context.Set<OpFrontTotalProductionGraphModel>().FromSqlRaw("execute [GetSurveyProductionData] @surveyId, @lastDate", parameters).ToListAsync();

            

                foreach (var opFrontProductions in opFrontProductionsList)
                {
                if (opFrontProductions.TotalPoints > 0)
                {

                    opFrontProductions.TotalMissing = opFrontProductions.TotalPoints - (opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized);
                    opFrontProductions.PercentRealized = (decimal)opFrontProductions.TotalRealized / (decimal)opFrontProductions.TotalPoints * 100;
                    opFrontProductions.PercentNotRealized = (decimal)opFrontProductions.TotalNotRealized / (decimal)opFrontProductions.TotalPoints * 100;
                    opFrontProductions.PercentMissing = (decimal)opFrontProductions.TotalMissing / (decimal)opFrontProductions.TotalPoints * 100;

                    var kmRealizedBkp = opFrontProductions.KmRealized;
                    opFrontProductions.KmRealized = kmRealizedBkp * opFrontProductions.TotalRealized /
                        ((opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized) <= 0 ?
                        1 : (opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized));
                    opFrontProductions.KmNotRealized = kmRealizedBkp * opFrontProductions.TotalNotRealized /
                        ((opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized) <= 0 ?
                        1 : (opFrontProductions.TotalRealized + opFrontProductions.TotalNotRealized));
                    opFrontProductions.KmMissing = opFrontProductions.KmTotal - kmRealizedBkp;
                    var tempFrontProd = await context.Set<OpFrontTotalProductionGraphModel>().FromSqlRaw(
                         "select " +
                         "  case " +
                         "    when DatumId = 4326 then CONVERT(decimal(30, 15), 0)" +
                         "    when DatumId <> 4326 then CONVERT(decimal(30, 15), dbo.GeometryToGeometry(Polygon, DatumId).STArea() / 1000000) " +
                         "  end as AreaTotal" +
                         "from survey " +
                         "where surveyId =" + surveyId).FirstOrDefaultAsync();
                    opFrontProductions.AreaTotal = tempFrontProd.AreaTotal;
                    opFrontProductions.AreaRealized = opFrontProductions.TotalRealized * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);
                    opFrontProductions.AreaNotRealized = opFrontProductions.TotalNotRealized * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);
                    opFrontProductions.AreaMissing = opFrontProductions.TotalMissing * (opFrontProductions.AreaTotal / opFrontProductions.TotalPoints);

                }
                
              opFrontProductions.RealizedStatusName = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Accomplished, (OperationalFrontType)opFrontProductions.OperationalFrontType);
                    opFrontProductions.NotRealizedStatusName = EnumHelper.GetOperationalFrontEnumDescription(ProductionStatus.Unaccomplished, (OperationalFrontType)opFrontProductions.OperationalFrontType);
                    //opFrontProductions.MissingStatusName = Enumerations.GetEnumDescription(ProductionStatus.Pending);
                }
                return opFrontProductionsList;
           
        }

        public async Task<List<RdoFrontOverFrontChartPointDataModel>> GetFrontOverFrontChartSerie(int surveyId, DateTime reportDate,
            int operationalFrontId)
        {
            using var context = _contextFactory.CreateDbContext();
            var parameters = new[] {
                    new SqlParameter("@surveyId", surveyId),
                    new SqlParameter("@reportDate", reportDate),
                    new SqlParameter("@operationalFrontId", operationalFrontId)
            };
            var opFrontProductionsList = await context.Set<RdoFrontOverFrontChartPointDataModel>().FromSqlRaw("execute [GetFrontOverFrontChartData] @surveyId, @reportDate, @operationalFrontId", parameters).ToListAsync();
            return opFrontProductionsList;
        }

        public async Task<List<ProductionPerLineChartSerieModel>> GetProductionPerLineSeries(int surveyId, string reportDate, IList<string> consideringLines, IList<OperationalFrontModel> operationalFronts)
        {
            using var context = _contextFactory.CreateDbContext();
            var date = DateHelper.StringToDate(reportDate);
            var stretches = await context.Stretches.Where(m => m.SurveyId == surveyId && m.Date <= date && consideringLines.Contains(m.Line)).OrderBy(m => m.Line).ToListAsync();
            var series = new List<ProductionPerLineChartSerieModel>();

            foreach (var opFront in operationalFronts)
            {
                var front = opFront;
                var frontStretches = stretches.Where(m => m.OperationalFrontId == front.OperationalFrontId).OrderBy(m => m.Line);
                var serieItem = new ProductionPerLineChartSerieModel { name = opFront.Name };
                foreach (var frontAndLineStretches in consideringLines.Select(line => frontStretches.Where(m => m.Line == line).ToList()))
                    serieItem.data.Add((frontAndLineStretches.Sum(m => m.TotalRealized) ?? 0) +
                                       (frontAndLineStretches.Sum(m => m.TotalNotRealized) ?? 0) +
                                       (frontAndLineStretches.Sum(m => m.TotalPending) ?? 0));

                serieItem.color = opFront.OperationalFrontColor;
                series.Add(serieItem);
            }
            return series;
        }
    }
}
