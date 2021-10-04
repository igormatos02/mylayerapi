using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPlannedStretchRepository
    {
        Task<PlannedStretchModel> GetStretch(int surveyId, int operationalFrontId, string date, string line, decimal initialStation, decimal finalStation);
        Task<bool> DeletePlannedStretch(PlannedStretchModel stretch);
        Task<PlannedStretchModel> GetPreviousStationt(PlannedStretchModel model);
        Task<PlannedStretchModel> GetNextStation(PlannedStretchModel dto);
        Task<List<PlannedStretchModel>> ListPlannedStretches(int surveyId,
            [Optional] int operationalFrontId,
            [Optional] string date,
            [Optional] string line,
            [Optional] decimal initialStation,
            [Optional] decimal finalStation,
            [Optional] int frontGroupLeaderId,
            [Optional] int frontGroupId);
        Task<bool> HasIntersectionedStretches(int surveyId, int operationalFrontId, string line, decimal initialStation, decimal finalStation);
        Task AddStretch(PlannedStretchModel model);
    }
}
