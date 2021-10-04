using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PointProductionMapping
    {
        public static PointProductionModel ToModel(this PointProduction entity)
        {
            if (entity == null) return null;

            var model = new PointProductionModel();


            return model;

        }

        public static void Copy(this PointProductionModel model, PointProduction entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static PointProduction ToEntity(this PointProductionModel model)
        {
            PointProduction entity = new PointProduction();
            model.Copy(entity);
            return entity;
        }

    }
}
