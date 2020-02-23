using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Player
    {
        private int symbol;
        
        public int Symbol { get { return this.symbol; } }
        
        public Player(int symbol) {
            this.symbol = symbol;
        }        
    }
}