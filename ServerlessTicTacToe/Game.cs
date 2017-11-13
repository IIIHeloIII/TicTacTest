using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTicTacToe
{
    public class Game : TableEntity
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int PlayerTurn { get; set; }
        public string Row1 { get; set; }
        public string Row2 { get; set; }
        public string Row3 { get; set; }
    }
}
