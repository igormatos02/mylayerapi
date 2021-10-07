using common.sismo.models;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface ISurveyRepository
    {
        Task<SurveyModel> GetSurvey(Int32 surveyId);
        Task<Geometry> GetSurveyPolygonGeometry(int surveyId);
        Task<int> GetSurveyDatumId(int surveyId);
        Task<decimal?> GetSurveyDefaultHolesDepth(int surveyId);
        Task<DateTime?> GetSurveyEndDate(int surveyId);
        Task<int?> GetSurveyDefaultHolesQuantityPerShotPoint(int surveyId);
        Task<decimal> GetSurveyDefaultDistanceBetweenHoles(int surveyId);
        Task<decimal> GetSurveyHoleBufferSizeFactor(int surveyId);
        Task<List<SurveyModel>> ListSurveys(bool? isActive = true);
        Task<List<ProjectBaseModel>> ListSurveyProjectBases(int surveyId);
        Task<List<SurveyParameterModel>> ListSurveyParameters(int surveyId);
        Task<List<SurveyModel>> ListProjectSurveys(Int32 projectId);
        Task<int> AddSurvey(SurveyModel survey);
        Task UpdateSurvey(SurveyModel modifiedSurvey);
    }
}
