using UnityEngine;

public partial class TicTacToe
{
    /// <summary>
    /// 获取最佳落子位置
    /// </summary>
    /// <param name="pawnType"></param>
    /// <returns></returns>
    private Grid GetBestMove(PawnType pawnType)
    {
        var bestScore = int.MinValue;
        var bestMove = Vector2Int.zero;
        
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (_boardData[x,y] == PawnType.None)
                {
                    _boardData[x, y] = pawnType;
                    var score = Minimax(new Vector2Int(x, y), pawnType, 0, pawnType == PawnType.PlayerPawn);
                    _boardData[x, y] = PawnType.None;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Vector2Int(x, y);
                    }
                }
            }
        }
        
        return _grids[bestMove.x, bestMove.y];
    }

    private int Minimax(Vector2Int coord, PawnType pawnType, int depth, bool isMaximizingNext, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        var result = WinnerCheck(coord, pawnType);
        if (result != (int)GameResult.Continue)
        {
            return result;
        }
        
        if (isMaximizingNext) // AI
        {
            var bestScore = int.MinValue;

            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    if (_boardData[x,y] == PawnType.None)
                    {
                        _boardData[x, y] = PawnType.ComputePawn;
                        var score = Minimax(new Vector2Int(x, y), PawnType.ComputePawn, depth + 1, false, alpha, beta);
                        _boardData[x, y] = PawnType.None;
                        bestScore = Mathf.Max(score, bestScore);
                        
                        alpha = Mathf.Max(alpha, score);
                        if (beta <= alpha)
                        {
                            return bestScore;
                        }
                    }
                }
            }
            return bestScore;
        }
        else // Player
        {
            var bestScore = int.MaxValue;

            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    if (_boardData[x,y] == PawnType.None)
                    {
                        _boardData[x, y] = PawnType.PlayerPawn;
                        var score = Minimax(new Vector2Int(x, y), PawnType.PlayerPawn, depth + 1, true, alpha, beta);
                        _boardData[x, y] = PawnType.None;
                        bestScore = Mathf.Min(score, bestScore);

                        beta = Mathf.Min(beta, score);
                        if (beta <= alpha)
                        {
                            return bestScore;
                        }
                    }
                }
            }
            return bestScore;
        }
    }
}
