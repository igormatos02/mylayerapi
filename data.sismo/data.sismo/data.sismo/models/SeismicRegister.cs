using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SeismicRegister
    {
        public int Ffid { get; set; }
        public int SurveyId { get; set; }
        public int? ShotNumber { get; set; }
        public string Swath { get; set; }
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
        public bool IsNoiseTest { get; set; }
        public int BlasterShotStatus { get; set; }
        public int? QualityControlStatus { get; set; }
        public int? SwathNumber { get; set; }

        public virtual PointProduction PointProduction { get; set; }
    }
}
