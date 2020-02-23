using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public static class Utils
    {
        public static int ConvertToPosition(int value, int length) {
            return value * 2 - length + 1;
        }
        
        public static int ConvertToIndex(int value, int length) {
            return (value - 1 + length) / 2;
        }
        
        public static int RoundToNearestPosition(float value, int remainder) {
            return Convert.ToInt32((value + remainder) / 2) * 2;
        }
    }
}