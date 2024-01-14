using Microsoft.AspNetCore.Mvc;
using Monitoring;
using System.Security.AccessControl;
using SharedModels;
using System.Text.Json;


namespace ApiGatewayService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ApiGatewayController : ControllerBase
    {

        public ApiGatewayController()
        {

        }

        [HttpGet("/rouletteService/get/game_types")]
        public async Task<IActionResult> GetGameTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/game_types";
                
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<GameType>? gameType = JsonSerializer.Deserialize<List<GameType>>(result);

                    MonitorService.Log.Debug($"Exiting GetGameTypes in ApiGatewayController");
                    return new JsonResult(gameType);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }
        [HttpGet("/rouletteService/get/game_type/{id}")]
        public async Task<IActionResult> GetRouletteGame(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetRouletteGame(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/game_type";
                var uri = new Uri($"{baseAddress}/{id}");

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    GameType? gameType = JsonSerializer.Deserialize<GameType>(result);

                    MonitorService.Log.Debug($"Exiting GetRouletteGame(int) in ApiGatewayController uri called: " +uri.AbsoluteUri);
                    return new JsonResult(gameType);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }

        [HttpGet("/rouletteService/get/bet_types")]
        public async Task<IActionResult> GetBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetBetTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/bet_types";

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<BetType>? betType = JsonSerializer.Deserialize<List<BetType>>(result);

                    MonitorService.Log.Debug($"Exiting GetBetTypes in ApiGatewayController");
                    return new JsonResult(betType);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }


        [HttpGet("/rouletteService/get/bet_type/{id}")]
        public async Task<IActionResult> GetBetType(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetBetType(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/bet_type";
                var uri = new Uri($"{baseAddress}/{id}");

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    BetType? betType = JsonSerializer.Deserialize<BetType>(result);
                    MonitorService.Log.Debug($"Exiting GetBetType(int) in ApiGatewayController uri called: " + uri.AbsoluteUri);
                    return new JsonResult(betType);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }

        [HttpGet("/rouletteService/get/game_bet_types")]
        public async Task<IActionResult> GetGameBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameBetTypes, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://roulette-service/get/game_bet_types";

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<GameBetType>? game_bet_types = JsonSerializer.Deserialize<List<GameBetType>>(result);

                    MonitorService.Log.Debug($"Exiting GetGameBetTypes in ApiGatewayController");
                    return new JsonResult(game_bet_types);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }

        }

        [HttpGet("/rouletteService/get/game_bet_types/{gameId}")]
        public async Task<IActionResult> GetGameBetTypesByGameId(int gameId)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, GetGameBetTypesByGameId(int), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/get/game_bet_types";
                var uri = new Uri($"{baseAddress}/{gameId}");

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<GameBetType>? game_bet_types = JsonSerializer.Deserialize<List<GameBetType>>(result);

                    MonitorService.Log.Debug($"Exiting GetGameBetTypesByGameId in ApiGatewayController uri called: " + uri.AbsoluteUri);
                    return new JsonResult(game_bet_types);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }




        [HttpPost("/rouletteService/post/bet")]
        public async Task<double> PostBet([FromQuery] int bet_type, [FromQuery] double bet_amount, [FromQuery] int bet_number, [FromQuery] string email, [FromQuery] string password)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug($"Entered Post Bet in ApiGatewayController");

            //Kald history service post:
            using (var client = new HttpClient())
            {
                var baseAddress = "http://roulette-service/post/bet";
                var uri = new Uri($"{baseAddress}?bet_type={bet_type}&bet_amount={bet_amount}&bet_number={bet_number}&email={email}&password={password}");

                var response = await client.PostAsync(uri, null);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                    if (result != null)
                    {
                        return Double.Parse(result);
                    }
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                }
                MonitorService.Log.Debug($"Exiting Post Bet in ApiGatewayController");

                return 0;
            }

        }

        [HttpPost("/userService/post/user")]
        public async Task SaveUser([FromQuery] string email, [FromQuery] string password, [FromQuery] int cpr, [FromQuery] string name, [FromQuery] double balance)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug($"Entered Post User in ApiGatewayController");

            //Kald history service post:
            using (var client = new HttpClient())
            {
                var baseAddress = "http://user-service/post/user";
                var uri = new Uri($"{baseAddress}?email={email}&password={password}&cpr={cpr}&name={name}&balance={balance}");

                var response = await client.PostAsync(uri, null);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                    if (result != null)
                    {
                        Console.WriteLine("User created through api gateway");

                    }
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                }
                MonitorService.Log.Debug($"Exiting Post user in ApiGatewayController");

            }
        }

        [HttpPost("/userService/post/change-balance")]
        public async Task<double> changeBalance([FromQuery] string email, [FromQuery] double amount)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug($"Entered change User Balance in ApiGatewayController");

            //Kald history service post:
            using (var client = new HttpClient())
            {
                var baseAddress = "http://user-service/post/change-balance";
                var uri = new Uri($"{baseAddress}?email={email}&amount={amount}");

                var response = await client.PostAsync(uri, null);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);
                    if (result != null)
                    {
                        Console.WriteLine("User balance updated through api gateway");
                       
                        return Double.Parse(result);
                    }
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                }
                MonitorService.Log.Debug($"Exiting Post Bet in ApiGatewayController");

            }
            return 0.0;
        }



        [HttpGet("/userService/get/users")]
        public async Task<IActionResult> getUsers()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, getUsers, Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var uri = "http://user-service/get/users";

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<User>? users = JsonSerializer.Deserialize<List<User>>(result);

                    MonitorService.Log.Debug($"Exiting getUsers in ApiGatewayController");
                    return new JsonResult(users);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }

        }


        [HttpGet("/userService/get/user")]
        public async Task<IActionResult> getUserByEmail([FromQuery] string email, [FromQuery] string password)
        {

            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("ApiGatewayController, getUserByEmail(string), Start");
            // Retrieve the user's bet history from the database
            using (var client = new HttpClient())
            {
                var baseAddress = "http://user-service/get/user";
                var uri = new Uri($"{baseAddress}?email={email}&password={password}");

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    User? user = JsonSerializer.Deserialize<User>(result);
                    Console.WriteLine("Vi fanger denne user: ");
                    Console.WriteLine(user.ToString());
                    MonitorService.Log.Debug($"Exiting getUserByEmail in ApiGatewayController uri called: " + uri.AbsoluteUri);
                    return new JsonResult(user);
                }
                else
                {
                    MonitorService.Log.Error($"API call failed with status code: {response.StatusCode}");
                    return BadRequest();
                }
            }
        }
    }

}
