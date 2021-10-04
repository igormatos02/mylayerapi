using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PreplotPointMapping
    {
        public static PreplotPointModel ToModel(this PreplotPoint entity)
        {
            if (entity == null) return null;

            var model = new PreplotPointModel();


            return model;

        }

        public static PreplotPointModel ToModel(this PreplotPoint entity, string toWkt, int toSrid)
        {
            if (entity == null) return null;

            var model = new PreplotPointModel();


            return model;

        }

        public static void Copy(this PreplotPointModel model, PreplotPoint entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static PreplotPoint ToEntity(this PreplotPointModel model)
        {
            PreplotPoint entity = new PreplotPoint();
            model.Copy(entity);
            return entity;
        }

    }
}
