using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class TicTacToe : MonoBehaviour
{
    private static TicTacToe _instance;
    public  static TicTacToe Instance => _instance;
    
    [SerializeField]
    private Transform chessboard;
    
    [SerializeField]
    private GameObject gridPrefab;
    
    [Range(3, 4)]
    [SerializeField]
    public int boardSize = 3;

    /// <summary>
    /// 对局开始时
    /// </summary>
    public delegate void GameStartDelegate();
    public GameStartDelegate OnGameStart;

    /// <summary>
    /// 回合开始时
    /// </summary>
    public delegate void RoundStartDelegate(bool isPlayerTurn);
    public RoundStartDelegate OnRoundStart;
    
    /// <summary>
    /// 棋子放置时
    /// </summary>
    public delegate void PawnPlacedDelegate();
    public PawnPlacedDelegate OnPawnPlaced;

    /// <summary>
    /// 对局结束时
    /// </summary>
    public delegate void GameOverDelegate(GameResult gameResult);
    public GameOverDelegate OnGameOver;

    /// <summary>
    /// 是否为玩家回合
    /// </summary>
    public bool IsPlayerTurn { get; set; }

    /// <summary>
    /// 游戏结束标识
    /// </summary>
    private bool _isGameOver;

    /// <summary>
    /// 最后一个被放置的 Grid
    /// </summary>
    public Grid LastPlacedGrid
    {
        get => _lastPlacedGrid;
        set
        {
            _lastPlacedGrid = value;
            if (value != null)
            {
                GameOverCheck(value.GridData.Coordinate, value.GridData.PawnType);
                OnPawnPlaced.Invoke();
                if (!_isGameOver)
                {
                    IsPlayerTurn = !IsPlayerTurn;
                    OnRoundStart.Invoke(IsPlayerTurn);
                }
            }
        }
    }
    
    private static Grid _lastPlacedGrid;

    /// <summary>
    /// Grid 数组
    /// </summary>
    private static Grid[,] _grids;
    
    /// <summary>
    /// Pawn 数组
    /// </summary>
    private static PawnType[,] _boardData;

    /// <summary>
    /// 完成列表
    /// </summary>
    private static List<Grid> _completedGrids = new();

    /// <summary>
    /// 初始化棋盘
    /// </summary>
    public void InitChessboard()
    {
        ClearChessboard();

        _isGameOver = false;
        _boardData = new PawnType[boardSize, boardSize];
        _grids = new Grid[boardSize, boardSize];
        _completedGrids = new List<Grid>();
        
        IsPlayerTurn = true;
        LastPlacedGrid = null;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Grid grid = Instantiate(gridPrefab, chessboard).GetComponent<Grid>();
                grid.Init(new Vector2Int(x, y), GridType.Empty);
                
                _grids[x, y] = grid;
                
                float timer = 0;
                DOTween.To(() => timer, a => timer = a, 1f, 
                    GameConstant.GridInitShowInterval * (x + boardSize * y)).OnComplete(() =>
                {
                    grid.ShowGrid();
                });
            }
        }
        
        OnGameStart.Invoke();
        OnRoundStart.Invoke(IsPlayerTurn);
    }

    /// <summary>
    /// 清理棋盘
    /// </summary>
    private void ClearChessboard()
    {
        _boardData = new PawnType[boardSize, boardSize];
        _grids = new Grid[boardSize, boardSize];

        foreach (Transform child in chessboard)
        {
            Destroy(child.gameObject);
        }
    }

    //////////////////////////////////////////////////////////

    /// <summary>
    /// AI 操作
    /// </summary>
    public void AIMove()
    {
        if (_isGameOver) return;
        GetBestMove(PawnType.ComputePawn).TrySetComputePawn();
    }

    //////////////////////////////////////////////////////////

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="gameResult"></param>
    private void GameOver(GameResult gameResult, CompletedLayout completedLayout)
    {
        if (gameResult == GameResult.Continue)
        {
            return;
        }
        
        if (_isGameOver)
        {
            return;
        }
        _isGameOver = true;

        _completedGrids = new List<Grid>();
        for (int i = 0; i < boardSize; i++)
        {
            if (completedLayout == CompletedLayout.Full)
            {
                foreach (Grid grid in _grids)
                {
                    grid.TiesHighlight();
                }
                break;
            }
            
            switch (completedLayout)
            {
                case CompletedLayout.Diagonal:
                    _completedGrids.Add(_grids[i, i]);
                    break;
                case CompletedLayout.InvDiagonal:
                    _completedGrids.Add(_grids[i, boardSize - i - 1]);
                    break;
                case CompletedLayout.Row:
                    _completedGrids.Add(_grids[LastPlacedGrid.GridData.Coordinate.x, i]);
                    break;
                case CompletedLayout.Column:
                    _completedGrids.Add(_grids[i, LastPlacedGrid.GridData.Coordinate.y]);
                    break;
            }
        }
        
        foreach (Grid grid in _completedGrids)
        {
            grid.CompletedHighlight();
        }

        OnGameOver.Invoke(gameResult);
        Debug.Log($"WINNER IS: {gameResult}");
    }

    //////////////////////////////////////////////////////////
    

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(_instance.gameObject);
    }

    private void Start()
    {
        Invoke("InitChessboard", 1);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetBestMove(PawnType.ComputePawn).TrySetComputePawn();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            InitChessboard();
        }
#endif
    }
}