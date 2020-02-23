using UnityEngine;
using System;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameView : MonoBehaviour
{
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

        for (int i = 0; i < this.width; i++) {
            for (int j = 0; j < this.height; j++) {
                var b = GameObject.Instantiate(boardPiece) as GameObject;
                var posX = Utils.ConvertToPositions(i, this.width);
                var posY = Utils.ConvertToPositions(j, this.height);
                b.transform.localPosition = new Vector3(posX, posY, -1);
                b.transform.localScale = new Vector3(0.42f, 0.42f, 0f);
                b.transform.parent = boardPiece.transform.parent;
            }
        }
        
        this.turn = false;
        this.running = true;
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
        var posX = Utils.ConvertToPositions(x, this.width);
        var posY = Utils.ConvertToPositions(y, this.height);
        p.transform.localPosition = new Vector3(posX, posY, -2);
        p.transform.localScale = new Vector3(0.56f, 0.56f, 0f);
        p.transform.parent = piece.transform.parent;
    }
    
    private void HandleOnWin() {
        this.running = false;
        this.textPiece.SetActive(true);
    }
}
