using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class FrontGroupMapping
    {
        public static FrontGroupModel ToModel(this FrontGroup entity)
        {
            if (entity == null) return null;

            var model = new FrontGroupModel();


            return model;

        }

        public static void Copy(this FrontGroupModel model, FrontGroup entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static FrontGroup ToEntity(this FrontGroupModel model)
        {
            FrontGroup entity = new FrontGroup();
            model.Copy(entity);
            return entity;
        }

    }
}
