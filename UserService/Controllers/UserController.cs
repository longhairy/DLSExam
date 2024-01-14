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
        //private readonly IDbConnection userDBConnection = new MySqlConnection("Server=user-db;Database=user-database;Uid=userdb;Pwd=C@ch3d1v;");
        private readonly IDbConnection userDBConnection;

        public UserController(IDbConnection userDBConnection)
        {
            this.userDBConnection = userDBConnection;

            MonitorService.Log.Debug("UserController Constructor Start");

            try
            {

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

                    userDBConnection.Execute("REPLACE INTO users (email, password, cpr, name, balance) VALUES ('test@email.com', 'password123', 123321, 'navnet', 33.00)");
                    userDBConnection.Execute("REPLACE INTO users (email, password, cpr, name, balance) VALUES ('1@1.dk', '123', 123, 'MyName', 10)");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors appropriately.
                MonitorService.Log.Error("Error in UserController constructor: " + ex.Message);
            }
            finally
            {
                MonitorService.Log.Debug("UserController Constructor, End");
            }

        }
        [HttpPost("/post/user")]
        public async Task SaveUser([FromQuery] string email, [FromQuery] string password, [FromQuery] int cpr, [FromQuery] string name, [FromQuery] double balance)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("UserController SaveUser, email: "+email+ ", password: "+password+ ", cpr: "+cpr+ ", name: "+name+ ", balance: "+balance+", Start");

       
            // TODO: update parameters 
            await userDBConnection.ExecuteAsync("REPLACE INTO users (email, password, cpr, name, balance) VALUES (@email, @password, @cpr, @name, @balance)",
            new { email = email, password = password, cpr = cpr, name = name, balance = balance });
   
        }

        [HttpGet("/get/users")]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("UserController GetUser, Start");
          
            var allUsers = await userDBConnection.QueryAsync<User>("SELECT * FROM users");

            MonitorService.Log.Debug("UserController GetUser, usersHistory: [" + allUsers.ToString() + "], at Return");

            return Ok(allUsers);
            
        }

        [HttpGet("/get/user")]
        public async Task<ActionResult<User>> GetUserByEmail([FromQuery] string email, [FromQuery] string password)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("UserController GetUserByEmail, email: "+email+ ", password: "+password+", Start");

      
            var user = await userDBConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE email = @email", new { email });

            if (user != null && user.Password == password)
            {
                MonitorService.Log.Debug("UserController GetUserByEmail, user: ["+user.ToString()+"], email: " + email + ", password: " + password + ", at Return");
                return Ok(user);
            }

            MonitorService.Log.Warning("User not found or password incorrect, email: "+email+", password: "+password+", at Return");
            return NotFound("User not found or incorrect password");
        }


        [HttpPost("/post/change-balance")]
        public async Task<double> ChangeUserBalance([FromQuery] string email, [FromQuery] double amount)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("UserController ChangeUserBalance, email: " + email + ", amount: " + amount + ", Start");
            // Ensure the balance cannot go below 0
            var currentBalance = await GetUserBalance(email);
            var newBalance = Math.Max(0, currentBalance + amount);
            //using (userDBConnection)
            //{
                await userDBConnection.ExecuteAsync("UPDATE users SET balance = @newBalance WHERE email = @email", new { email, newBalance });
                MonitorService.Log.Debug("UserController ChangeUserBalance, email: " + email + ", amount: " + amount + ", newBalance: " + newBalance + ", at return");

            return newBalance; //Ok(new { newBalance });
            //}
        }

        private async Task<double> GetUserBalance(string email)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            MonitorService.Log.Debug("UserController GetUserBalance, email: " + email + ", Start");
           // using (userDBConnection)
           // {
                return await userDBConnection.QueryFirstOrDefaultAsync<double>("SELECT balance FROM users WHERE email = @email", new { email });
           // }
                
        }


    }
}