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


    public class SerieValue
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
        public decimal Value { get; set; }
    }
    public class Serie
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<SerieValue> Data { get; set; }
    }
    public class ProductionChartSerie
    {
        public List<string> OperationalFronts { get; set; }
        public List<int> TotalMissingSerie { get; set; }
        public List<int> TotalRealizedSerie { get; set; }
        public List<int> TotalNotRealizedSerie { get; set; }

    }
    public class ProductionPerLinePerOperationalFrontSerie
    {
        public List<string> Lines { get; set; }
        public List<ProductionPerLineChartSerieModel> Series { get; set; }

    }
    public class FrontOverFrontChartSerie
    {
        public List<Serie> Data { get; set; }
        public List<string> Dates { get; set; }


    }
    public class ProductionOfPeriodSerie
    {
        public List<string> Days { get; set; }
        public Serie Pontos { get; set; }
        public Serie Quilometragem { get; set; }
        public Serie MediaPt { get; set; }
        public Serie MediaKm { get; set; }
        public int LastAvgX { get; set; }
    }
}
