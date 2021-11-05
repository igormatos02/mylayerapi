using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces
{
    public interface ILineService
    {
        Task<SurveyLinesModel> ListSurveyLines(int surveyId, int preplotVersionId, int linePointsType);

        Task<List<LineModel>> ListSummarizedLines(int surveyId, int operationalFrontId);

        Task<String> ListSummarizedLinesCsv(int surveyId, int operationalFrontId);

        Task<SurveyLinesModel> ListSurveyLinesSummary(int surveyId);
    }
}
