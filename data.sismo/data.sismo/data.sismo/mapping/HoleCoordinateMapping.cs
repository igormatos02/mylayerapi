using common.sismo.models;
using data.sismo.models;


namespace data.sismo.mapping
{
    public static class HoleCoordinateMapping
    {
        public static HoleCoordinateModel ToModel(this HoleCoordinate entity,string toWkt, int toSrid)
        {
            if (entity == null) return null;

            var model = new HoleCoordinateModel();

            //if (entity == null) return null;

            //var geometry = entity.Coordinate;
            //if (!string.IsNullOrWhiteSpace(toWkt) && !toWkt.StartsWith("GEOGCS"))
            //    geometry = Functions.ConvertPointCoordinateFromWgs84(entity.Coordinate, toSrid, toWkt);

            //var _dto = CreateDto(entity) as HoleCoordinateDTO;
            //_dto.CoordinateX = geometry?.XCoordinate.ToString() ?? entity.Coordinate.XCoordinate.ToString();
            //_dto.CoordinateY = geometry?.YCoordinate.ToString() ?? entity.Coordinate.YCoordinate.ToString();
            //_dto.CoordinateZ = entity.Coordinate.Elevation.ToString();
            //_dto.Coordinate = geometry;

          

            return model;

        }

        public static void Copy(this HoleCoordinateModel model, HoleCoordinate entity)
        {




            //entity.ProjectId = model.ProjectId;
            //entity.SurveyId = model.SurveyId;

        }
        public static HoleCoordinate ToEntity(this HoleCoordinateModel model)
        {
            HoleCoordinate entity = new HoleCoordinate();
            model.Copy(entity);
            return entity;
        }

    }
}
