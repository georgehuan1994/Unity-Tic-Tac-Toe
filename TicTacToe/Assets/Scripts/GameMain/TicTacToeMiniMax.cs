using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public partial class TicTacToe
{
    /// <summary>
    /// 获取最佳落子位置
    /// </summary>
    /// <param name="pawnType"></param>
    /// <returns></returns>
    private static Grid GetBestMove(PawnType pawnType)
    {
        var bestScore = int.MinValue;
        var bestMove = Vector2Int.zero;

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (_boardData[x, y] == PawnType.None)
                {
                    _boardData[x, y] = pawnType;
                    var score = Minimax(_boardData, new Vector2Int(x, y), pawnType, 0, pawnType == PawnType.PlayerPawn);
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

    /// <summary>
    /// 并行获取
    /// </summary>
    /// <param name="pawnType"></param>
    /// <returns></returns>
    private static Grid GetBestMoveParallel(PawnType pawnType)
    {
        Object locker = new Object();
        long bestScore = int.MinValue;
        var bestMove = Vector2Int.zero;
        var boardData = _boardData;
        
        Debug.Log("-----------------");

        var n = BoardSize;

        Parallel.For(0, n*n, i =>
        {
            int x = i % n;
            int y = i / n;
            
            if (boardData[x, y] == PawnType.None)
            {
                boardData[x, y] = pawnType;
                var score = Minimax(boardData, new Vector2Int(x, y), pawnType, 0, pawnType == PawnType.PlayerPawn);
                boardData[x, y] = PawnType.None;

                lock (locker)
                {
                    if (score > bestScore)
                    {
                        Debug.Log($"bestScore: {score} > {bestScore}, Coord: ({x},{y})");
                        bestScore = score;
                        bestMove = new Vector2Int(x, y);
                    }
                }
            }
        });

        return _grids[bestMove.x, bestMove.y];
    }

    /// <summary>
    /// Minimax
    /// </summary>
    /// <param name="boardData"></param>
    /// <param name="coord"></param>
    /// <param name="pawnType"></param>
    /// <param name="depth"></param>
    /// <param name="isMaximizingNext"></param>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <returns></returns>
    private static int Minimax(PawnType[,] boardData, Vector2Int coord, PawnType pawnType, int depth, bool isMaximizingNext, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        var result = WinnerCheck(boardData, coord, pawnType);
        if (result != (int)GameResult.Continue)
        {
            return result;
        }
        
        if (isMaximizingNext) // AI
        {
            var bestScore = int.MinValue;

            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    if (boardData[x, y] == PawnType.None)
                    {
                        boardData[x, y] = PawnType.ComputePawn;
                        var score = Minimax(boardData, new Vector2Int(x, y), PawnType.ComputePawn, depth + 1, false, alpha, beta);
                        boardData[x, y] = PawnType.None;
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

            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    if (boardData[x, y] == PawnType.None)
                    {
                        boardData[x, y] = PawnType.PlayerPawn;
                        var score = Minimax(boardData, new Vector2Int(x, y), PawnType.PlayerPawn, depth + 1, true, alpha, beta);
                        boardData[x, y] = PawnType.None;
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
