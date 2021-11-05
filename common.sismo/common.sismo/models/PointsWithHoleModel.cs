using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class PointsWithHoleModel
    {
        public List<PointWithHolesCoordinatesModel> Points { get; set; }
        public int? HolesPerShotPoint { get; set; }
        public string Datum { get; set; }
        public int Count { get; set; }
    }
}
