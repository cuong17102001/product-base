using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test-log")]
        public IActionResult LogTest()
        {
            _logger.LogInformation("✅ Login form submitted successfully");
            _logger.LogError("❌ An error occurred while logging in");
            return Ok("Logged to Elasticsearch!");
        }
        
        [HttpGet("weatherforecast" ,Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Get WeatherForecast testttttttt");
            try
            {
                Test();
            }
            catch (Exception e)
            {
                _logger.LogError("Get WeatherForecast test exception {@e}", e);
            }
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private void Test()
        {
            throw new NotImplementedException();
        }
    }
}
