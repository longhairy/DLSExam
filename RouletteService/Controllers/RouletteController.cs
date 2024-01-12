using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using MySqlConnector;
using SharedModels;
using System.Data;
using System.Text.Json;


namespace RouletteService.Controllers
{
    public class RouletteController : Controller
    {
        private IDbConnection GameDBConnection = new MySqlConnection("Server=roulette-db;Database=roulette-database;Uid=roulettedb;Pwd=C@ch3d1v;");

        public RouletteController()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController Constructor Start");

            GameDBConnection.Open();

            var game_type_tables = GameDBConnection.Query<string>("SHOW TABLES LIKE 'game_type'");
            if (!game_type_tables.Any())
            {
                GameDBConnection.Execute("CREATE TABLE game_type (" +
                    "game_type_id INT NOT NULL AUTO_INCREMENT," +
                    "name VARCHAR(50) NOT NULL, " +
                    "description VARCHAR(100) NOT NULL, " +
                    "url VARCHAR(100) NOT NULL, " +
                    "PRIMARY KEY (game_type_id))");
            }
            if (game_type_tables.Count() == 0)
            {
                GameDBConnection.Execute("REPLACE INTO " +
                  "game_type (name,description,url)" +
                  "values ('Roulette','A game where a ball is spinned around a disc, and lands on a random number between 1 and 36','/roulette')");
                GameDBConnection.Execute("REPLACE INTO " +
                 "game_type (name,description,url)" +
                 "values ('BlackJack','A game of cards where the goal is to get the highest value not passing 21','/blackjack')");
            }



            var bet_type_tables = GameDBConnection.Query<string>("SHOW TABLES LIKE 'bet_type'");
            if (!bet_type_tables.Any())
            {
                GameDBConnection.Execute("CREATE TABLE bet_type (" +
                    "bet_type_id INT NOT NULL AUTO_INCREMENT," +
                    "name VARCHAR(50) NOT NULL, " +
                    "multiplier Decimal(10,2) NOT NULL, " +
                    "max_bet Decimal(10,2) NOT NULL, " +
                    "min_bet Decimal(10,2) NOT NULL, " +
                    "PRIMARY KEY (bet_type_id))");

                GameDBConnection.Execute("INSERT INTO " +
                    "bet_type (name,multiplier,max_bet,min_bet)" +
                    "values ('Even',2,100,1)");
                GameDBConnection.Execute("INSERT INTO " +
                    "bet_type (name,multiplier,max_bet,min_bet)" +
                    "values ('Odd',2,100,1)");
                GameDBConnection.Execute("INSERT INTO " +
                    "bet_type (name,multiplier,max_bet,min_bet)" +
                    "values ('High',2,100,1)");
                GameDBConnection.Execute("INSERT INTO " +
                    "bet_type (name,multiplier,max_bet,min_bet)" +
                    "values ('Low',2,100,1)");
                GameDBConnection.Execute("INSERT INTO " +
                    "bet_type (name,multiplier,max_bet,min_bet)" +
                    "values ('Exact Number',36,10,1)");

            }


            var game_bet_type_tables = GameDBConnection.Query<string>("SHOW TABLES LIKE 'game_bet_type'");
            if (!bet_type_tables.Any())
            {
                GameDBConnection.Execute("CREATE TABLE game_bet_type (" +
                    "game_bet_type_id INT NOT NULL AUTO_INCREMENT," +
                    "game_id INT NOT NULL, " +
                    "bet_type_id INT NOT NULL, " +
                    "PRIMARY KEY (game_bet_type_id)," +
                    "FOREIGN KEY (game_id) REFERENCES game_type(game_type_id)," +
                    "FOREIGN KEY (bet_type_id) REFERENCES bet_type(bet_type_id))");

                GameDBConnection.Execute("INSERT INTO " +
                    "game_bet_type (game_id, bet_type_id)" +
                    "values (1, 1)");
                GameDBConnection.Execute("INSERT INTO " +
                    "game_bet_type (game_id, bet_type_id)" +
                    "values (1, 2)");
                GameDBConnection.Execute("INSERT INTO " +
                    "game_bet_type (game_id, bet_type_id)" +
                    "values (1, 3)");
                GameDBConnection.Execute("INSERT INTO " +
                    "game_bet_type (game_id, bet_type_id)" +
                    "values (1, 4)");
                GameDBConnection.Execute("INSERT INTO " +
                    "game_bet_type (game_id, bet_type_id)" +
                    "values (1, 5)");

            }


