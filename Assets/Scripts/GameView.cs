using UnityEngine;
using System;
using System.Collections.Generic;
using AssemblyCSharp;
using Grid = AssemblyCSharp.Grid;

public class GameView : MonoBehaviour
{
    public  GameBoard  Board;
    public  int        BoardSize;
    public  GameObject MyCamera;

    private GameObject blackPiece;
    private GameObject whitePiece;
    private GameObject textPiece;

    private int start;
    private int end;

    private string boardPiecePath = "Board/Boardpiece";
    private string whitePiecePath = "Pieces/White";
    private string blackPiecePath = "Pieces/Black";
    private string textPath       = "Display/Winning";

    private bool turn;

    private bool running;

    // Use this for initialization
    void Start ()
    {
        Board = new GameBoard(BoardSize);

        AssembleBoard();

        textPiece = GameObject.Find(textPath);

        turn = true;
        running = true;
    }

    // Update is called once per frame
    void Update ()
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

        for (int i = start; i <= end; i = i + 2)
        {
            for (int j = start; j <= end; j = j + 2)
            {
                var mousePos = MyCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                if (Math.Abs(mousePos.x - i) < 1 &&
                    Math.Abs(mousePos.y - j) < 1 &&
                    Board.AllGrids[i / 2 + Board.Length / 2, j / 2 + Board.Length / 2].State == Grid.States.Unoccupied)
                {
                    MakeMove(i, j, blackPiece, Grid.States.Black);
                    turn = false;
                }
            }
        }
    }

    void AITurn()
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

        MakeMove(rowColumn[0] * 2 - (Board.Length - 1), rowColumn[1]  * 2 - (Board.Length - 1),
            whitePiece, Grid.States.White);
        turn = true;
    }

    void MakeMove(int x, int y, GameObject piece, Grid.States state)
    {
        var p = GameObject.Instantiate(piece) as GameObject;
        p.transform.localPosition = new Vector3(x, y, -2);
        p.transform.localScale = new Vector3(0.56f, 0.56f, 0f);
        p.transform.parent = piece.transform.parent;
        Board.AllGrids[x / 2 + Board.Length / 2, y / 2 + Board.Length / 2].State = state;
    }

    void AssembleBoard()
    {
        start = (Board.Length - 1) * -1;
        end = (Board.Length - 1);

        var boardPiece = GameObject.Find(boardPiecePath);

        whitePiece = GameObject.Find(whitePiecePath);
        blackPiece = GameObject.Find(blackPiecePath);

        var board = new List<int[]>();

        for (int i = start; i <= end; i = i + 2)
        {
            for (int j = start; j <= end; j = j + 2)
            {
                board.Add(new int[]{i, j});
                var b = GameObject.Instantiate(boardPiece) as GameObject;
                b.transform.localPosition = new Vector3(i, j, -1);
                b.transform.localScale = new Vector3(0.42f, 0.42f, 0f);
                b.transform.parent = boardPiece.transform.parent;
            }
        }
    }

    void ResetBoard()
    {
    }

}

