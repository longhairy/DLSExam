using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedModels
{
    public class RouletteGame
    {

        public RouletteGame(int uid, int bet_type, double bet_amount, int bet_number)
        {
            this.uid = uid;
            this.type = new(bet_type);
            this.bet_amount = bet_amount;
            this.bet_number = bet_number;
        }

        private int gid { get; set; }
        private int uid { get; set; }
        private DateTime date { get; set; }
        private BetType type { get; set; }
        private double bet_amount { get; set; }
        private int bet_number { get; set; }
        public double result { get; set; }

        public void Spin()
        {
            Random rand = new Random();
            int spinresult = rand.Next(1,37);
            bool isWinning = false;
            switch(type.id)
            {
                case 1:
                    if(spinresult%0==0 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
                        isWinning = true;
                    break;
                case 2:
                    if (spinresult % 0 == 1 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
                        isWinning = true;
                    break;
                case 3:
                    if(spinresult >=19 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
                        isWinning = true;
                    break;
                case 4:
                    if (spinresult <= 18 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
                        isWinning = true;
                    break;
                case 5:
                    if(spinresult == bet_number && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
                        isWinning = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
            if(isWinning)
            {
                result = bet_amount * type.multiplier;
            }
        }
        internal class BetType
        {
            public BetType(int id) 
            {
                this.id = id;
                switch (id)
                {
                    case 1:
                        name = "Even";
                        multiplier = 2;
                        max_bet = 100;
                        min_bet = 1;
                        break;
                    case 2:

                        name = "Odd";
                        multiplier = 2;
                        max_bet = 100;
                        min_bet = 1;
                        break;
                    case 3:

                        name = "High";
                        multiplier = 2;
                        max_bet = 100;
                        min_bet = 1;
                        break;
                    case 4:

                        name = "Low";
                        multiplier = 2;
                        max_bet = 100;
                        min_bet = 1;
                        break;
                    case 5:

                        name = "Exact Number";
                        multiplier = 36;
                        max_bet = 10;
                        min_bet = 1;
                        break;
                    default:
                        throw new NotImplementedException();

                }
            }
            public int id { get; set; }
            public string name { get; set; }
            public double multiplier { get; set; }
            public double max_bet { get; set; }
            public double min_bet { get; set;}

        }
    }
}
