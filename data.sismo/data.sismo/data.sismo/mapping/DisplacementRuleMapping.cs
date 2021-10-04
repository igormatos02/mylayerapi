using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class DisplacementRuleMapping
    {
        public static DisplacementRuleModel ToModel(this DisplacementRule entity)
        {
            if (entity == null) return null;

            var model = new DisplacementRuleModel();


            return model;

        }

        public static void Copy(this DisplacementRuleModel model, DisplacementRule entity)
        {

            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static DisplacementRule ToEntity(this DisplacementRuleModel model)
        {
            DisplacementRule entity = new DisplacementRule();
            model.Copy(entity);
            return entity;
        }

    }
}
