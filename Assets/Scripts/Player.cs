using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Player
    {
        private int symbol;
        private String name;
        
        public int Symbol { get { return this.symbol; } }
        public String Name { get { return this.name; } }
        
        public Action OnMakeMove;

        public Player(int symbol, String name) {
            if (symbol == 0) {
                throw new Exception("Cannot use 0 for player symbol, reserved.");
            }
            
            this.name = name;
            this.symbol = symbol;
        }
    }
}