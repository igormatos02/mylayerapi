using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ILineRepository
    {
        Task<LineModel> GetLine(int surveyId, string lineName);
        Task<LineModel> GetLine(int surveyId, string lineName, int preplotVersionId);
        Task<List<LineModel>> ListLines(int surveyId);
        Task<List<LineModel>> ListLines(int surveyId, PreplotPointType linesPointType);
        Task<List<LineModel>> ListLines(int surveyId, int preplotVersionId);
        Task<List<LineModel>> ListLines(int surveyId, int preplotVersionId, int linesPointType);
        Task<List<LineModel>> ListSummarizedLines(int surveyId, int operationalfrontId);
        Task<List<string>> ListLinesNames(int surveyId);
        Task<List<string>> ListLinesNames(int surveyId, PreplotPointType linePointsType);
        Task<List<string>> ListLinesNames(int surveyId, int preplotVersionId);
        Task<List<string>> ListLinesNames(int surveyId, int preplotVersionId, PreplotPointType linePointsType);
        Task<LineModel> SaveLine(LineModel line);
        Task SaveLines(IEnumerable<LineModel> lines);
    }
}
