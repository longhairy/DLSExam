using Microsoft.AspNetCore.Mvc;
using Monitoring;
using System.Security.AccessControl;

namespace ApiGatewayService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ApiGatewayController : ControllerBase
    {

        public ApiGatewayController()
        {

        }

        [HttpGet("/RouletteService/get/game_types")]
        public IActionResult GetGameTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/game_types";
                
                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetGameTypes in ApiGatewayController");
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }
        [HttpGet("/RouletteService/get/game_type/{id}")]
        public IActionResult GetRouletteGame(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetRouletteGame(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/game_type";
                var uri = new Uri($"{baseAddress}/{id}");

                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetRouletteGame(int) in ApiGatewayController uri called: " +uri.AbsoluteUri);
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }

        [HttpGet("/RouletteService/get/bet_types")]
        public IActionResult GetBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetBetTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/bet_types";

                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetBetTypes in ApiGatewayController");
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }


        [HttpGet("/RouletteService/get/bet_type/{id}")]
        public IActionResult GetBetType(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetBetType(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/bet_type";
                var uri = new Uri($"{baseAddress}/{id}");

                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetBetType(int) in ApiGatewayController uri called: " + uri.AbsoluteUri);
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }

        [HttpGet("/RouletteService/get/game_bet_types")]
        public IActionResult GetGameBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameBetTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/game_bet_types";

                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetGameBetTypes in ApiGatewayController");
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }

        }

        [HttpGet("/RouletteService/get/game_bet_types/{gameId}")]
        public IActionResult GetGameBetTypesByGameId(int gameId)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameBetTypesByGameId(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/game_bet_types";
                var uri = new Uri($"{baseAddress}/{gameId}");

                var response = client.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    MonitorService.Log.Debug($"Exiting GetGameBetTypesByGameId in ApiGatewayController uri called: " + uri.AbsoluteUri);
                    return new JsonResult(result);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }




        [HttpPost("/RouletteService/Post/bet")]
        public double PostBet([FromQuery] int uid, [FromQuery] int bet_type, [FromQuery] double bet_amount, [FromQuery] int bet_number)
        {

            return 0;

        }






    }

}