            var roulette_tables = GameDBConnection.Query<string>("SHOW TABLES LIKE 'roulette_game'");
            if (!roulette_tables.Any())
            {
                GameDBConnection.Execute("CREATE TABLE roulette_game (" +
                    "gid INT NOT NULL AUTO_INCREMENT," +
                    "uid INT NOT NULL, " +
                    "date DATETIME NOT NULL, " +
                    "bet_type_id INT NOT NULL, " +
                    "Bet_amount Decimal(10,2) NOT NULL, " +
                    "result Decimal(10,2) NOT NULL, " +
                    "PRIMARY KEY (gid)," +
                    "FOREIGN KEY (bet_type_id) REFERENCES bet_type(bet_type_id))");
            }

        }
        [HttpGet("/get/game_types")]
        public IActionResult GetGameTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameTypes, Start");
            // Retrieve the user's bet history from the database
            var gameTypes = GameDBConnection.Query<GameType>("SELECT Name, Description, url FROM game_type");

            MonitorService.Log.Debug("RouletteController, GetGameTypes, no of GameTypes: "+gameTypes.Count()+", at Return");

            return new JsonResult(gameTypes);
        }

        [HttpGet("/get/game_type/{id}")]
        public IActionResult GetRouletteGame(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetRouletteGame, id: "+id+", Start");

            // Retrieve the roulette game based on the provided ID
            var rouletteGame = GameDBConnection.QueryFirstOrDefault<GameType>("SELECT game_type_id as GameTypeId, Name, Description, url FROM game_type WHERE game_type_id = @Id", new { Id = id });

            if (rouletteGame == null)
            {
                MonitorService.Log.Debug("RouletteController, GetRouletteGame, rouletteGame: " + rouletteGame + ", at Return");

                return NotFound(); // Return 404 Not Found if the roulette game with the given ID is not found
            }
            MonitorService.Log.Debug("RouletteController, GetRouletteGame, rouletteGame: " + rouletteGame + ", at Return");

            return new JsonResult(rouletteGame);
        }

        [HttpGet("/get/bet_types")]
        public IActionResult GetBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetBetTypes, Start");

            // Retrieve the user's bet history from the database
            var betTypes = GameDBConnection.Query<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet FROM bet_type");

            MonitorService.Log.Debug("RouletteController, GetBetTypes, BetType: ["+betTypes.ToString()+"], Start");

            return new JsonResult(betTypes);
        }


        [HttpGet("/get/bet_type/{id}")]
        public IActionResult GetBetType(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetBetType(int id), id: "+id+", Start");

            // Retrieve the bet type based on the provided ID
            var betType = GameDBConnection.QueryFirstOrDefault<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet  FROM bet_type WHERE bet_type_id = @Id", new { Id = id });

            if (betType == null)
            {
                MonitorService.Log.Debug("RouletteController, GetBetType, id: " + id + ", at Return");

                return NotFound(); // Return 404 Not Found if the bet type with the given ID is not found
            }
            MonitorService.Log.Debug("RouletteController, GetBetType, id: " + id + ", Bettype: " + betType.ToString() + ", at Return");

            return new JsonResult(betType);
        }

        [HttpGet("/get/game_bet_types")]
        public IActionResult GetGameBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameBetTypes, Start");
            // Retrieve the user's bet history from the database
            var gameBetTypes = GameDBConnection.Query<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type");

            MonitorService.Log.Debug("RouletteController, GetGameBetTypes, GameBetType: ["+gameBetTypes.ToString()+"], at Return");

            return new JsonResult(gameBetTypes);
        }

        [HttpGet("/get/game_bet_types/{gameId}")]
        public IActionResult GetGameBetTypesByGameId(int gameId)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameBetTypesByGameId, id: "+gameId+", Start");

            // Retrieve game_bet_types based on the provided game_id
            var gameBetTypes = GameDBConnection.Query<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type WHERE game_id = @GameId", new { GameId = gameId });
            
            MonitorService.Log.Debug("RouletteController, GetGameBetTypesByGameId, id: " + gameId + ", GameBetType: ["+gameBetTypes.ToString()+"], at Return");

            return new JsonResult(gameBetTypes);
        }




        [HttpPost("/Post/bet")]
        public async Task<double> PostBet([FromQuery] int uid, [FromQuery] int bet_type, [FromQuery] double bet_amount, [FromQuery] int bet_number, [FromQuery] string email, [FromQuery] string password)
        {
            //TODO finish method.
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, PostBet, uid: " + uid + ", bet_type: " + bet_type + ", bet_amount: " + bet_amount + ", bet_number: " + bet_number + ", Start");
            
            double winnings = 0;
         
            
            var betType = GameDBConnection.QueryFirstOrDefault<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet  FROM bet_type WHERE bet_type_id = @Id", new { Id = bet_type });

            User user = await getUser(email, password);
            Console.WriteLine("Bet type name: " + betType.Name);

            Console.WriteLine(user.Balance);

            if (bet_amount<=(double)betType.MaxBet && bet_amount >= (double)betType.MinBet && user.Balance >= bet_amount)
            {
                Console.WriteLine("Bet values confirmed.");
                winnings = getSpinResults(betType.Name, bet_amount, (double)betType.Multiplier, bet_number);
                Console.WriteLine("Winnings: " + winnings);
               
            }
            else
            {
                MonitorService.Log.Warning("Bet outside of Max or Min limits");
            }

            //MonitorService.Log.Debug("RouletteController, PostBet, actualSpin: "+actualSpinResult+", uid: " + uid + ", bet_type: " + bet_type + ", bet_amount: " + bet_amount + ", bet_number: " + bet_number + ",Actual spin: "+actualSpinResult+" at Return");

            return winnings;



        }

        private double getSpinResults(string bet_name, double bet_amount, double multiplier, int bet_number)
        {
            Random random = new Random();
            var actualSpinResult = random.Next(1, 37);
            Console.WriteLine("Spin result:" + actualSpinResult);
            switch (bet_name)
            {
                case "High":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at High, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult > 18)
                        return bet_amount * multiplier;
                    break;
                case "Low":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Low, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult <= 18)
                        return bet_amount * multiplier;
                    break;
                case "Even":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Even, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult % 2 == 0)
                        return bet_amount * multiplier;
                    break;
                case "Odd":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Odd, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult % 2 == 1)
                        return bet_amount * multiplier;
                    break;
                case "Exact Number":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Exact Number, actualSpinResult: " + actualSpinResult + ", bet_number: " + bet_number);
                    if (actualSpinResult == bet_number)
                        return bet_amount * multiplier;
                    break;
            }
            return 0;
        }

        public async Task<User> getUser(string userEmail, string userPassword)
        {
            // Set the base URL of the user service
            string userServiceBaseUrl = "http://user-service";

            // Construct the URL for the GetUserByEmail API
            string getUserUrl = $"{userServiceBaseUrl}/get/user?email={userEmail}&password={userPassword}";
            // Create an instance of HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Make the GET request to the GetUserByEmail API
                    HttpResponseMessage response = await httpClient.GetAsync(getUserUrl);


                    // Check if the request was successful (status code 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();

                        // Deserialize the string content into a User object using JSON deserialization
                        User? user = JsonSerializer.Deserialize<User>(content);
                  
                        Console.WriteLine($"User retrieved: {user}");
                        return user;
                    }
                    else
                    {
                        // Print an error message if the request was not successful
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., network issues
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return null;
        }
    }
}
