using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class LineMapping
    {
        public static LineModel ToModel(this Line entity)
        {
            if (entity == null) return null;

            var model = new LineModel();


            return model;

        }

        public static void Copy(this LineModel model, Line entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static Line ToEntity(this LineModel model)
        {
            Line entity = new Line();
            model.Copy(entity);
            return entity;
        }

    }
}
