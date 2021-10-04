using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class SurveyParameterMapping
    {
        public static SurveyParameterModel ToModel(this SurveyParameter entity)
        {
            if (entity == null) return null;

            var model = new SurveyParameterModel();


            return model;

        }

        public static void Copy(this SurveyParameterModel model, SurveyParameter entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static SurveyParameter ToEntity(this SurveyParameterModel model)
        {
            SurveyParameter entity = new SurveyParameter();
            model.Copy(entity);
            return entity;
        }

    }
}
