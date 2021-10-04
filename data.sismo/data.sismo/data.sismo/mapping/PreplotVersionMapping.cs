using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PreplotVersionMapping
    {
        public static PreplotVersionModel ToModel(this PreplotVersion entity)
        {
            if (entity == null) return null;

            var model = new PreplotVersionModel();


            return model;

        }

        public static void Copy(this PreplotVersionModel model, PreplotVersion entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static PreplotVersion ToEntity(this PreplotVersionModel model)
        {
            PreplotVersion entity = new PreplotVersion();
            model.Copy(entity);
            return entity;
        }

    }
}
