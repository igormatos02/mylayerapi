using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class SeismicRegisterModel
    {
        public int Ffid { get; set; }
        public int SurveyId { get; set; }
        public int? ShotNumber { get; set; }
        public string Swath { get; set; }
        public Int32? SwathNumber { get; set; }
        public bool? Itb { get; set; }
        public decimal PointNumber { get; set; }
        public int? NumberOfLiveSeis { get; set; }
        public int? NumberOfDeadSeis { get; set; }
        public string LineName { get; set; }
        public int BlasterId { get; set; }
        public decimal? UpholeTime { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int? WorkNumber { get; set; }
        public int OperationalFrontId { get; set; }
        public int PreplotPointType { get; set; }
        public int BlasterShotStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsNoiseTest { get; set; }
        public QualityControlStatus? QualityControlStatus { get; set; }
        //---POINT PRODUCTION--------------
        public PointProductionModel PointProduction { get; set; }
    }
}
