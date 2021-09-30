using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class OperationalFrontMapping
    {
        public static OperationalFrontModel ToModel(this OperationalFront entity)
        {
           
            return new OperationalFrontModel()
            {
                // OwnerId = entity.OwnerId,
                //ProjectId = entity.ProjectId,
                //Name = entity.Name,
                

            };
        }
        public static void Copy(this OperationalFrontModel project, OperationalFront entity)
        {

            //entity.ProjectId = project.ProjectId;
            //// entity.OwnerId = project.OwnerId;
            //entity.Name = project.Name;
           

        }
        public static OperationalFront ToEntity(this OperationalFrontModel model)
        {
            OperationalFront entity = new OperationalFront();
            model.Copy(entity);
            return entity;
        }
    }
}
