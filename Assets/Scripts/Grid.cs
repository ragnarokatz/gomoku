using System;

namespace AssemblyCSharp
{
    public class Grid
    {
        public enum States
        {
            Unoccupied,
            Black,
            White,
        }

        public States State;

        public Grid (States state)
        {
            State = state;
        }
    }
}

