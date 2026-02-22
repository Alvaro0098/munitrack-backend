using Domain.Dtos;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWeatherApiService
    {
        Task<WeatherResultDto> GetRosarioWeatherAsync();
    }
}