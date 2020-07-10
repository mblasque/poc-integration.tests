using Xunit;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using WebApplication1;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using IntegrationTestExample.IntegrationTests.Fixture;

namespace IntegrationTestExample.IntegrationTests.Tests
{
    public class WeatherForecastControllerTest : IClassFixture<TestFactory>
    {
        private readonly TestFactory _factory;
        private readonly HttpClient _client;

        public WeatherForecastControllerTest(TestFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Weather_Get()
        {
            var response = await _client.GetAsync("WeatherForecast");

            var str = await response.Content.ReadAsStringAsync();

            var weather = JsonConvert.DeserializeObject<List<WeatherForecast>>(str);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            weather.Should().HaveCount(5);
        }

        [Fact]
        public async Task Weather_Post()
        {
            var newWeather = new WeatherForecast { Date = new System.DateTime(), Summary = "", TemperatureC = 30 };

            var response = await _client.PostAsJsonAsync("WeatherForecast", newWeather);

            var str = await response.Content.ReadAsStringAsync();

            var weather = JsonConvert.DeserializeObject<WeatherForecast>(str);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            weather.Date.Should().Be(newWeather.Date);
            weather.Summary.Should().Be(newWeather.Summary);
            weather.TemperatureC.Should().Be(newWeather.TemperatureC);
            weather.TemperatureF.Should().Be(newWeather.TemperatureF);
        }
    }
}
