using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class SwathMapping
    {
        public static SwathModel ToModel(this Swath entity)
        {
            if (entity == null) return null;

            var model = new SwathModel();


            return model;

        }

        public static void Copy(this SwathModel model, Swath entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static Swath ToEntity(this SwathModel model)
        {
            Swath entity = new Swath();
            model.Copy(entity);
            return entity;
        }

    }
}
