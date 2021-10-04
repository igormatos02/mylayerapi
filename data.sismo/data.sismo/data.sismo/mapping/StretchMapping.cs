using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class StretchMapping
    {
        public static StretchModel ToModel(this Stretch entity)
        {
            if (entity == null) return null;

            var model = new StretchModel();


            return model;

        }

        public static void Copy(this StretchModel model, Stretch entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static Stretch ToEntity(this StretchModel model)
        {
            Stretch entity = new Stretch();
            model.Copy(entity);
            return entity;
        }

    }
}
