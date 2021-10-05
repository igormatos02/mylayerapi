using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISwathRepository
    {
        Task<SwathModel> GetSwath(int surveyId, int swathNumber);
        Task<SwathModel> GetSwath(int surveyId, int swathNumber, int preplotVersionId);
        Task<List<SwathModel>> ListSwaths(int surveyId);
        Task<List<int>> ListSwathsNumbers(int surveyId);
        Task<List<SwathModel>> ListSwaths(int surveyId, int preplotVersionId);
        Task SaveSwath(SwathModel model);
        Task SaveSwaths(IEnumerable<SwathModel> swaths);
        Task UpdateSwathPolygonSalva(int surveyId);
    }
}
