using common.sismo.helpers;
using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ProjectMapping
    {
        public static SeismicProjectModel ToModel(this Project entity)
        {
            string typeComplete = "";

            switch (entity.ProjectType)
            {
                case "F":
                    typeComplete = "Fomento";
                    break;
                case "E":
                    typeComplete = "Exclusivo";
                    break;
                case "N":
                    typeComplete = "Não Exclusivo";
                    break;
            }

            return new SeismicProjectModel()
            {
                OwnerId = entity.OwnerId,
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                ClientName = entity.ClientName,
                ProjectType = typeComplete,
                IsActive = entity.IsActive,
                DateIni = entity.DateIni != null ? DateHelper.DateToString((DateTime)entity.DateIni) : "",
                DateEnd = entity.DateEnd != null ? DateHelper.DateToString((DateTime)entity.DateEnd) : "",
                LastUpdate = entity.LastUpdate,
                //CR = entity.CR,
                //CRObservation = entity.CRObservation,
                ExplosiveMax = entity.ExplosiveMax,
                FuseMax = entity.FuseMax,
                OnlineProjectId = entity.OnlineProjectId

            };
        }
        public static void Copy(this SeismicProjectModel project, Project entity)
        {

            entity.ProjectId = project.ProjectId;
            entity.OwnerId = project.OwnerId;
            entity.Name = project.Name;
            entity.ClientName = project.ClientName;
            entity.ProjectType = project.ProjectType.Substring(0, 1);
            entity.IsActive = project.IsActive;
            entity.LastUpdate = project.LastUpdate;
           // entity.CR = project.CR;
            //entity.CRObservation = project.CRObservation;
            entity.ExplosiveMax = project.ExplosiveMax;
            entity.FuseMax = project.FuseMax;
            entity.DateIni = string.IsNullOrEmpty(project.DateIni) ? new DateTime?() : DateHelper.StringToDate(project.DateIni);
            entity.DateEnd = string.IsNullOrEmpty(project.DateEnd) ? new DateTime?() : DateHelper.StringToDate(project.DateEnd);
            entity.OnlineProjectId = project.OnlineProjectId;

        }
        public static Project ToEntity(this SeismicProjectModel project)
        {
            Project entity = new Project();
            project.Copy(entity);
            return entity;
        }
    }
}
