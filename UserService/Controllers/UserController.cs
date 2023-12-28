using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using Dapper;
using SharedModels;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IDbConnection userDBConnection = new MySqlConnection("Server=user-db;Database=user-database;Uid=userdb;Pwd=C@ch3d1v;");

        public UserController() 
        {
            userDBConnection.Open();
            var tables = userDBConnection.Query<string>("SHOW TABLES LIKE 'users'");
            if (!tables.Any())
            {
                userDBConnection.Execute("CREATE TABLE users (" +
                    "id INT NOT NULL AUTO_INCREMENT," +
                    "email VARCHAR(50) NOT NULL, " +
                    "password VARCHAR(50) NOT NULL, " +
                    "cpr INT NOT NULL, " +
                    "name VARCHAR(50) NOT NULL, " +
                    "balance DECIMAL(15,2) NOT NULL, " +
                    "PRIMARY KEY (id))");

                userDBConnection.ExecuteAsync("REPLACE INTO users (email, password, cpr, name, balance) VALUES ('test@email.com', 'password123', 123321, 'navnet', 33.00)");
            }
        }
        [HttpPost("/post/user")]
        public void SaveUser([FromQuery] string email, [FromQuery] string password, [FromQuery] int cpr, [FromQuery] string name, [FromQuery] double balance)
        {
            // TODO: update parameters 
            userDBConnection.ExecuteAsync("REPLACE INTO users (email, password, cpr, name, balance) VALUES (@email, @password, @cpr, @name, @balance)",
            new { email = email, password = password, cpr = cpr, name = name, balance = balance });
        }

        [HttpGet("/get/users")]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            var usersHistory = await userDBConnection.QueryAsync<User>("SELECT * FROM users"); // WHERE operation = 'multiplication'


            return Ok(usersHistory);

        }
    }
}