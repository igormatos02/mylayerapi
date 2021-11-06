using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class FieldReportDataModel
    {
        public FieldReportDataModel()
        {
            PointProductions = new List<List<PointProductionModel>>();
        }
        public string Line { get; set; }
        public string Swath { get; set; }
        public string HolesArrangementDescription { get; set; }
        public string HolesArrangementImagePath { get; set; }
        public int HolesQuantity { get; set; }
        public List<List<PointProductionModel>> PointProductions { get; set; }
    }

}
