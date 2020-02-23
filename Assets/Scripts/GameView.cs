using UnityEngine;
using System;
using System.Collections.Generic;
using AssemblyCSharp;
using Grid = AssemblyCSharp.Grid;

public class GameView : MonoBehaviour
{
    public Board Board;
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private GameObject boardPiece;
    [SerializeField] private GameObject blackPiece;
    [SerializeField] private GameObject whitePiece;
    [SerializeField] private GameObject textPiece;

    private Board board;
    private Player[] players;
    
    private int widthStart;
    private int widthEnd;
    private int heightStart;
    private int heightEnd;
    private bool turn;
    private bool running;
    
    private void Start()
    {
        Board = new Board(this.width, this.height);

        AssembleBoard();

        turn = true;
        running = true;
    }

    private void Update()
    {
        if (! running)
            return;

        if (Board.CheckWin(Board.AllGrids) == "White")
        {
            var t = GameObject.Instantiate(textPiece) as GameObject;
            t.transform.localPosition = new Vector3(10, 1, -5);
            t.transform.parent = textPiece.transform.parent;
            t.GetComponent<TextMesh>().text = "Blue Wins";
            running = false;
            return;
        }

        if (Board.CheckWin(Board.AllGrids) == "Black")
        {
            var t = GameObject.Instantiate(textPiece) as GameObject;
            t.transform.localPosition = new Vector3(10, 1, -5);
            t.transform.parent = textPiece.transform.parent;
            t.GetComponent<TextMesh>().text = "Green Wins";
            running = false;
            return;
        }

        if (! turn)
        {
            AITurn();
            return;
        }

        if (! Input.GetMouseButtonDown(0))
            return;

        var mousePos = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
        
        for (int i = this.widthStart; i <= this.widthEnd; i = i + 2)
        {
            for (int j = this.heightStart; j <= this.heightEnd; j = j + 2)
            {
                if (Math.Abs(mousePos.x - i) < 1 &&
                    Math.Abs(mousePos.y - j) < 1 &&
                    Board.AllGrids[i / 2 + this.width / 2, j / 2 + this.height / 2].State == Grid.States.Unoccupied)
                {
                    MakeMove(i, j, blackPiece, Grid.States.Black);
                    turn = false;
                }
            }
        }
    }

    private void HandleOnPiecePlaced(int x, int y, Player player) {
        
    }
    
    private void AITurn()
    {
        var maxScore = -1;
        var rowColumn = new int[2];
        var allMoves = Board.AllMoves(Grid.States.White, Board.AllGrids);
        for (int i = 0; i < allMoves.Count; i ++)
        {
            var score = Board.BoardScore(allMoves[i]);
            if (score >= maxScore)
            {
                maxScore = score;
                rowColumn[0] = Board.moveRowColumn[i][0];
                rowColumn[1] = Board.moveRowColumn[i][1];
            }
        }

        MakeMove(rowColumn[0] * 2 - (this.width - 1), rowColumn[1]  * 2 - (this.height - 1),
            whitePiece, Grid.States.White);
        turn = true;
    }

    private void MakeMove(int x, int y, GameObject piece, Grid.States state)
    {
        var p = GameObject.Instantiate(piece) as GameObject;
        p.transform.localPosition = new Vector3(x, y, -2);
        p.transform.localScale = new Vector3(0.56f, 0.56f, 0f);
        p.transform.parent = piece.transform.parent;
        Board.AllGrids[x / 2 + this.width / 2, y / 2 + this.height / 2].State = state;
    }

    private void AssembleBoard()
    {
        this.widthStart = (this.width - 1) * -1;
        this.widthEnd = (this.width - 1);
        this.heightStart = (this.height - 1) * -1;
        this.heightEnd = (this.height - 1);

        var board = new List<int[]>();

        for (int i = widthStart; i <= widthEnd; i = i + 2)
        {
            for (int j = heightStart; j <= heightEnd; j = j + 2)
            {
                board.Add(new int[]{i, j});
                var b = GameObject.Instantiate(boardPiece) as GameObject;
                b.transform.localPosition = new Vector3(i, j, -1);
                b.transform.localScale = new Vector3(0.42f, 0.42f, 0f);
                b.transform.parent = boardPiece.transform.parent;
            }
        }
    }

    private void ResetBoard()
    {
    }

}

