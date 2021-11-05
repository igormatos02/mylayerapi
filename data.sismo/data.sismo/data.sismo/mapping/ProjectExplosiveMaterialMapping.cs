using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ProjectExplosiveMaterialMapping
    {
        public static ProjectExplosiveMaterialModel ToModel(this ProjectExplosiveMaterial entity)
        {
            if (entity == null) return null;

            var model = new ProjectExplosiveMaterialModel();


            return model;

        }

        public static void Copy(this ProjectExplosiveMaterialModel model, ProjectExplosiveMaterial entity)
        {


            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static ProjectExplosiveMaterial ToEntity(this ProjectExplosiveMaterialModel model)
        {
            ProjectExplosiveMaterial entity = new ProjectExplosiveMaterial();
            model.Copy(entity);
            return entity;
        }

    }
}
