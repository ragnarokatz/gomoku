using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Board
    {
        private int width;
        private int height;
        private int[,] board;

        public Action<int, int, Player> OnPiecePlaced;
        
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
            
            if (this.OnPiecePlaced != null)
                this.OnPiecePlaced(x, y, player);
            
            return true;
        }
        
        public void RemovePiece(int x, int y) {
            this.board[x, y] = 0;
        }
    }
}
