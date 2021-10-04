using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class TerrainChargeControlMapping
    {
        public static TerrainChargeControlModel ToModel(this TerrainChargeControl entity)
        {
            if (entity == null) return null;

            var model = new TerrainChargeControlModel();


            return model;

        }

        public static void Copy(this TerrainChargeControlModel model, TerrainChargeControl entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static TerrainChargeControl ToEntity(this TerrainChargeControlModel model)
        {
            TerrainChargeControl entity = new TerrainChargeControl();
            model.Copy(entity);
            return entity;
        }

    }
}
