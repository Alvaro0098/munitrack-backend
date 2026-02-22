using System.Collections.Generic;

namespace Domain.Dtos
{
    public class WeatherResultDto
    {
        public string City { get; set; }
        public WeatherDataPoint Current { get; set; }
        
    }

    public class WeatherDataPoint
    {
        public string DateTime { get; set; }
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }
}