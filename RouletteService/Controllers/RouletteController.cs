﻿using Dapper;
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
                    "PRIMARY KEY (game_type_id))");
            }
            GameDBConnection.Execute("INSERT INTO " +
                   "game_type (name,description)" +
                   "values ('Roulette','A game where a ball is spinned around a disc, and lands on a random number between 1 and 36')");


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
            var gameTypes = GameDBConnection.Query<String>("SELECT * FROM game_type");

            // Return the bet history as JSON (you can customize this based on your needs)
            //return Json(gameTypes);

         

            // Return a JsonResult with the JSON object
            return new JsonResult(gameTypes);
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