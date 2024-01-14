using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedModels
{
    public class GameType
    {
        [JsonPropertyName("gameTypeId")]
        public int GameTypeId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        public override string ToString()
        {
            return $"Game Type ID: {GameTypeId}, Name: {Name}, Description: {Description}, URL: {Url}"; 
        }
    }

}
