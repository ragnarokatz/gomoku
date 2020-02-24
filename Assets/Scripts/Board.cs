using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Board
    {
        private int width;
        private int height;
        private int[,] board;

        public Action<int, int, Player> OnStonePlaced;
        public Action<Player> OnWin;
        public Action OnResetBoard;

        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }
        
        public Board(int width, int height) {
            this.width = width;
            this.height = height;
            this.board = new int[this.width, this.height];
        }
        
        public Board(Board b) {
            this.width = b.width;
            this.height = b.height;
            
            this.board = new int[this.width, this.height];
            for (int i = 0; i < this.width; i++) {
                for (int j = 0; j < this.height; j++) {
                    this.board[i, j] = b.board[i, j];
                }
            }
        }
        
        public int Cell(int x, int y) {
            return this.board[x, y];
        }
        
        public bool PlaceStone(int x, int y, Player player) {
            
            if (x < 0 || x >= this.width) {
                Log.Trace("Cannot place stone at {0}, {1}, x is out of bounds.", x, y);
                return false;
            }
            
            if (y < 0 || y >= this.height) {
                Log.Trace("Cannot place stone at {0}, {1}, y is out of bounds.", x, y);
                return false;
            }
            
            if (this.board[x, y] != 0) {
                Log.Trace("Cannot place stone at {0}, {1}, already occupied.", x, y);
                return false;
            }
            
            this.board[x, y] = player.Symbol;
            
            if (this.OnStonePlaced != null)
                this.OnStonePlaced(x, y, player);
            
            return true;
        }
        
        public void RemoveStone(int x, int y) {
            this.board[x, y] = 0;
        }
        
        public bool Validate(Player player, Player opponent) {
            var score = Calc.ScoreCase(Calc.CASE_WIN, Calc.SCORE_WIN, this, player, opponent);
            if (score >= Calc.SCORE_WIN) {
                this.OnWin(player);
                return true;
            } else {
                if (this.IsFilled()) {
                    this.OnWin(null);
                }
                return false;
            }
        }
        
        public void ResetBoard() {
            this.board = new int[this.width, this.height];
            this.OnResetBoard();
        }
        
        private bool IsFilled() {
            for (int i = 0; i < this.width; i++) {
                for (int j = 0; j < this.height; j++) {
                    if (this.board[i, j] == 0)
                        return false;
                }
            }
            
            return true;
        }
    }
}
