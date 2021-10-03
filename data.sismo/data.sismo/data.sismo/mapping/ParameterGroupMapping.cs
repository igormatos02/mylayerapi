using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ParameterGroupMapping
    {
        public static ParameterGroupModel ToModel(this ParameterGroup entity)
        {
            if (entity == null) return null;

            var model = new ParameterGroupModel();


            return model;

        }

        public static void Copy(this ParameterGroupModel model, ParameterGroup entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static ParameterGroup ToEntity(this ParameterGroupModel model)
        {
            ParameterGroup entity = new ParameterGroup();
            model.Copy(entity);
            return entity;
        }

    }
}
