
using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class FrontGroupLeaderMapping
    {
        public static FrontGroupLeaderModel ToModel(this FrontGroupLeader entity)
        {
            if (entity == null) return null;

            var model = new FrontGroupLeaderModel();


            return model;

        }

        public static void Copy(this FrontGroupLeaderModel model, FrontGroupLeader entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static FrontGroupLeader ToEntity(this FrontGroupLeaderModel model)
        {
            FrontGroupLeader entity = new FrontGroupLeader();
            model.Copy(entity);
            return entity;
        }

    }
}
