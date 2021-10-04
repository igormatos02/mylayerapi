using common.sismo.models;
using data.sismo.models;

namespace data.sismo.mapping
{
    public static class PosplotCoordinateMapping
    {
        public static PosplotCoordinateModel ToModel(this PosplotCoordinate entity)
        {
            if (entity == null) return null;

            var model = new PosplotCoordinateModel();


            return model;

        }

        public static PosplotCoordinateModel ToModel(this PosplotCoordinate entity, string toWkt, int toSrid)
        {
            if (entity == null) return null;

            var model = new PosplotCoordinateModel();


            return model;

        }

        public static void Copy(this PosplotCoordinateModel model, PosplotCoordinate entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static PosplotCoordinate ToEntity(this PosplotCoordinateModel model)
        {
            PosplotCoordinate entity = new PosplotCoordinate();
            model.Copy(entity);
            return entity;
        }

    }
}
