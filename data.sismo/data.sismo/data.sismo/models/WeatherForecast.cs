using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class WeatherForecast
    {
        public int IdWeatherForecast { get; set; }
        public string City { get; set; }
        public string Uf { get; set; }
        public DateTime? RegistrationTime { get; set; }
        public DateTime? ForecastDate { get; set; }
        public double? MaxTemperature { get; set; }
        public double? MinTemperature { get; set; }
        public double? UvIncidence { get; set; }
        public string Weather { get; set; }
        public int? Precipitation { get; set; }
        public double? Humidity { get; set; }
        public double? WindSpeed { get; set; }
        public double? Pressure { get; set; }
        public bool IsActive { get; set; }
    }
}
