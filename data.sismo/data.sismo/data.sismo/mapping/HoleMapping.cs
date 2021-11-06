
using common.sismo.enums;
using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class HoleMapping
    {
        public static HoleModel ToModel(this Hole entity)
        {
            if (entity == null) return null;

            var model = new HoleModel();


            return model;

        }

    


        public static void Copy(this HoleModel model, Hole entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }

     

        public static Hole ToEntity(this HoleModel model)
        {
            Hole entity = new Hole();
            model.Copy(entity);
            return entity;
        }

    }
}
