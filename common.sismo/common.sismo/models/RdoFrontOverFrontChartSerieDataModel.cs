using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class RdoFrontOverFrontChartSerieDataModel
    {
        public RdoFrontOverFrontChartSerieDataModel()
        {
            AccumulatedProductions = new List<decimal>();
        }
        public string FrontName { get; set; }
        public List<decimal> AccumulatedProductions { get; set; }
    }
}
