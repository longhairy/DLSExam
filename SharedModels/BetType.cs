using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedModels
{
    public class BetType
    {
        [Column("bet_type_id")]
        [JsonPropertyName("betTypeId")]
        public int BetTypeId { get; set; }

        [Column("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Column("multiplier")]
        [JsonPropertyName("multiplier")]
        public decimal Multiplier { get; set; }

        [Column("max_bet")]
        [JsonPropertyName("maxBet")]
        public decimal MaxBet { get; set; }

        [Column("min_bet")]
        [JsonPropertyName("minBet")]
        public decimal MinBet { get; set; }
        public override string ToString()
        {
            return $"BetTypeId: {BetTypeId}, Name: {Name}, Multiplier: {Multiplier}, MaxBet: {MaxBet}, MinBet: {MinBet}";
        }
    }
}
