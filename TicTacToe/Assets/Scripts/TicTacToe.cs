using System;
using System.Collections.Generic;
using System.Linq;
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

    //////////////////////////////////////////////////////////

    public void CalculateConnectedCount()
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            var grid = _gridList[i];
            int connectedCount = 0;

            if (grid.GridData.GridType == GridType.Empty)
            {
                int x = grid.GridData.Coordinate.x;
                int y = grid.GridData.Coordinate.y;

                connectedCount = 2;

                if (x == y || x + y == boardSize - 1)
                {
                    connectedCount = 3; // 对角线 or 逆对角线 最大连通数：3
                }
                else if (x == y && x + y == boardSize - 1)
                {
                    connectedCount = 4; // 中心最大连通数：4
                }

                // 对角线是否连通？
                if (x == y)
                {
                    for (int l = 0; l < boardSize; l++)
                    {
                        if (!(_gridList[l * (boardSize + 1)].GridData.GridType == GridType.Empty ||
                              _gridList[l * (boardSize + 1)].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 逆对角线是否连通？
                if (x + y == boardSize - 1)
                {
                    for (int n = 0; n < boardSize; n++)
                    {
                        if (!(_gridList[(n + 1) * (boardSize - 1)].GridData.GridType == GridType.Empty ||
                              _gridList[(n + 1) * (boardSize - 1)].GridData.PawnType == grid.GridData.PawnType))
                        {
                            connectedCount--;
                            break;
                        }
                    }
                }

                // 列是否连通？
                for (int j = 0; j < boardSize; j++)
                {
                    if (!(_gridList[boardSize * j + x].GridData.GridType == GridType.Empty ||
                          _gridList[boardSize * j + x].GridData.PawnType == grid.GridData.PawnType))
                    {
                        connectedCount--;
                        break;
                    }
                }

                // 行是否连通？
                for (int k = 0; k < boardSize; k++)
                {
                    if (!(_gridList[boardSize * y + k].GridData.GridType == GridType.Empty ||
                          _gridList[boardSize * y + k].GridData.PawnType == grid.GridData.PawnType))
                    {
                        connectedCount--;
                        break;
                    }
                }
            }

            grid.GridData.ConnectedCount = connectedCount;
        }
    }

    public void CalculateMaxSameTypeCount()
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            var grid = _gridList[i];

            int maxSameTypeCountInPipeline = 0;
            int maxDiffTypeCountInPipeline = 0;

            int maxSameTypeCount = 0;
            int maxDiffTypeCount = 0;

            if (grid.GridData.GridType == GridType.Empty)
            {
                int x = grid.GridData.Coordinate.x;
                int y = grid.GridData.Coordinate.y;

                // 对角线
                if (x == y)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        if (_gridList[j * (boardSize + 1)].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_gridList[j * (boardSize + 1)].GridData.PawnType == PawnType.PlayerPawn)
                        {
                            maxDiffTypeCount++;
                            if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                            {
                                maxDiffTypeCountInPipeline = maxDiffTypeCount;
                            }
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 逆对角线
                if (x + y == boardSize - 1)
                {
                    for (int k = 0; k < boardSize; k++)
                    {
                        if (_gridList[(k + 1) * (boardSize - 1)].GridData.PawnType == PawnType.ComputePawn)
                        {
                            maxSameTypeCount++;
                            if (maxSameTypeCountInPipeline < maxSameTypeCount)
                            {
                                maxSameTypeCountInPipeline = maxSameTypeCount;
                            }
                        }

                        if (_gridList[(k + 1) * (boardSize - 1)].GridData.PawnType == PawnType.PlayerPawn)
                        {
                            maxDiffTypeCount++;
                            if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                            {
                                maxDiffTypeCountInPipeline = maxDiffTypeCount;
                            }
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 列
                for (int l = 0; l < boardSize; l++)
                {
                    if (_gridList[boardSize * l + x].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_gridList[boardSize * l + x].GridData.PawnType == PawnType.PlayerPawn)
                    {
                        maxDiffTypeCount++;
                        if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                        {
                            maxDiffTypeCountInPipeline = maxDiffTypeCount;
                        }
                    }
                }

                maxSameTypeCount = 0;
                maxDiffTypeCount = 0;

                // 行
                for (int m = 0; m < boardSize; m++)
                {
                    if (_gridList[boardSize * y + m].GridData.PawnType == PawnType.ComputePawn)
                    {
                        maxSameTypeCount++;
                        if (maxSameTypeCountInPipeline < maxSameTypeCount)
                        {
                            maxSameTypeCountInPipeline = maxSameTypeCount;
                        }
                    }

                    if (_gridList[boardSize * y + m].GridData.PawnType == PawnType.PlayerPawn)
                    {
                        maxDiffTypeCount++;
                        if (maxDiffTypeCountInPipeline < maxDiffTypeCount)
                        {
                            maxDiffTypeCountInPipeline = maxDiffTypeCount;
                        }
                    }
                }
            }

            grid.GridData.MaxSameTypeCountInPipeline = maxSameTypeCountInPipeline;
            grid.GridData.MaxDiffTypeCountInPipeline = maxDiffTypeCountInPipeline;
        }
    }


    public void CalculateWeights()
    {
        CalculateConnectedCount();
        CalculateMaxSameTypeCount();

        foreach (var grid in _gridList)
        {
            int maxSameTypeFactory = boardSize - 1;
            int maxDiffTypeFactory = boardSize;
            
            // 优先求胜
            if (grid.GridData.MaxSameTypeCountInPipeline == boardSize - 1)
            {
                maxSameTypeFactory = boardSize * boardSize;
            }
            // 再求不败
            else if (grid.GridData.MaxDiffTypeCountInPipeline == boardSize - 1)
            {
                maxDiffTypeFactory = boardSize * boardSize;
            }
            
            grid.GridData.Weight = 
                grid.GridData.ConnectedCount +
                grid.GridData.MaxSameTypeCountInPipeline * maxSameTypeFactory + 
                grid.GridData.MaxDiffTypeCountInPipeline * maxDiffTypeFactory;

            grid.debugTextConnectedCount.text = $"{grid.GridData.ConnectedCount}/{grid.GridData.MaxSameTypeCountInPipeline}/{grid.GridData.MaxDiffTypeCountInPipeline}->{grid.GridData.Weight}";
        }
    }

    public Grid GetHighestWeightsGrid()
    {
        return _gridList.OrderByDescending(x => x.GridData.Weight).FirstOrDefault();
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
                if (_lastPlacedGrid.GridData.PawnType != _gridList[(i + 1) * (boardSize - 1)].GridData.PawnType) return;
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
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize * i + x].GridData.PawnType) return;
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
            if (_lastPlacedGrid.GridData.PawnType != _gridList[boardSize * y + i].GridData.PawnType) return;
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
        InitChessboard();
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