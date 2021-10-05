using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class ProductionPerLineChartSerieModel
    {
        public ProductionPerLineChartSerieModel()
        {
            data = new List<int>();
        }
        public string name { get; set; }
        public List<int> data { get; set; }
        public string color { get; set; }

    }
}
