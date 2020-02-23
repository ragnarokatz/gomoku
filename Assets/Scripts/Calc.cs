using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public static class Calc
    {
        private static Dictionary<int[], int> cases;
        public static Action OnWin;
        
        static Calc() {
            cases = new Dictionary<int[], int>();
            
            cases.Add(new int[]{1, 1, 1, 1, 1}, 100000);
            cases.Add(new int[]{1, 2, 2, 2, 2, 1}, 40000);
            cases.Add(new int[]{1, 1, 1, 1}, 40000);
            cases.Add(new int[]{2, 2, 1, 2}, 10000);
            cases.Add(new int[]{2, 1, 2, 2}, 10000);
            cases.Add(new int[]{1, 2, 2, 2, 2}, 10000);
            cases.Add(new int[]{2, 2, 2, 2, 1}, 10000);
            cases.Add(new int[]{1, 2, 2, 2}, 4000);
            cases.Add(new int[]{2, 2, 2, 1}, 4000);
            cases.Add(new int[]{1, 1, 1}, 1000);
            cases.Add(new int[]{1, 2, 2}, 400);
            cases.Add(new int[]{2, 2, 1}, 400);
            cases.Add(new int[]{1, 1}, 100);
            cases.Add(new int[]{2, 1}, 10);
            cases.Add(new int[]{1, 2}, 10);
        }
        
        public static int ScoreBoard(Board board, Player me, Player opponent) {
            var totalScore = 0;
            foreach (var kvp in Calc.cases)
            {
                var c = kvp.Key;
                var score = kvp.Value;
                totalScore += ScoreCase(c, score, board, me, opponent);
            }
            return totalScore;
        }
        
        private static int ScoreCase(int[] c, int score, Board board, Player me, Player opponent)
        {
            var totalScore = 0;
            var length = c.Length;
            
            for (int i = 0; i < board.Width - length + 1; i++)
                for (int j = 0; j < board.Height; j++)
                    totalScore += ScoreHorizontal(c, score, i, j, board, me, opponent);

            for (int i = 0; i < board.Width; i++)
                for (int j = 0; j < board.Height - length + 1; j++)
                    totalScore += ScoreVertical(c, score, i, j, board, me, opponent);

            for (int i = 0; i < board.Width - length + 1; i++)
                for (int j = 0; j < board.Height - length + 1; j++)
                    totalScore += ScoreDiagonalLeftRight(c, score, i, j, board, me, opponent);

            for (int i = board.Width - 1; i >= length - 1; i--)
                for (int j = 0; j < board.Height - length + 1; j++)
                    totalScore += ScoreDiagonalRightLeft(c, score, i, j, board, me, opponent);

            return totalScore;
        }

        private static int ScoreHorizontal(int[] c, int score, int x, int y, Board board, Player me, Player opponent) {
            var length = c.Length;
            for (int i = 0; i < length; i ++)
                if (!IsMatch(c[i], board.Cell(x + i, y), me, opponent))
                    return 0;
            return score;
        }
        
        private static int ScoreVertical(int[] c, int score, int x, int y, Board board, Player me, Player opponent) {
            var length = c.Length;
            for (int i = 0; i < length; i ++)
                if (!IsMatch(c[i], board.Cell(x, y + i), me, opponent))
                    return 0;
            return score;
        }
        
        private static int ScoreDiagonalLeftRight(int[] c, int score, int x, int y, Board board, Player me, Player opponent) {
            var length = c.Length;
            for (int i = 0; i < length; i ++)
                if (!IsMatch(c[i], board.Cell(x + i, y + i), me, opponent))
                    return 0;
            return score;
        }
        
        private static int ScoreDiagonalRightLeft(int[] c, int score, int x, int y, Board board, Player me, Player opponent) {
            var length = c.Length;
            for (int i = 0; i < length; i ++)
                if (!IsMatch(c[i], board.Cell(x - i, y + i), me, opponent))
                    return 0;
            return score;
        }
        
        private static bool IsMatch(int c, int cell, Player me, Player opponent) {
            if (c == 1)
                return cell == me.Symbol;
            else
                return cell == opponent.Symbol;
        }
        
        public static bool ValidateWin(Board board, Player player, Player opponent) {
            var c = new int[]{1, 1, 1, 1, 1};
            var s = 1;
            var score = ScoreCase(c, s, board, player, opponent);
            if (score >= s) {
                Calc.OnWin();
                return true;
            } else {
                return false;
            }
        }
    }
}