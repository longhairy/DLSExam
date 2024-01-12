using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SharedModels
{
    public class User
    {

        [Column("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Column("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [Column("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [Column("cpr")]
        [JsonPropertyName("cpr")]
        public int Cpr { get; set; }
        [Column("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Column("balance")]
        [JsonPropertyName("balance")]
        public double Balance { get; set; }      

        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}, Password: {Password}, Cpr: {Cpr}, Name: {Name}, Balance: {Balance}";
        }
    }
}
