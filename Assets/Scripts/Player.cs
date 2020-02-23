using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Player
    {
        private Board board;
        private int symbol;
        
        public int Symbol { get { return this.symbol; } }
        
        public Player(Board board, int symbol) {
            this.board = board;
            this.symbol = symbol;
        }
    }
}