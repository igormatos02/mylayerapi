using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ParameterMapping
    {
        public static ParameterModel ToModel(this Parameter entity)
        {
            if (entity == null) return null;

            var model = new ParameterModel();

           
            return model;

        }

        public static void Copy(this ParameterModel model, Parameter entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;
          
        }
        public static Parameter ToEntity(this ParameterModel model)
        {
            Parameter entity = new Parameter();
            model.Copy(entity);
            return entity;
        }
      
    }
}
