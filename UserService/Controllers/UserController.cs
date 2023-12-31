using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;
using Dapper;
using SharedModels;
using System.Data.Common;
using Monitoring;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IDbConnection userDBConnection = new MySqlConnection("Server=user-db;Database=user-database;Uid=userdb;Pwd=C@ch3d1v;");

        public UserController()
        {
            MonitorService.Log.Debug("UserController Constructor Start");

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
                userDBConnection.ExecuteAsync("REPLACE INTO users (email, password, cpr, name, balance) VALUES ('1@1.dk', '123', 123, 'MyName', 10)");
            }
        }
        [HttpPost("/post/user")]
        public void SaveUser([FromQuery] string email, [FromQuery] string password, [FromQuery] int cpr, [FromQuery] string name, [FromQuery] double balance)
        {
            MonitorService.Log.Debug("UserController SaveUser, email: "+email+ ", password: "+password+ ", cpr: "+cpr+ ", name: "+name+ ", balance: "+balance+", Start");

            // TODO: update parameters 
            userDBConnection.ExecuteAsync("REPLACE INTO users (email, password, cpr, name, balance) VALUES (@email, @password, @cpr, @name, @balance)",
            new { email = email, password = password, cpr = cpr, name = name, balance = balance });
        }

        [HttpGet("/get/users")]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            MonitorService.Log.Debug("UserController GetUser, Start");
            var usersHistory = await userDBConnection.QueryAsync<User>("SELECT * FROM users");


            return Ok(usersHistory);

        }

        [HttpGet("/get/user")]
        public ActionResult GetUserByEmail([FromQuery] string email, [FromQuery] string password)
        {
            MonitorService.Log.Debug("UserController GetUserByEmail, email:"+email+ ", password:"+password+", Start");

            var user = userDBConnection.QueryFirstOrDefault<User>("SELECT * FROM users WHERE email = @email", new { email });

            if (user != null && user.Password == password)
            {
                return Ok(user);
            }

            return NotFound("User not found or incorrect password");
        }


        [HttpPost("/post/change-balance")]
        public ActionResult ChangeUserBalance([FromQuery] string email, [FromQuery] double amount)
        {
            MonitorService.Log.Debug("UserController ChangeUserBalance, email:" + email + ", amount:" + amount + ", Start");
            // Ensure the balance cannot go below 0
            var newBalance = Math.Max(0, GetUserBalance(email) + amount);

            userDBConnection.ExecuteAsync("UPDATE users SET balance = @newBalance WHERE email = @email",
                new { email, newBalance });

            return Ok(new { newBalance });
        }

        private double GetUserBalance(string email)
        {
            MonitorService.Log.Debug("UserController GetUserBalance, email:" + email + ", Start");

            return userDBConnection.QueryFirstOrDefault<double>("SELECT balance FROM users WHERE email = @email", new { email });
        }


    }
}