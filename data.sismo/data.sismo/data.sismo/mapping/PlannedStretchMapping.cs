using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PlannedStretchMapping
    {
        public static PlannedStretchModel ToModel(this PlannedStretch entity)
        {
            if (entity == null) return null;

            var model = new PlannedStretchModel();


            return model;

        }

        public static void Copy(this PlannedStretchModel model, PlannedStretch entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static PlannedStretch ToEntity(this PlannedStretchModel model)
        {
            PlannedStretch entity = new PlannedStretch();
            model.Copy(entity);
            return entity;
        }

    }
}
