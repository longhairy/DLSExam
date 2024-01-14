using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedModels
{
    public class GameBetType
    {
        [Column("game_bet_type_id")]
        [JsonPropertyName("gameBetTypeId")]
        public int GameBetTypeId { get; set; }

        [Column("game_id")]
        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [Column("bet_type_id")]
        [JsonPropertyName("betTypeId")]
        public int BetTypeId { get; set; }

        public override string ToString()
        {
            return $"GameBetTypeId: {GameBetTypeId}, GameId: {GameId}, BetTypeId: {BetTypeId}";
        }

    }
}
