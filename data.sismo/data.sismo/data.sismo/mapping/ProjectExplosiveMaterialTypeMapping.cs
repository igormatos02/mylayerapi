using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ProjectExplosiveMaterialTypeMapping
    {
        public static ProjectExplosiveMaterialTypeModel ToModel(this ProjectExplosiveMaterialType entity)
        {
            if (entity == null) return null;

            var model = new ProjectExplosiveMaterialTypeModel();


            return model;

        }

        public static void Copy(this ProjectExplosiveMaterialTypeModel model, ProjectExplosiveMaterialType entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static ProjectExplosiveMaterialType ToEntity(this ProjectExplosiveMaterialTypeModel model)
        {
            ProjectExplosiveMaterialType entity = new ProjectExplosiveMaterialType();
            model.Copy(entity);
            return entity;
        }

    }
}
