using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SharedModels;
using System.Data;

namespace RouletteService.Controllers
{
    public class RouletteController : Controller
    {
        private IDbConnection GameDBConnection = new MySqlConnection("Server=roulette-db;Database=roulette-database;Uid=roulettedb;Pwd=C@ch3d1v;");

        public RouletteController()
        {
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
            // Retrieve the user's bet history from the database
            var gameTypes = GameDBConnection.Query<GameType>("SELECT Name, Description, url FROM game_type");


            return new JsonResult(gameTypes);
        }

        [HttpGet("/get/game_type/{id}")]
        public IActionResult GetRouletteGame(int id)
        {
            // Retrieve the roulette game based on the provided ID
            var rouletteGame = GameDBConnection.QueryFirstOrDefault<GameType>("SELECT game_type_id as GameTypeId, Name, Description, url FROM game_type WHERE game_type_id = @Id", new { Id = id });

            if (rouletteGame == null)
            {
                return NotFound(); // Return 404 Not Found if the roulette game with the given ID is not found
            }

            return new JsonResult(rouletteGame);
        }

        [HttpGet("/get/bet_types")]
        public IActionResult GetBetTypes()
        {
            // Retrieve the user's bet history from the database
            var betTypes = GameDBConnection.Query<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet FROM bet_type");


            return new JsonResult(betTypes);
        }


        [HttpGet("/get/bet_type/{id}")]
        public IActionResult GetBetType(int id)
        {
            // Retrieve the bet type based on the provided ID
            var betType = GameDBConnection.QueryFirstOrDefault<BetType>("SELECT bet_type_id as BetTypeId, name as Name, multiplier, max_bet as MaxBet, min_bet as MinBet  FROM bet_type WHERE bet_type_id = @Id", new { Id = id });

            if (betType == null)
            {
                return NotFound(); // Return 404 Not Found if the bet type with the given ID is not found
            }

            return new JsonResult(betType);
        }

        [HttpGet("/get/game_bet_types")]
        public IActionResult GetGameBetTypes()
        {
            // Retrieve the user's bet history from the database
            var gameBetTypes = GameDBConnection.Query<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type");


            return new JsonResult(gameBetTypes);
        }

        [HttpGet("/get/game_bet_types/{gameId}")]
        public IActionResult GetGameBetTypesByGameId(int gameId)
        {
            // Retrieve game_bet_types based on the provided game_id
            var gameBetTypes = GameDBConnection.Query<GameBetType>("SELECT game_bet_type_id as GameBetTypeId, game_id as GameId, bet_type_id as BetTypeId FROM game_bet_type WHERE game_id = @GameId", new { GameId = gameId });

            return new JsonResult(gameBetTypes);
        }




        [HttpPost("/Post/bet")]
        public double PostBet([FromQuery] int uid, [FromQuery] int bet_type, [FromQuery] double bet_amount, [FromQuery] int bet_number)
        {

            //Task<double>
            //RouletteGame game = new RouletteGame(uid, bet_type, bet_amount,bet_number);
            //game.Spin();
            // Retrieve the user's bet history from the database
            var betTypes = GameDBConnection.Query<RouletteGame>("SELECT * FROM roulette_game");

            // Return the bet history as JSON (you can customize this based on your needs)
            return 0;

        }
    }
}
