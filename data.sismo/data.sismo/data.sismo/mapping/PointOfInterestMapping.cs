using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PointOfInterestMapping
    {
        public static PointOfInterestModel ToModel(this SurveyPointsOfInterest entity)
        {
            if (entity == null) return null;

            var model = new PointOfInterestModel();


            return model;

        }

        public static void Copy(this PointOfInterestModel model, SurveyPointsOfInterest entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static SurveyPointsOfInterest ToEntity(this PointOfInterestModel model)
        {
            SurveyPointsOfInterest entity = new SurveyPointsOfInterest();
            model.Copy(entity);
            return entity;
        }

    }
}
