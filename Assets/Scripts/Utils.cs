using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public static class Utils
    {
        public static int ConvertToPositions(int value, int length) {
            return value * 2 - length + 1;
        }
        
        public static int ConvertToIndices() {
            return 0;
        }
    }
}