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
    private Dictionary<Player, GameObject> pieces;

    private int widthStart;
    private int widthEnd;
    private int heightStart;
    private int heightEnd;
    
    private bool turn;
    private bool running;
    
    private Player Me { get { return this.players[this.turn ? 1 : 0]; } }
    private Player Opponent { get { return this.players[this.turn ? 0 : 1]; } }

    private void Start()
    {
        Calc.OnWin = this.HandleOnWin;
        this.board = new Board(this.width, this.height);
        this.board.OnPiecePlaced = HandleOnPiecePlaced;
        
        var player1 = new Player(1);
        var player2 = new Player(2);
        player1.OnMakeMove = HumanMakeMove;
        player2.OnMakeMove = AIMakeMove;
        
        this.players = new Player[2];
        this.players[0] = player1;
        this.players[1] = player2;
        
        this.pieces = new Dictionary<Player, GameObject>(2);
        this.pieces[player1] = this.blackPiece;
        this.pieces[player2] = this.whitePiece;

        this.turn = false;
        this.running = true;
        
        this.widthStart = (this.width - 1) * -1;
        this.widthEnd = (this.width - 1);
        this.heightStart = (this.height - 1) * -1;
        this.heightEnd = (this.height - 1);
        
        Board = new Board(this.width, this.height);
        AssembleBoard();
    }

    private void Update()
    {
        if (! running)
            return;
        
        this.Me.OnMakeMove();
    }
    
    private void OnDestroy() {
        foreach (var player in this.players) {
            player.OnMakeMove = null;
        }
        this.board.OnPiecePlaced = null;
        Calc.OnWin = null;
    }
    
    private void AIMakeMove() {
        var result = AI.Think(this.board, this.Me, this.Opponent);
        this.board.PlacePiece(result.Item1, result.Item2, this.Me);
        this.DoPostMakeMove();
    }
    
    private void HumanMakeMove() {
        
        if (! Input.GetMouseButtonDown(0))
            return;
        
        var mousePos = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
        int x = Convert.ToInt32(mousePos.x / 2) * 2;
        int y = Convert.ToInt32(mousePos.y / 2) * 2;
        var success = this.board.PlacePiece((x - 1 + this.width) / 2, (y - 1 + this.height) / 2, this.Me);
        
        if (! success)
            return;
        
        this.DoPostMakeMove();
    }
    
    private void DoPostMakeMove() {
        Calc.ValidateWin(this.board, this.Me, this.Opponent);
        this.turn = !this.turn;
    }
    
    private void HandleOnPiecePlaced(int x, int y, Player player) {
        var piece = this.pieces[player];
        var p = GameObject.Instantiate(piece) as GameObject;
        p.transform.localPosition = new Vector3(x * 2 - this.width + 1, y * 2 - this.height + 1, -2);
        p.transform.localScale = new Vector3(0.56f, 0.56f, 0f);
        p.transform.parent = piece.transform.parent;
    }
    
    private void HandleOnWin() {
        this.running = false;
        this.textPiece.SetActive(true);
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
}
