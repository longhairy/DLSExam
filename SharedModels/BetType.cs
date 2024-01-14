using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class BetType
    {
        [Column("bet_type_id")]
        public int BetTypeId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("multiplier")]
        public decimal Multiplier { get; set; }

        [Column("max_bet")]
        public decimal MaxBet { get; set; }

        [Column("min_bet")]
        public decimal MinBet { get; set; }
        public override string ToString()
        {
            return $"BetTypeId: {BetTypeId}, Name: {Name}, Multiplier: {Multiplier}, MaxBet: {MaxBet}, MinBet: {MinBet}";
        }
    }
}
