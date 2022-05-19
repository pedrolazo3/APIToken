using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace APIToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly jwtAutenthicationManager jwtAutenthicationManager;
        public WeatherForecastController(jwtAutenthicationManager _jwtAutenthicationManager)
        {
            this.jwtAutenthicationManager = _jwtAutenthicationManager;
        }
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
 

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User usr)
        {
            var token= jwtAutenthicationManager.Authenticate(usr.username,usr.password);

            if(token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }

    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}