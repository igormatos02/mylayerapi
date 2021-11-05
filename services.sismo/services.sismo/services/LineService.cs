using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces;
using common.sismo.interfaces.repositories;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
   
    public class LineService : ILineService
    {
        private readonly IPreplotVersionRepository _preplotVersionRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IConfiguration _configuration;

        public LineService(
            IPreplotVersionRepository preplotVersionRepository,
            ILineRepository lineRepository,
            IConfiguration configuration)
        {
            _preplotVersionRepository = preplotVersionRepository;
            _lineRepository = lineRepository;
            _configuration = configuration;
        }

        public async Task<SurveyLinesModel> ListSurveyLines(int surveyId, int preplotVersionId, int linePointsType)
        {   
            try
            {
                var lines = await _lineRepository.ListLines(surveyId, preplotVersionId, linePointsType);
                var linesCount = lines.Count();
                return  new SurveyLinesModel
                {
                    Lines = linesCount > 0 ? lines.ToList(): (new List<LineModel>()),
                    LinesCount = linesCount,
                    LinesTotalPoints = lines.Sum(m => m.TotalPoints),
                    LinesTotalKm = lines.Sum(m => m.TotalKm)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<LineModel>> ListSummarizedLines(int surveyId, int operationalFrontId)
        {   
            try
            {
               return  await _lineRepository.ListSummarizedLines(surveyId, operationalFrontId);
            }
             catch (Exception ex) { throw ex; }
   
        }

        public async Task<String> ListSummarizedLinesCsv(int surveyId, int operationalFrontId)
        {
            StringBuilder sb = new StringBuilder();
            
            try
            {

                List<LineModel> lines =  await _lineRepository.ListSummarizedLines(surveyId, operationalFrontId);
                sb.AppendLine("Tipo;Linha;TotalKm;Total Estacas;Estaca Inicial;Estaca Final;Total Realizados PT;Total Realizados ER;Total Realizados;Total N Realizados;Estacas Restantes;Total Km Realizados;Km Restantes");
                foreach (var l in lines)
                {
                    sb.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
                                    l.LineTypeName,
                                    l.LineName,
                                    l.TotalKm.ToString().Replace(".", ","),
                                    l.TotalPoints,
                                    l.InitialStation,
                                    l.FinalStation,
                                    l.TotalRealizedPT,
                                    l.TotalRealizedER,
                                    l.TotalRealized,
                                    l.TotalNotRealized,
                                    l.RemainingPoints,
                                    l.TotalKmRealized.ToString().Replace(".", ","),
                                    l.RemainingKm.ToString().Replace(".", ",")
                                    ));
                }
            }
            catch (Exception ex) { throw ex; }
            return sb.ToString();
        }

        public async Task<SurveyLinesModel> ListSurveyLinesSummary(int surveyId)
        {
            
            try
            {
                var lines = await _lineRepository.ListLines(surveyId, PreplotPointType.All);
                return new SurveyLinesModel
                {
                    Lines = lines.ToList(),
                    LinesTotalPoints = lines.Sum(m => m.TotalPoints),
                    LinesTotalKm = lines.Sum(m => m.TotalKm)
                };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<LineModel>> ListLines(int surveyId, string versionDate)
        {
            var fomrattedDate = DateHelper.StringToDate(versionDate);
            var version = await  _preplotVersionRepository.GetPreplotVersion(surveyId, fomrattedDate);
            var lines =  await _lineRepository.ListLines(surveyId, version.PreplotVersionId);
            return lines.ToList();
        }
    }
}
