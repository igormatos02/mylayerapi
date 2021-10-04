using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class HolesCoordinatesFileMapping
    {
        public static HolesCoordinatesFileModel ToModel(this HolesCoordinatesFile entity)
        {
            if (entity == null) return null;

            var model = new HolesCoordinatesFileModel();


            return model;

        }

        public static void Copy(this HolesCoordinatesFileModel model, HolesCoordinatesFile entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static HolesCoordinatesFile ToEntity(this HolesCoordinatesFileModel model)
        {
            HolesCoordinatesFile entity = new HolesCoordinatesFile();
            model.Copy(entity);
            return entity;
        }

    }
}
