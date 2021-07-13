using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace VulnerableApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            _logger.LogInformation("");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult GetFile(string input)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine("~/ClientDocument/") + input);

            var cmdText = "SELECT * FROM Users WHERE username = '" + input + "' and role='user'";
            using (var cn = new SqlConnection("connection string"))
            {
                var cmd = new SqlCommand(cmdText);
                cmd.ExecuteNonQuery();
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, input);
        }
    }
}
