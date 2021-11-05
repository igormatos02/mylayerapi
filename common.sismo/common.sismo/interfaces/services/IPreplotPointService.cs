using common.sismo.enums;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IPreplotPointService
    {
        Task<string> SavePreplotGPSeismic(int surveyId, string userLogin, string comment, int inputType);


        Task<PreplotPointModel> GetPreplotPoint(int surveyId, PreplotPointType preplotPointType, int preplotPointVersionId,
            int preplotPointId);

        Task<List<PointDetailModel>> GetPointDetail(int surveyId, int preplotVersionId, int preplotPointId);

        Task<VersionAndTypesModel> ListVersionsTypesLines(int surveyId, int pointType);

        Task<List<PreplotVersionModel>> ListPreplotVersions(int surveyId);

        Task<List<string>> ListLines(Int32 surveyId, Int32 preplotPointType);

        Task<List<string>> ListLinesForOperationalFront(Int32 surveyId, Int32 operationalFrontId);

        Task<object> ListLinesForAutocomplete(Int32 surveyId, Int32 preplotPointType, string key);

        Task<List<string>> ListLinesByVersion(Int32 surveyId, Int32 preplotPointType, int preplotVersionId);

        Task<List<decimal>> ListPointsFromLine(Int32 surveyId, Int32 preplotPointType, String line);

        Task<List<decimal>> ListPointsFromLineForOperationalFront(Int32 surveyId, Int32 operationalFrontId, String line);

        Task<PreplotPointListWithSrs> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId,
            string line, PaginationModel pagination, bool exportCsv);

        Task<PreplotPointListWithSrs> ListPreplotPoints(int surveyId, PreplotPointType pointType, int preplotVersionId,
            int swathNumber, PaginationModel pagination, bool exportCsv);

        PreplotPointType GetPreplotPointTypeByOpFrontType(OperationalFrontType operationalFrontType);


    }
}
