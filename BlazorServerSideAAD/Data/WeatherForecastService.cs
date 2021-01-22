using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;


namespace BlazorServerSideAAD.Data
{
    public class WeatherForecastService
    {
        private ITokenAcquisition _tokenService;
        private Microsoft.Identity.Web.MicrosoftIdentityConsentAndConditionalAccessHandler _handler;
        public WeatherForecastService(ITokenAcquisition token, MicrosoftIdentityConsentAndConditionalAccessHandler handler)
        {
            _tokenService = token;
            _handler = handler;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {

            var rng = new Random();
            try
            {
                var a = await _tokenService.GetAccessTokenForUserAsync(new[] { "https://jpdab2c.onmicrosoft.com/blazor-backend/Stuff.Read" });
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = startDate.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)],
                    AccessToken = a
                }).ToArray();
            }
            catch (Exception ex)
            {
                _handler.HandleException(ex);
            }


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}