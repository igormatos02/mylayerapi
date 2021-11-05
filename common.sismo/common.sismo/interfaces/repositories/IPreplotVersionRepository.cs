using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPreplotVersionRepository
    {
        Task<PreplotVersionModel> GetPreplotVersion(int surveyId, int versionId);

        Task<PreplotVersionModel> GetPreplotVersion(int surveyId, DateTime versionDate);

        Task<List<PreplotVersionModel>> ListPreplotVersions(int surveyId);

        Task<int> InsertPreplotVersion(PreplotVersionModel preplotVersion);

        Task _Delete(PreplotVersionModel version);

        Task UpdatePreplotDate(Int32 surveyId, Int32 currentVersion);
    }
}
