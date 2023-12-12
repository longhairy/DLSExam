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
                    "name VARCHAR(MAX) NOT NULL, " +
                    "balance DECIMAL(15,2) NOT NULL, " +
                    "PRIMARY KEY (id))");
            }
        }
        [HttpPost("/post/user")]
        public void SaveUser([FromQuery] long inputone, [FromQuery] long inputtwo, [FromQuery] long output)
        {
            // TODO: update parameters 
            userDBConnection.ExecuteAsync("REPLACE INTO users (inputone, inputtwo, output, operation) VALUES (@inputone, @inputtwo, @output, 'addition')", new { inputone = inputone, inputtwo = inputtwo, output = output, operation = "addition" });
        }

        //[HttpGet("/get/user")]
        //public async Task<ActionResult<List<User>>> GetUser()
        //{ 

        //}
     }
}