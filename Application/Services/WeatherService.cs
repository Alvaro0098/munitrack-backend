using Application.Interfaces;
using Domain.Dtos;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WeatherService : IWeatherApiService
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public async Task<WeatherResultDto> GetRosarioWeatherAsync()
        {
            var rawData = await _weatherRepository.GetWeatherAsync();
                                                                            // lo que hago en estas 2 lineas es:
            string nowString = DateTime.Now.ToString("yyyy-MM-dd HH:00");  // -> obtengo la fecha actual y la guardo en una variable en el formato que necesito
            int index = rawData.Data_1h.Time.IndexOf(nowString);          // comparo y busco el indice de la hora que obtuve en la linea anterior

            if (index == -1) index = 0; // si por algun momento falla la busqueda del indice, usamos el indice 0 es decir el primer dato del clima

            var result = new WeatherResultDto
            {
                City = "Rosario",
                Current = new WeatherDataPoint
                {
                    DateTime = rawData.Data_1h.Time[index],
                    Temp = rawData.Data_1h.Temperature[index],
                    Humidity = rawData.Data_1h.Relativehumidity[index]
                },
            };

            return result;
        }
    }
}