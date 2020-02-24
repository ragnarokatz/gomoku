using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameView : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private GameObject boardTile;
    [SerializeField] private GameObject blackStone;
    [SerializeField] private GameObject whiteStone;
    [SerializeField] private GameObject winText;

    private Board board;
    private Player[] players;
    private Dictionary<Player, GameObject> stoneTemplates;
    private List<GameObject> stones;
    
    private bool turn;
    private bool running;
    
    private Player me { get { return this.players[this.turn ? 1 : 0]; } }
    private Player opponent { get { return this.players[this.turn ? 0 : 1]; } }

    private void Start()
    {
        this.board = new Board(this.width, this.height);
        this.board.OnStonePlaced = HandleOnStonePlaced;
        this.board.OnWin = this.HandleOnWin;
        this.board.OnResetBoard = this.HandleOnResetBoard;
        
        var player1 = new Player(1, "Black");
        var player2 = new Player(2, "White");
        player1.OnMakeMove = HumanMakeMove;
        player2.OnMakeMove = AIMakeMove;
        
        this.players = new Player[2];
        this.players[0] = player1;
        this.players[1] = player2;
        
        this.stoneTemplates = new Dictionary<Player, GameObject>(2);
        this.stoneTemplates[player1] = this.blackStone;
        this.stoneTemplates[player2] = this.whiteStone;

        for (int i = 0; i < this.width; i++) {
            for (int j = 0; j < this.height; j++) {
                var tile = GameObject.Instantiate(boardTile) as GameObject;
                var posX = Utils.ConvertToPosition(i, this.width);
                var posY = Utils.ConvertToPosition(j, this.height);
                tile.transform.localPosition = new Vector3(posX, posY, -1);
                tile.transform.parent = boardTile.transform.parent;
            }
        }
        
        this.stones = new List<GameObject>(this.width * this.height);
        this.turn = false;
        this.running = true;
    }

    private void Update()
    {
        if (! running) {
            this.DoPostGame();
            return;
        }
        
        this.me.OnMakeMove();
    }
    
    private void OnDestroy() {
        foreach (var player in this.players) {
            player.OnMakeMove = null;
        }
        this.board.OnStonePlaced = null;
        this.board.OnWin = null;
        this.board.OnResetBoard = null;
    }
    
    private void AIMakeMove() {
        var result = AI.Think(this.board, this.me, this.opponent);
        this.board.PlaceStone(result.Item1, result.Item2, this.me);
        this.DoPostMakeMove();
    }
    
    private void HumanMakeMove() {
        
        if (! Input.GetMouseButtonDown(0))
            return;
        
        var mousePos = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
        int x = Utils.RoundToNearestPosition(mousePos.x, (this.width + 1) % 2);
        int y = Utils.RoundToNearestPosition(mousePos.y, (this.height + 1) % 2);
        x = Utils.ConvertToIndex(x, this.width);
        y = Utils.ConvertToIndex(y, this.height);
        var success = this.board.PlaceStone(x, y, this.me);
        
        if (! success)
            return;
        
        this.DoPostMakeMove();
    }
    
    private void DoPostMakeMove() {
        this.board.Validate(this.me, this.opponent);
        this.turn = !this.turn;
    }
    
    private void DoPostGame() {
        if (! Input.GetMouseButtonDown(0))
            return;
        
        this.board.ResetBoard();
    }
    
    private void HandleOnStonePlaced(int x, int y, Player player) {
        var stoneTemplate = this.stoneTemplates[player];
        var stone = GameObject.Instantiate(stoneTemplate) as GameObject;
        var posX = Utils.ConvertToPosition(x, this.width);
        var posY = Utils.ConvertToPosition(y, this.height);
        stone.transform.localPosition = new Vector3(posX, posY, -2);
        stone.transform.parent = stoneTemplate.transform.parent;
        this.stones.Add(stone);
    }
    
    private void HandleOnWin(Player player) {
        this.running = false;
        this.winText.SetActive(true);
        var text = this.winText.GetComponent<Text>();
        
        if (player != null)
            text.text = String.Format("{0} won!", player.Name);
        else
            text.text = "Draw!";
    }
    
    private void HandleOnResetBoard() {
        this.winText.SetActive(false);
        
        var length = this.stones.Count;
        for (int i = 0; i < length; i++) {
            var stone = this.stones[i];
            Destroy(stone);
        }
        
        this.stones = new List<GameObject>(this.width * this.height);
        this.turn = false;
        this.running = true;
    }
}
