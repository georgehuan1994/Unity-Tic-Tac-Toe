using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum GridType
{
    Unknown = 0,
    Locked,     // 禁用
    Empty,      // 空
    Pawn,       // 已放置
}

/// <summary>
/// 棋子类型
/// </summary>
public enum PawnType
{
    None = 0,
    PlayerPawn,
    ComputePawn
}

/// <summary>
/// 格子数据
/// </summary>
public class GridData
{
    private Vector2Int _coordinate = Vector2Int.zero;
    private GridType _gridType = GridType.Unknown;
    private PawnType _pawnType = PawnType.None;
    private int _weight = 0;
    private int _connectedCount = 0;
    private int _maxSameTypeCountInPipeline = 0;
    private int _maxDiffTypeCountInPipeline = 0;

    public Vector2Int Coordinate
    {
        get => _coordinate;
        set => _coordinate = value;
    }

    public GridType GridType
    {
        get => _gridType;
        set => _gridType = value;
    }

    public PawnType PawnType
    {
        get => _pawnType;
        set => _pawnType = value;
    }

    public int Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public int ConnectedCount
    {
        get => _connectedCount;
        set => _connectedCount = value;
    }

    public int MaxSameTypeCountInPipeline
    {
        get => _maxSameTypeCountInPipeline;
        set => _maxSameTypeCountInPipeline = value;
    }

    public int MaxDiffTypeCountInPipeline
    {
        get => _maxDiffTypeCountInPipeline;
        set => _maxDiffTypeCountInPipeline = value;
    }
}