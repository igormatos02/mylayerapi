using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IPointOfInterestRepository
    {
        Task<PointOfInterestModel> SavePointOfInterest(PointOfInterestModel p);
        Task<PointOfInterestModel> GetPointOfInterest(int? surveyId, int pointOfInterestId);
        Task<List<PointOfInterestModel>> ListPointsOfInterest(int surveyId);
        Task<List<PointOfInterestModel>> ListAllPointsOfInterest();
        Task<bool> DeletePointOfInterest(Int32 pointOfInterestId);
    }
}
