using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class SeismicRegisterMapping
    {
        public static SeismicRegisterModel ToModel(this SeismicRegister entity)
        {
            if (entity == null) return null;

            var model = new SeismicRegisterModel();


            return model;

        }

        public static void Copy(this SeismicRegisterModel model, SeismicRegister entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static SeismicRegister ToEntity(this SeismicRegisterModel model)
        {
            SeismicRegister entity = new SeismicRegister();
            model.Copy(entity);
            return entity;
        }

    }
}
