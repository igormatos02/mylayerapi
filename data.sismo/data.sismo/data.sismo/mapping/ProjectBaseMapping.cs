using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ProjectBaseMapping
    {

        public static ProjectBaseModel ToModel(this ProjectBase entity)
        {

            return new ProjectBaseModel()
            {
                BaseId = entity.BaseId,
                BaseName = entity.BaseName
            };

        }

        public static void Copy(this ProjectBaseModel model, ProjectBase entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static ProjectBase ToEntity(this ProjectBaseModel model)
        {
            ProjectBase entity = new ProjectBase();
            model.Copy(entity);
            return entity;
        }
    }
}
