using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public static class AI
    {
        public static Tuple<int, int> Think(Board board, Player me, Player opponent) {
            var maxScore = 0;
            var maxScoreX = 0;
            var maxScoreY = 0;
            
            var copy = new Board(board);
            
            for (int i = 0; i < copy.Width; i++) {
                for (int j = 0; j < copy.Height; j++) {
                    if (copy.Cell(i, j) != 0)
                        continue;
                    
                    copy.PlacePiece(i, j, me);
                    
                    var score = Calc.ScoreBoard(copy, me, opponent);
                    if (score > maxScore) {
                        maxScore = score;
                        maxScoreX = i;
                        maxScoreY = j;
                    }
                    
                    copy.RemovePiece(i, j);
                }
            }
            
            return new Tuple<int, int>(maxScoreX, maxScoreY);
        }
    }
}