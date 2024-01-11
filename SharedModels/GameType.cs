using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class GameType
    {
        public int GameTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public override string ToString()
        {
            // return $"Game Type ID: {GameTypeId}, Name: {Name}, Description: {Description}, URL: {Url}"; 
            return Name;
        }
    }

}
