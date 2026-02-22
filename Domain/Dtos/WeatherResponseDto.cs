using System.Collections.Generic;

namespace Domain.Dtos
{
    public class WeatherResponseDto
    {
        public MetadataDto Metadata { get; set; }
        public Data1hDto Data_1h { get; set; }
    }

    public class MetadataDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Data1hDto
    {
        public List<double> Temperature { get; set; }
        public List<int> Relativehumidity { get; set; }
        public List<string> Time { get; set; }
    }
}