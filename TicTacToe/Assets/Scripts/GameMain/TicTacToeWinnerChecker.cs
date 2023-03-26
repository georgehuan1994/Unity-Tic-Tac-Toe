using UnityEngine;

public partial class TicTacToe
{
    /// <summary>
    /// 对局结束检查
    /// </summary>
    private void GameOverCheck(PawnType[,] boardData, Vector2Int coord, PawnType pawnType)
    {
        _boardData[coord.x, coord.y] = pawnType;
        GameOver(CheckDiagonal(boardData, coord, pawnType), CompletedLayout.Diagonal);
        GameOver(CheckInvDiagonal(boardData, coord, pawnType), CompletedLayout.InvDiagonal);
        GameOver(CheckRow(boardData, coord, pawnType), CompletedLayout.Row);
        GameOver(CheckColumn(boardData, coord, pawnType), CompletedLayout.Column);
        GameOver(CheckTie(boardData), CompletedLayout.Full);
    }

    /// <summary>
    /// 胜平负检查
    /// </summary>
    /// <param name="boardDataCop">棋子数据拷贝</param>
    /// <param name="coord">棋子坐标</param>
    /// <param name="pawnType">棋子类型</param>
    /// <returns></returns>
    private static int WinnerCheck(PawnType[,] boardDataCop, Vector2Int coord, PawnType pawnType)
    {
        var result = CheckDiagonal(boardDataCop, coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckInvDiagonal(boardDataCop, coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckRow(boardDataCop, coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckColumn(boardDataCop, coord, pawnType);
        if (result != GameResult.Continue) return (int)result;
        
        return (int)CheckTie(boardDataCop);
    }
    
    //////////////////////////////////////////////////////////

    /// <summary>
    /// 检查对角线
    /// </summary>
    private static GameResult CheckDiagonal(PawnType[,] boardData, Vector2Int coord, PawnType pawnType)
    {
        if (coord.x == coord.y)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (pawnType != boardData[i, i]) return GameResult.Continue;
            }

            return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
        }

        return GameResult.Continue;
    }

    /// <summary>
    /// 检查逆对角线
    /// </summary>
    private static GameResult CheckInvDiagonal(PawnType[,] boardData, Vector2Int coord, PawnType pawnType)
    {
        if (coord.x + coord.y == BoardSize - 1)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (pawnType != boardData[i, BoardSize - i - 1]) return GameResult.Continue;
            }
            
            return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
        }
        
        return GameResult.Continue;
    }

    /// <summary>
    /// 检查当前列
    /// </summary>
    private static GameResult CheckRow(PawnType[,] boardData, Vector2Int coord, PawnType pawnType)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            if (pawnType != boardData[coord.x, i]) return GameResult.Continue;
        }

        return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
    }

    /// <summary>
    /// 检查当前行
    /// </summary>
    private static GameResult CheckColumn(PawnType[,] boardData, Vector2Int coord, PawnType pawnType)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            if (pawnType != boardData[i, coord.y]) return GameResult.Continue;
        }

        return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
    }

    /// <summary>
    /// 检查和棋
    /// </summary>
    private static GameResult CheckTie(PawnType[,] boardData)
    {
        foreach (var pawnType in boardData)
        {
            if (pawnType == PawnType.None)
            {
                return GameResult.Continue;
            }
        }
        return GameResult.Tie;
    }
}
