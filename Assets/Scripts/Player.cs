﻿using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Player
    {
        private int symbol;
        
        public int Symbol { get { return this.symbol; } }
        
        public Action Think;

        public Player(int symbol) {
            if (symbol == 0) {
                throw new Exception("Cannot use 0 for player symbol, reserved.");
            }
            
            this.symbol = symbol;
        }
    }
}