using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public static class Calc
    {
        private static List<int[]> stateCases;
        private static List<int> stateScores;
        
        private static Dictionary<int[], int> cases;
        
        static Calc() {
            cases = new Dictionary<int[], int>();
            
            cases.Add(new int[]{1, 1, 1, 1, 1}, 100000);
            cases.Add(new int[]{1, 2, 2, 2, 2, 1}, 50000);
            cases.Add(new int[]{1, 1, 1, 1}, 50000);
            cases.Add(new int[]{2, 2, 1, 2}, 10000);
            cases.Add(new int[]{2, 1, 2, 2}, 10000);
            cases.Add(new int[]{1, 2, 2, 2, 2}, 10000);
            cases.Add(new int[]{2, 2, 2, 2, 1}, 10000);
            cases.Add(new int[]{1, 2, 2, 2}, 5000);
            cases.Add(new int[]{2, 2, 2, 1}, 5000);
            cases.Add(new int[]{1, 1, 1}, 1000);
            cases.Add(new int[]{1, 2, 2}, 500);
            cases.Add(new int[]{2, 2, 1}, 500);
            cases.Add(new int[]{1, 1}, 100);
            cases.Add(new int[]{2, 1}, 10);
            cases.Add(new int[]{1, 2}, 10);
        }
        
        private static int StateScore(Grid.States[] stateCase, Grid[,] grid, int stateScore)
        {
            var score = 0;
            for (int i = 0; i < grid.GetLength(0) - (stateCase.Length - 1); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    score += ScoreHorizontal(stateCase, i, j, grid, stateScore);

            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j++)
                    score += ScoreVertical(stateCase, i, j, grid, stateScore);

            for (int i = 0; i < grid.GetLength(0) - (stateCase.Length - 1); i ++)
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j ++)
                    score += ScoreDiagonalLeftRight(stateCase, i, j, grid, stateScore);

            for (int i = grid.GetLength(0) - 1; i >= stateCase.Length - 1; i --)
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j ++)
                    score += ScoreDiagonalRightLeft(stateCase, i, j, grid, stateScore);

            return score;
        }

        private static int ScoreHorizontal(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column + i, row].State)
                    return 0;
            }
            return stateScore;
        }

        private static int ScoreVertical(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column, row + i].State)
                    return 0;
            }
            return stateScore;
        }

        private static int ScoreDiagonalLeftRight(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column + i, row + i].State)
                    return 0;
            }
            return stateScore;

        }

        private static int ScoreDiagonalRightLeft(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column - i, row + i].State)
                    return 0;
            }
            return stateScore;

        }
        
        public static bool CheckWin(Board board, Player player) {
            return true;
        }
    }
}