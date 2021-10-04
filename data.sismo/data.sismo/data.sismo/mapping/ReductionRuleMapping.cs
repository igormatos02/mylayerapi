using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ReductionRuleMapping
    {
        public static ReductionRuleModel ToModel(this ReductionRule entity)
        {
            if (entity == null) return null;

            var model = new ReductionRuleModel();


            return model;

        }

        public static void Copy(this ReductionRule model, ReductionRule entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static ReductionRule ToEntity(this ReductionRule model)
        {
            ReductionRule entity = new ReductionRule();
            model.Copy(entity);
            return entity;
        }

    }
}
