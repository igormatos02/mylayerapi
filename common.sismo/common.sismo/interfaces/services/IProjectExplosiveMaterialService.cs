using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface  IProjectExplosiveMaterialService 
    {
       Task<ProjectExplosiveMaterialDataModel> ListProjectExplosiveMaterials(int projectId, Int32 entryType);
       Task<ProjectExplosiveMaterialDataModel> ListSurveyExplosiveMaterials(int projectId, int surveyId, Int32 entryType);
       Task<ProjectExplosiveMaterialTypeSummaryDataModel> GetDashboardData(Int32 projectId, string date);
       Task<ProjectExplosiveMaterialTypeSummaryModel> GetProjectExplosiveMaterialTypeSummary( int projectId, int surveyId, int projectExplosiveMaterialTypeId, DateTime dateLimit);
       Task<decimal> GetTotalVolumeAvailable(int projectId, int surveyId, String Unity);
       Task<decimal> GetTotalUnityAvailable(int projectId, int surveyId, String Unity);
       Task<bool> SaveProjectExplosiveMaterials(ProjectExplosiveMaterialModel dto);
       Task<bool> DeleteProjectExplosiveMaterial(ProjectExplosiveMaterialModel dto);
       Task<ProjectExplosiveMaterialModel> GetProjectExplosiveMaterial(int entryId);
    }
}
