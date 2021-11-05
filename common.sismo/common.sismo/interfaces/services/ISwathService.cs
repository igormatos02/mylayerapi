using common.sismo.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface ISwathService
    {
        Task<List<SwathModel>> ListSwaths(int surveyId);

        Task<List<LineStretchModel>> ListShotStretchsFromSwath(int surveyId, int swathNumber);

        Task<bool> SaveSwath(Stream fileStream, Int32 surveyId, Int32 preplotVersionId);
    }
}
