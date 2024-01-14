using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedModels
{
    public class RouletteGame
    {

        [Column("gid")]
        private int gid { get; set; }
        [Column("uid")]
        private int uid { get; set; }
        [Column("date")]
        private DateTime date { get; set; }

        [Column("bet_type_id")]
        private BetType? bet_type_id { get; set; }
        [Column("bet_amount")]
        private double bet_amount { get; set; }
        [Column("bet_number")]
        private int bet_number { get; set; }
        [Column("result")]
        public double result { get; set; }

    }
}
