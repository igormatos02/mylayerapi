using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IPreplotVersionService
    {
        Task<List<PreplotVersionModel>> ListPreplotVersions(int surveyId);

        Task<int> GetLastPreplotVersion(int surveyId);

        Task<int> InsertPreplotVersion(PreplotVersionModel dto);
        Task DeleteVersion(int surveyId, int versionId);
    }
}
