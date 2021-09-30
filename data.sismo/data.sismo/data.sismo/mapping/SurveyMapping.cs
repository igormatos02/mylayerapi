using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class SurveyMapping
    {
        public static SurveyModel ToModel(this Survey entity)
        {
           
            return new SurveyModel()
            {
                // OwnerId = entity.OwnerId,
                //ProjectId = entity.ProjectId,
                //Name = entity.Name,
                

            };
        }
        public static void Copy(this SurveyModel project, Survey entity)
        {

            //entity.ProjectId = project.ProjectId;
            //// entity.OwnerId = project.OwnerId;
            //entity.Name = project.Name;
           

        }
        public static Survey ToEntity(this SurveyModel model)
        {
            Survey entity = new Survey();
            model.Copy(entity);
            return entity;
        }
    }
}
