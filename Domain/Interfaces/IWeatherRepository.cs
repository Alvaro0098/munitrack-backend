using Domain.Dtos;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWeatherRepository
    {
        Task<WeatherResponseDto> GetWeatherAsync();
    }
}