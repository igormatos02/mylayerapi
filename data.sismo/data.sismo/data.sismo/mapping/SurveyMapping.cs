using common.sismo.helpers;
using common.sismo.models;
using data.sismo.models;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Collections.Generic;
using System.Linq;

namespace data.sismo.mapping
{
    public static class SurveyMapping
    {
        public static SurveyModel ToFullModel(this Survey entity)
        {
            if (entity == null) return null;

            var model = ToSimplifiedModel(entity);

            //model.Project = entity.Project.ToModel();
            //OperationalFronts = entity.SurveyOperationalFronts.Select(x => x.ToModel()),
            //ProjectBases = entity.SurveyProjectBases.Select(x => x.ToModel()),
            model.PolygonColor = entity.PolygonColor;
            model.LastUpdate = entity.LastUpdate;
            // OperationalFrontsIds.AddRange(entity.SurveyOperationalFronts.Select(x => x.OperationalFrontId).ToList()),
            //ProjectBasesIds.AddRange(entity.SurveyProjectBases.Select(x => x.ProjectBaseId)),
            //CountryId = entity.Project.CountryId;
            //StateId = entity.Project.StateId;
            return model;
          
        }

        public static void Copy(this SurveyModel model, Survey entity)
        {
            
            
         
            
            entity.ProjectId = model.ProjectId;
            entity.SurveyId = model.SurveyId;
            //entity.Name = project.Name;
            if (!string.IsNullOrEmpty(model.PolygonWKT)) {
                WKTReader reader = new WKTReader(GeometryFactory.Default);
                var geom = reader.Read(model.PolygonWKT);
                entity.Polygon = geom;  //wKTReader.Read.FromText(vDto.PolygonWKT, 4326);
            }
        }
        public static Survey ToEntity(this SurveyModel model)
        {
            Survey entity = new Survey();
            model.Copy(entity);
            return entity;
        }
        public static SurveyModel ToSimplifiedModel(this Survey entity)
        {
            if (entity == null) return null;


            var model =  new SurveyModel
            {

                ProjectId = entity.ProjectId,
                SurveyId = entity.SurveyId,
                CoordinateSystem = entity.CoordinateSystem,
                DatumId = entity.DatumId,
                Dimension = entity.Dimension,
                IsActive = entity.IsActive,
                LastEditorUser = entity.LastEditorUserLogin,
                DateIni = entity.DateIni.HasValue ? DateHelper.DateToString(entity.DateIni.Value) : "",
                DateEnd = entity.DateEnd.HasValue ? DateHelper.DateToString(entity.DateEnd.Value) : "",
                Name = entity.Name,
                PolygonWKT = entity.Polygon != null ? entity.Polygon.ToString() : "Polígono não preenchido!",
                PolygonColor = entity.PolygonColor,

                HolesDepth = entity.HolesDepth,
                HolesPerShotPoint = entity.HolesPerShotPoint,
                HolesArrangementImagePath = entity.HolesArrangementImagePath,
                HolesArrangementDescription = entity.HolesArrangementDescription,
                //GPSDatabaseName = entity.GPSDatabaseName,
                LastUpdate = entity.LastUpdate,
                // CountryId = entity.Project.CountryId,
                //StateId = entity.Project.StateId
            };

            return model;

        }
    }
}

