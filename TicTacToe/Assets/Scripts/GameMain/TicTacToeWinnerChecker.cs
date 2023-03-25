using UnityEngine;

public partial class TicTacToe
{
    /// <summary>
    /// 检查获胜者
    /// </summary>
    public void CheckWinnerForInput(Vector2Int coord, PawnType pawnType)
    {
        _boardData[coord.x, coord.y] = pawnType;

        GameOver(CheckDiagonal(coord, pawnType), CompletedLayout.Diagonal);
        GameOver(CheckInvDiagonal(coord, pawnType), CompletedLayout.InvDiagonal);
        GameOver(CheckRow(coord, pawnType), CompletedLayout.Row);
        GameOver(CheckColumn(coord, pawnType), CompletedLayout.Column);
        GameOver(CheckTie(), CompletedLayout.Full);

        if (!_isGameOver)
        {
            IsPlayerTurn = !IsPlayerTurn;
            OnRoundStart.Invoke(IsPlayerTurn);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="pawnType"></param>
    /// <returns></returns>
    private int CheckWinner(Vector2Int coord, PawnType pawnType)
    {
        var result = CheckDiagonal(coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckInvDiagonal(coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckRow(coord, pawnType);
        if (result != GameResult.Continue) return (int)result;

        result = CheckColumn(coord, pawnType);
        if (result != GameResult.Continue) return (int)result;
        
        return (int)CheckTie();
    }

    /// <summary>
    /// 检查对角线
    /// </summary>
    private GameResult CheckDiagonal(Vector2Int coord, PawnType pawnType)
    {
        if (coord.x == coord.y)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (pawnType != _boardData[i, i]) return GameResult.Continue;
            }

            return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
        }

        return GameResult.Continue;
    }

    /// <summary>
    /// 检查逆对角线
    /// </summary>
    private GameResult CheckInvDiagonal(Vector2Int coord, PawnType pawnType)
    {
        if (coord.x + coord.y == boardSize - 1)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (pawnType != _boardData[i, boardSize - i - 1]) return GameResult.Continue;
            }
            
            return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
        }
        
        return GameResult.Continue;
    }

    /// <summary>
    /// 检查当前列
    /// </summary>
    private GameResult CheckRow(Vector2Int coord, PawnType pawnType)
    {
        for (int i = 0; i < boardSize; i++)
        {
            if (pawnType != _boardData[coord.x, i]) return GameResult.Continue;
        }

        return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
    }

    /// <summary>
    /// 检查当前行
    /// </summary>
    private GameResult CheckColumn(Vector2Int coord, PawnType pawnType)
    {
        for (int i = 0; i < boardSize; i++)
        {
            if (pawnType != _boardData[i, coord.y]) return GameResult.Continue;
        }

        return pawnType == PawnType.PlayerPawn ? GameResult.PlayerWin : GameResult.ComputeWin;
    }

    /// <summary>
    /// 检查和棋
    /// </summary>
    private GameResult CheckTie()
    {
        foreach (var pawnType in _boardData)
        {
            if (pawnType == PawnType.None)
            {
                return GameResult.Continue;
            }
        }
        return GameResult.Tie;
    }
}
