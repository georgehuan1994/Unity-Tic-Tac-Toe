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
    
    [Range(3, 9)]
    [SerializeField]
    private int boardSize = 3;

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
    /// 当前步数
    /// </summary>
    private uint _currentStep = 0;

    /// <summary>
    /// 游戏结束标识
    /// </summary>
    private bool _isGameOver = false;

    /// <summary>
    /// 最后一个被放置的格子
    /// </summary>
    private static Grid _lastPlacedGrid = null;

    /// <summary>
    /// 格子列表
    /// </summary>
    private static List<Grid> _gridList = new List<Grid>();

    /// <summary>
    /// 完成列表
    /// </summary>
    public static List<Grid> CompletedGrids = new List<Grid>();

    /// <summary>
    /// 初始化棋盘
    /// </summary>
    public void InitChessboard()
    {
        ClearChessboard();

        _isGameOver = false;
        _gridList = new List<Grid>();
        CompletedGrids = new List<Grid>();

        _currentStep = 0;
        IsPlayerTurn = true;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Grid item = Instantiate(gridPrefab, chessboard).GetComponent<Grid>();
                item.Init(new Vector2Int(x, y), GridType.Empty);
                
                _gridList.Add(item);
                
                float timer = 0;
                DOTween.To(() => timer, a => timer = a, 1f, 
                    GameConstant.GridInitShowInterval * (x + boardSize * y)).OnComplete(() =>
                {
                    item.ShowGrid();
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
        _gridList = null;

        foreach (Transform child in chessboard)
        {
            Destroy(child.gameObject);
        }
    }

    //////////////////////////////////////////////////////////

    public void SetLastPlacedGrid(Grid grid)
    {
        _lastPlacedGrid = grid;
        _currentStep++;
    }

    /// <summary>
    /// 检查获胜者
    /// </summary>
    public void CheckWinner()
    {
        CheckDiagonal();
        CheckInvDiagonal();
        CheckRow();
        CheckColumn();
        CheckDraw();

        if (!_isGameOver)
        {
            IsPlayerTurn = !IsPlayerTurn;
            OnRoundStart.Invoke(IsPlayerTurn);
        }
    }

    /// <summary>
    /// 检查对角线
    /// </summary>
    private void CheckDiagonal()
    {
        if (_lastPlacedGrid.GridData.Coordinate.x == _lastPlacedGrid.GridData.Coordinate.y)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (_lastPlacedGrid.GridData.PawnType != _gridList[i * (boardSize + 1)].GridData.PawnType) return;
            }
            
            GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin, CompletedLayout.Diagonal);
        }
    }

    /// <summary>
    /// 检查逆对角线
    /// </summary>
    private void CheckInvDiagonal()
    {
        if (_lastPlacedGrid.GridData.Coordinate.x + _lastPlacedGrid.GridData.Coordinate.y == boardSize - 1)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (_lastPlacedGrid.GridData.PawnType != _gridList[(i + 1) * (boardSize - 1)].GridData.PawnType) return;
            }

            GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin, CompletedLayout.InvDiagonal);
        }
    }

    /// <summary>
    /// 检查当前列
    /// </summary>
    private void CheckRow()
    {
        int x = _lastPlacedGrid.GridData.Coordinate.x;
        for (int i = 0; i < boardSize; i++)
        {
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize * i + x].GridData.PawnType) return;
        }

        GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin, CompletedLayout.Row);
    }

    /// <summary>
    /// 检查当前行
    /// </summary>
    private void CheckColumn()
    {
        int y = _lastPlacedGrid.GridData.Coordinate.y;
        for (int i = 0; i < boardSize; i++)
        {
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize * y + i].GridData.PawnType) return;
        }

        GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin, CompletedLayout.Column);
    }

    /// <summary>
    /// 检查和棋
    /// </summary>
    private void CheckDraw()
    {
        if (_currentStep >= boardSize * boardSize)
        {
            GameOver(GameResult.Draw, CompletedLayout.Incomplete);
        }
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="gameResult"></param>
    private void GameOver(GameResult gameResult, CompletedLayout completedLayout)
    {
        if (_isGameOver)
        {
            return;
        }
        _isGameOver = true;

        CompletedGrids = new List<Grid>();
        for (int i = 0; i < boardSize; i++)
        {
            switch (completedLayout)
            {
                case CompletedLayout.Incomplete:
                    CompletedGrids = _gridList;
                    break;
                case CompletedLayout.Diagonal:
                    CompletedGrids.Add(_gridList[i * (boardSize + 1)]);
                    break;
                case CompletedLayout.InvDiagonal:
                    CompletedGrids.Add(_gridList[(i + 1) * (boardSize - 1)]);
                    break;
                case CompletedLayout.Row:
                    CompletedGrids.Add(_gridList[boardSize * i + _lastPlacedGrid.GridData.Coordinate.x]);
                    break;
                case CompletedLayout.Column:
                    CompletedGrids.Add(_gridList[boardSize * _lastPlacedGrid.GridData.Coordinate.y + i]);
                    break;
            }
        }

        foreach (Grid grid in CompletedGrids)
        {
            if (completedLayout == CompletedLayout.Incomplete)
            {
                grid.DrawHighlight();
            }
            else
            {
                grid.CompletedHighlight();
            }
        }

        OnGameOver.Invoke(gameResult);
        Debug.Log($"WINNER IS: {gameResult}");
    }

    public void AIMove()
    {
        if (_isGameOver) return;
        CalculateWeights();
        GetHighestWeightsGrid().TrySetComputePawn();
    }

    //////////////////////////////////////////////////////////

    /// <summary>
    /// 获取格子
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    private Grid GetGridViaCoord(Vector2Int coord)
    {
        return GetGridViaCoord(coord.x, coord.y);
    }

    /// <summary>
    /// 获取格子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Grid GetGridViaCoord(int x, int y)
    {
        int index = x + boardSize * y;
        if (chessboard.childCount <= index)
        {
            Debug.LogWarning(
                $"Can not find Grid-({x}, {y}) which Index-{index}, current chessboard grid length-{chessboard.childCount}");
            return null;
        }

        return chessboard.GetChild(index).GetComponent<Grid>();
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
        if (Input.GetKeyUp(KeyCode.F))
        {
            CalculateWeights();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CalculateWeights();
            GetHighestWeightsGrid().TrySetComputePawn();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            InitChessboard();
        }
#endif
    }
}