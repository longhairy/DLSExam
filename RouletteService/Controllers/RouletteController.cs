﻿using Dapper;
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
        //private IDbConnection GameDBConnection = new MySqlConnection("Server=roulette-db;Database=roulette-database;Uid=roulettedb;Pwd=C@ch3d1v;");
        private readonly IDbConnection GameDBConnection;

        public RouletteController(IDbConnection gameDBConnection)
        {
            GameDBConnection = gameDBConnection;
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController Constructor Start");

            //GameDBConnection.Open();

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
        public async Task<IActionResult> GetGameTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameTypes, Start");
            // Retrieve the user's bet history from the database
            var gameTypes = await GameDBConnection.QueryAsync<GameType>("SELECT game_type_id as GameTypeId ,Name, Description, url FROM game_type");

            MonitorService.Log.Debug("RouletteController, GetGameTypes, no of GameTypes: " + gameTypes.Count() + ", at Return");

            return new JsonResult(gameTypes);
        }

        [HttpGet("/get/game_type/{id}")]
        public async Task<IActionResult> GetRouletteGame(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetRouletteGame, id: " + id + ", Start");

            // Retrieve the roulette game based on the provided ID
            var rouletteGame = await GameDBConnection.QueryFirstOrDefaultAsync<GameType>("SELECT game_type_id as GameTypeId, Name, Description, url FROM game_type WHERE game_type_id = @Id", new { Id = id });

            if (rouletteGame == null)
            {
                MonitorService.Log.Debug("RouletteController, GetRouletteGame, rouletteGame: " + rouletteGame + ", at Return");

                return NotFound(); // Return 404 Not Found if the roulette game with the given ID is not found
            }
            MonitorService.Log.Debug("RouletteController, GetRouletteGame, rouletteGame: " + rouletteGame + ", at Return");

            return new JsonResult(rouletteGame);
        }

        [HttpGet("/get/bet_types")]
        public async Task<IActionResult> GetBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetBetTypes, Start");

            // Retrieve the user's bet history from the database
            var betTypes = await GameDBConnection.QueryAsync<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet FROM bet_type");

            MonitorService.Log.Debug("RouletteController, GetBetTypes, BetType: [" + betTypes.ToString() + "], Start");

            return new JsonResult(betTypes);
        }


        [HttpGet("/get/bet_type/{id}")]
        public async Task<IActionResult> GetBetType(int id)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetBetType(int id), id: " + id + ", Start");

            // Retrieve the bet type based on the provided ID
            var betType = await GameDBConnection.QueryFirstOrDefaultAsync<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet  FROM bet_type WHERE bet_type_id = @Id", new { Id = id });

            if (betType == null)
            {
                MonitorService.Log.Debug("RouletteController, GetBetType, id: " + id + ", at Return");

                return NotFound(); // Return 404 Not Found if the bet type with the given ID is not found
            }
            MonitorService.Log.Debug("RouletteController, GetBetType, id: " + id + ", Bettype: " + betType.ToString() + ", at Return");

            return new JsonResult(betType);
        }

        [HttpGet("/get/game_bet_types")]
        public async Task<IActionResult> GetGameBetTypes()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameBetTypes, Start");
            // Retrieve the user's bet history from the database
            var gameBetTypes = await GameDBConnection.QueryAsync<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type");

            MonitorService.Log.Debug("RouletteController, GetGameBetTypes, GameBetType: [" + gameBetTypes.ToString() + "], at Return");

            return new JsonResult(gameBetTypes);
        }

        [HttpGet("/get/game_bet_types/{gameId}")]
        public async Task<IActionResult> GetGameBetTypesByGameId(int gameId)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, GetGameBetTypesByGameId, id: " + gameId + ", Start");

            // Retrieve game_bet_types based on the provided game_id
            var gameBetTypes = await GameDBConnection.QueryAsync<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type WHERE game_id = @GameId", new { GameId = gameId });

            MonitorService.Log.Debug("RouletteController, GetGameBetTypesByGameId, id: " + gameId + ", GameBetType: [" + gameBetTypes.ToString() + "], at Return");

            return new JsonResult(gameBetTypes);
        }




        [HttpPost("/post/bet")]
        public async Task<double> PostBet([FromQuery] int bet_type, [FromQuery] double bet_amount, [FromQuery] int bet_number, [FromQuery] string email, [FromQuery] string password)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, PostBet, Start");

            double winnings = 0;

            Console.WriteLine("StartPOST BET PASSWORD: " + password);

            var betType = GameDBConnection.QueryFirstOrDefault<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet  FROM bet_type WHERE bet_type_id = @Id", new { Id = bet_type });

            User user = await getUser(email, password);


            if (user != null)
            {
                Console.WriteLine(user.Balance);

                if (bet_amount <= (double)betType.MaxBet && bet_amount >= (double)betType.MinBet && user.Balance >= bet_amount)
                {
                    winnings = getSpinResults(betType.Name, bet_amount, (double)betType.Multiplier, bet_number) - bet_amount;
                    if (winnings > 0)
                    {
                        MonitorService.Log.Information($"Win: {winnings} for user:{user.ToString()}");
                    }
                    else
                    {
                        MonitorService.Log.Information($"Loss: {Math.Abs(winnings)} for user:{user.ToString()}");
                    }
                    await changeUserBalance(email, winnings);

                }
                else
                {

                    MonitorService.Log.Warning("Bet outside of Max or Min limits or higher than user balance");
                    MonitorService.Log.Information($"bet_amount: {bet_amount}, max: {betType.MaxBet}, min: {betType.MinBet}, balance: {user.Balance}, user: {user.ToString()}");
                }
            }

            MonitorService.Log.Debug("RouletteController, PostBet, at Return");

            return winnings;



        }

        private double getSpinResults(string bet_name, double bet_amount, double multiplier, int bet_number)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, getSpinResults, Start");


            Random random = new Random();
            var actualSpinResult = random.Next(1, 37);
            MonitorService.Log.Information($"actualSpinResult: {actualSpinResult}");
            Console.WriteLine("Spin result:" + actualSpinResult);
            bool isWinningSpin = false;
            switch (bet_name)
            {
                case "High":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at High, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult > 18)
                        isWinningSpin = true;
                    break;
                case "Low":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Low, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult <= 18)
                        isWinningSpin = true;
                    break;
                case "Even":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Even, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult % 2 == 0)
                        isWinningSpin = true;
                    break;
                case "Odd":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Odd, actualSpinResult: " + actualSpinResult);
                    if (actualSpinResult % 2 == 1)
                        isWinningSpin = true;
                    break;
                case "Exact Number":
                    MonitorService.Log.Debug("RouletteController, PostBet, inside switch at Exact Number, actualSpinResult: " + actualSpinResult + ", bet_number: " + bet_number);
                    if (actualSpinResult == bet_number)
                        isWinningSpin = true;
                    break;
            }
            if(isWinningSpin) 
            {
                MonitorService.Log.Information($"Winning spin with {bet_name}, bet of {bet_amount}");
                MonitorService.Log.Debug("RouletteController, getSpinResults, win at return");

                return bet_amount * multiplier;
            }
            else
            {

                MonitorService.Log.Information($"Loosing spin with {bet_name}, bet of {bet_amount}");
                MonitorService.Log.Debug("RouletteController, getSpinResults, loss at return");
                return 0;
            }
        }

        public async Task<User> getUser(string userEmail, string userPassword)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, getUser, Start");

            // Set the base URL of the user service
            string userServiceBaseUrl = "http://api-gateway-service";

            Console.WriteLine("User EMAIL:" +  userEmail);
            Console.WriteLine("User userPassword:" + userPassword);
            // Construct the URL for the GetUserByEmail API
            string getUserUrl = $"{userServiceBaseUrl}/userService/get/user?email={userEmail}&password={userPassword}";
            // Create an instance of HttpClient
            Console.WriteLine("GetUserurl: "  + getUserUrl);
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

                        Console.WriteLine("Vi fanger denne user: ");
                        Console.WriteLine(user.ToString());

                        Console.WriteLine($"User retrieved: {user}");
                        MonitorService.Log.Information($"user successfully retrieved {user.ToString()}");
                        return user;
                    }
                    else
                    {
                        MonitorService.Log.Warning($"user not retrieved with email: {userEmail}");

                        Console.WriteLine($"Get User 1 Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    MonitorService.Log.Error($"Exception while retrieving user Exception message {ex.Message}");

                    Console.WriteLine($"Get User 2 Error: {ex.Message}");
                }
            }
            return null;
        }

        public async Task changeUserBalance(string email, double amount)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("RouletteController, changeUserBalance, Start");

            // Set the base URL of the user service
            string userServiceBaseUrl = "http://api-gateway-service";

            // Construct the URL for the GetUserByEmail API
            string changeBalanceUrl = $"{userServiceBaseUrl}/userService/post/change-balance?email={email}&amount={amount}";
            // Create an instance of HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Make the GET request to the GetUserByEmail API
                    HttpResponseMessage response = await httpClient.PostAsync(changeBalanceUrl, null);

                    // Check if the request was successful (status code 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string content = await response.Content.ReadAsStringAsync();
                        MonitorService.Log.Information($"New Balance: {content}");

                        Console.WriteLine($"New balance: {content}");
                    }
                    else
                    {
                        MonitorService.Log.Warning($"Change User  Error: {response.StatusCode} - {response.ReasonPhrase}");
                        Console.WriteLine($"Change User  Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    MonitorService.Log.Error($"Exception while retrieving user Exception message {ex.Message}");
                    Console.WriteLine($"Change User Error: {ex.Message}");
                }
            }
        }
    }
}
