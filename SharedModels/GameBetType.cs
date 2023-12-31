using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class GameBetType
    {
        [Column("game_bet_type_id")]
        public int GameBetTypeId { get; set; }

        [Column("game_id")]
        public int GameId { get; set; }

        [Column("bet_type_id")]
        public int BetTypeId { get; set; }



    }
}
