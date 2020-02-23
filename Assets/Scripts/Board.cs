using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Board
    {
        public Grid[,] AllGrids;
        private int width;
        private int height;
        private List<Grid.States[]> stateCases;
        private List<int> stateScores;

        public List<int[]> moveRowColumn;
        
        private int[,] board;

        public Action<int, int, Player> OnPiecePlaced;
        
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }

        public bool PlacePiece(int x, int y, Player player) {
            
            if (x < 0 || x >= this.width) {
                Log.Trace("Cannot place piece at {0}, {1}, x is out of bounds.", x, y);
                return false;
            }
            
            if (y < 0 || y >= this.height) {
                Log.Trace("Cannot place piece at {0}, {1}, y is out of bounds.", x, y);
                return false;
            }
            
            if (this.board[x, y] != 0) {
                Log.Trace("Cannot place piece at {0}, {1}, already occupied.", x, y);
                return false;
            }
            
            this.board[x, y] = player.Symbol;
            this.validateBoard();
            
            if (this.OnPiecePlaced != null)
                this.OnPiecePlaced(x, y, player);
            
            return true;
        }
        
        private void validateBoard() {
            
        }
        
        public Board(int width, int height) {
            this.width = width;
            this.height = height;
            
            this.board = new int[this.width, this.height];
        }
        
        public Board(int width, int height, bool extra)
        {
            this.width = width;
            this.height = height;
            this.AllGrids = ResetGrid();

            stateCases  = new List<Grid.States[]>();
            stateScores = new List<int>();

            stateCases.Add(new Grid.States[5]{Grid.States.White, Grid.States.White, Grid.States.White, Grid.States.White, Grid.States.White});
            stateScores.Add(10000);

            stateCases.Add(new Grid.States[5]{Grid.States.Black, Grid.States.Black, Grid.States.Black, Grid.States.Black, Grid.States.Black});
            stateScores.Add(-10000);

            stateCases.Add(new Grid.States[4]{Grid.States.Black, Grid.States.Black, Grid.States.White, Grid.States.Black});
            stateScores.Add(1000);

            stateCases.Add(new Grid.States[4]{Grid.States.Black, Grid.States.White, Grid.States.Black, Grid.States.Black});
            stateScores.Add(1000);

            stateCases.Add(new Grid.States[6]{Grid.States.White, Grid.States.Black, Grid.States.Black, Grid.States.Black, Grid.States.Black, Grid.States.White});
            stateScores.Add(1000);

            stateCases.Add(new Grid.States[4]{Grid.States.White, Grid.States.White, Grid.States.White, Grid.States.White});
            stateScores.Add(25);

            stateCases.Add(new Grid.States[4]{Grid.States.White, Grid.States.Black, Grid.States.Black, Grid.States.Black});
            stateScores.Add(100);

            stateCases.Add(new Grid.States[4]{Grid.States.Black, Grid.States.Black, Grid.States.Black, Grid.States.White});
            stateScores.Add(100);

            stateCases.Add(new Grid.States[3]{Grid.States.White, Grid.States.White, Grid.States.White});
            stateScores.Add(1);

            stateCases.Add(new Grid.States[3]{Grid.States.White, Grid.States.Black, Grid.States.Black});
            stateScores.Add(10);

            stateCases.Add(new Grid.States[3]{Grid.States.White, Grid.States.Black, Grid.States.Black});
            stateScores.Add(10);
        }

        private Grid[,] ResetGrid()
        {
            var grid = new Grid[this.width, this.height];
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                    grid[i,j] = new Grid(Grid.States.Unoccupied);
            }
            return grid;
        }

        public string CheckWin(Grid[,] board)
        {
            if (CheckWinWhite(board))
                return "White";
            else if (CheckWinBlack(board))
                return "Black";
            return "None";
        }

        private bool CheckWinWhite(Grid[,] board)
        {
            if (StateScore(stateCases[0], board, stateScores[0]) == stateScores[0])
                return true;
            return false;
        }

        private bool CheckWinBlack(Grid[,] board)
        {
            if (StateScore(stateCases[1], board, stateScores[1]) == stateScores[1])
                return true;
            return false;
        }

        public List<Grid[,]> AllMoves(Grid.States turn, Grid[,] oldGrid)
        {
            moveRowColumn = new List<int[]>();

            var allMoves = new List<Grid[,]>();

            for (int i = 0; i < this.width; i ++)
            {
                for (int j = 0; j < this.height; j ++)
                {
                    if (oldGrid[i,j].State == Grid.States.Unoccupied)
                    {
                        allMoves.Add(Move(i, j, turn, oldGrid));
                        moveRowColumn.Add(new int[2]{i, j});
                    }
                }
            }

            return allMoves;
        }

        private Grid[,] Move(int x, int y, Grid.States turn, Grid[,] oldGrid)
        {
            var grid = new Grid[this.width, this.height];
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                    grid[i,j] = new Grid(oldGrid[i,j].State);
            }
            grid[x, y].State = turn;
            return grid;
        }

        public int BoardScore(Grid[,] grid)
        {
            var score = 0;
            for (int i = 0; i < stateCases.Count; i ++)
            {
                score += StateScore(stateCases[i], grid, stateScores[i]);
            }
            return score;
        }

        private int StateScore(Grid.States[] stateCase, Grid[,] grid, int stateScore)
        {
            var score = 0;
            for (int i = 0; i < grid.GetLength(0) - (stateCase.Length - 1); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    score += ScoreHorizontal(stateCase, i, j, grid, stateScore);
                }
            }

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j++)
                {
                    score += ScoreVertical(stateCase, i, j, grid, stateScore);
                }
            }

            for (int i = 0; i < grid.GetLength(0) - (stateCase.Length - 1); i ++)
            {
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j ++)
                {
                    score += ScoreDiagonalLeftRight(stateCase, i, j, grid, stateScore);
                }
            }

            for (int i = grid.GetLength(0) - 1; i >= stateCase.Length - 1; i --)
            {
                for (int j = 0; j < grid.GetLength(1) - (stateCase.Length - 1); j ++)
                {
                    score += ScoreDiagonalRightLeft(stateCase, i, j, grid, stateScore);
                }
            }

            return score;
        }

        private int ScoreHorizontal(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column + i, row].State)
                    return 0;
            }
            return stateScore;
        }

        private int ScoreVertical(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column, row + i].State)
                    return 0;
            }
            return stateScore;
        }

        private int ScoreDiagonalLeftRight(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column + i, row + i].State)
                    return 0;
            }
            return stateScore;

        }

        private int ScoreDiagonalRightLeft(Grid.States[] stateCase, int column, int row, Grid[,] grid, int stateScore)
        {
            for (int i = 0; i < stateCase.Length; i ++)
            {
                if (stateCase[i] != grid[column - i, row + i].State)
                    return 0;
            }
            return stateScore;

        }
    }
}
