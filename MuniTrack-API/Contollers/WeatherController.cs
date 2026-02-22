using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Dtos; 
using System.Threading.Tasks;

namespace MuniTrack_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherApiService _weatherService;

        public WeatherController(IWeatherApiService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("rosario")]
        public async Task<ActionResult<WeatherResultDto>> GetWeather()
        {
            var result = await _weatherService.GetRosarioWeatherAsync();
            return Ok(result);
        }
    }
}