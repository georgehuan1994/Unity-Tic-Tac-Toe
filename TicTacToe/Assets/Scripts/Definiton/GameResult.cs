/// <summary>
/// 游戏结果
/// </summary>
public enum GameResult
{
    Tie = 0,
    PlayerWin = -1,
    ComputeWin = 1,
    Continue = 999,
}

/// <summary>
/// 完成布局
/// </summary>
public enum CompletedLayout
{
    Full,
    Diagonal,
    InvDiagonal,
    Row,
    Column
}