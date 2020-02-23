using System;

namespace AssemblyCSharp
{
    public class Grid
    {
        public enum States
        {
            Unoccupied = 0,
            Black = 1,
            White = 2,
        }

        public States State;

        public Grid (States state)
        {
            State = state;
        }
    }
}

