using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;

namespace RouletteService.Controllers
{
    public class RouletteController : Controller
    {
        private IDbConnection rouletteDBConnection = new MySqlConnection("Server=roulette-db;Database=roulette-database;Uid=userdb;Pwd=C@ch3d1v;");

        public RouletteController()
        {
            rouletteDBConnection.Open();
            var bet_type_tables = rouletteDBConnection.Query<string>("SHOW TABLES LIKE 'bet_type'");
            if (!bet_type_tables.Any())
            {
                rouletteDBConnection.Execute("CREATE TABLE bet_type (" +
                    "id INT NOT NULL AUTO_INCREMENT," +
                    "name VARCHAR(50) NOT NULL, " +
                    "multiplier DECIMAL(2,2) NOT NULL, " +
                    "max_bet Decimal(10,2) NOT NULL, " +
                    "min_bet Decimal(10,2) NOT NULL, " +
                    "PRIMARY KEY (id))");
            }
            //comment
        }
        //[HttpPost("/post/user")]
        //public void SaveUser([FromQuery] long inputone, [FromQuery] long inputtwo, [FromQuery] long output)
        //{
        //    // TODO: update parameters 
        //    userDBConnection.ExecuteAsync("REPLACE INTO users (inputone, inputtwo, output, operation) VALUES (@inputone, @inputtwo, @output, 'addition')", new { inputone = inputone, inputtwo = inputtwo, output = output, operation = "addition" });
        //}
    }
}
