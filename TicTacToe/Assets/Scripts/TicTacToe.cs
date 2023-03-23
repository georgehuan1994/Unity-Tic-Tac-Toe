using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TicTacToe : MonoBehaviour
{
    private static TicTacToe _instance;
    public  static TicTacToe Instance => _instance;
    
    /* Inspector Property */
    public Transform chessboard;
    public GameObject gridPrefab;
    [Range(3, 9)] public int boardSize = 3;

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
    /// 初始化棋盘
    /// </summary>
    public void InitChessboard()
    {
        ClearChessboard();
        
        _isGameOver = false;
        _gridList = new List<Grid>();
        
        _currentStep = 0;
        IsPlayerTurn = true;
        
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Grid item = Instantiate(gridPrefab, chessboard).GetComponent<Grid>();
                item.InitGrid(new Vector2Int(x, y), GridType.Empty);
                _gridList.Add(item);
            }
        }
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

    private void ComputeMove()
    {
        int x, y, score = 1;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                var grid = GetGridViaCoord(j, i);
                if (grid.GridData.GridType == GridType.Empty)
                {
                    
                }
            }
        }
    }

    private int MaxSearch()
    {
        return 0;
    }
    
    private int MinSearch()
    {
        return 0;
    }

    public void SetLastPlacedGrid(Grid grid)
    {
        _lastPlacedGrid = grid;
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
                if (_lastPlacedGrid.GridData.PawnType != _gridList[i*(boardSize+1)].GridData.PawnType) return;
            }
            
            GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin);
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
                if (_lastPlacedGrid.GridData.PawnType != _gridList[(i+1)*(boardSize-1)].GridData.PawnType) return;
            }
            
            GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin);
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
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize*i+x].GridData.PawnType) return;
        }
        
        GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin);
    }

    /// <summary>
    /// 检查当前行
    /// </summary>
    private void CheckColumn()
    {
        int y = _lastPlacedGrid.GridData.Coordinate.y;
        for (int i = 0; i < boardSize; i++)
        {
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize*y+i].GridData.PawnType) return;
        }
        
        GameOver(IsPlayerTurn ? GameResult.PlayerWin : GameResult.ComputeWin);
    }
    
    /// <summary>
    /// 检查和棋
    /// </summary>
    private void CheckDraw()
    {
        if (_currentStep >= boardSize * boardSize)
        {
            GameOver(GameResult.Draw);
        }
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    /// <param name="gameResult"></param>
    private void GameOver(GameResult gameResult)
    {
        if (_isGameOver) return;
        _isGameOver = true;
        Debug.Log($"WINNER IS: {gameResult}");
    }

    /// <summary>
    /// 放置棋子
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="pawnType"></param>
    public void SetPawn(Vector2Int coord, PawnType pawnType = PawnType.ComputePawn)
    {
        var grid = GetGridViaCoord(coord);
        if (grid)
        {
            if (pawnType == PawnType.PlayerPawn)
            {
                grid.TrySetPlayerPawn();
            }
            else if (pawnType == PawnType.ComputePawn)
            {
                grid.TrySetComputePawn();
            }
        }
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
        int index = x + (int)boardSize * y;
        if (chessboard.childCount <= index)
        {
            Debug.LogWarning($"Can not find Grid-({x}, {y}) which Index-{index}, current chessboard grid length-{chessboard.childCount}");
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
        InitChessboard();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetPawn(new Vector2Int(2, 2));
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            InitChessboard();
        }
#endif
    }
}
