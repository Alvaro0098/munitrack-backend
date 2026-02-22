using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Interfaces;

namespace Infrastructure.ExternalServices
{
    public class WeatherRepositoryService : IWeatherRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherRepositoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync()
        {
            var client = _httpClientFactory.CreateClient("weatherHttpClient"); //instancia de httpClient, busca la url base en el program
            string endpoint = "packages/basic-1h?lat=-32.9468&lon=-60.6393&format=json&apikey=K1EA4zPIoun8m6Uq";
            
            var response = await client.GetFromJsonAsync<WeatherResponseDto>(endpoint); //orden de partida
            
            return response ?? throw new Exception("Error al obtener datos de Meteoblue");
        }
    }
}