using System;

namespace common.sismo.models
{
    public class ProjectBaseModel
    {
        public Int32 BaseId { get; set; }
        public Int32 ProjectId { get; set; }
        public String BaseName { get; set; }
        public String Description { get; set; }
        public String CoordinateX { get; set; }
        public String CoordinateY { get; set; }
        public String ImageFolder { get; set; }
        public String Datum { get; set; }
        public bool IsActive { get; set; }
        public bool? IsChecked { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
