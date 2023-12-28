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

        //public RouletteGame(int uid, Enum betType, double betAmount, int betNumber)
        //{
        //    this.uid = uid;
        //    this.bet_amount = bet_amount;
        //    this.bet_number = bet_number;
        //}

        private int gid { get; set; }
        private int uid { get; set; }
        private DateTime date { get; set; }
        private enum BetType { Even,Odd,High,Low,ExactNumber }
        private double bet_amount { get; set; }
        private int bet_number { get; set; }
        public double result { get; set; }

        //public void Spin()
        //{
        //    Random rand = new Random();
        //    int spinresult = rand.Next(1,37);
        //    bool isWinning = false;
        //    switch(type.id)
        //    {
        //        case 1:
        //            if(spinresult%0==0 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
        //                isWinning = true;
        //            break;
        //        case 2:
        //            if (spinresult % 0 == 1 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
        //                isWinning = true;
        //            break;
        //        case 3:
        //            if(spinresult >=19 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
        //                isWinning = true;
        //            break;
        //        case 4:
        //            if (spinresult <= 18 && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
        //                isWinning = true;
        //            break;
        //        case 5:
        //            if(spinresult == bet_number && bet_amount >= type.min_bet && bet_amount <= type.max_bet)
        //                isWinning = true;
        //            break;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //    if(isWinning)
        //    {
        //        result = bet_amount * type.multiplier;
        //    }
        //}
       
            

    }
}
