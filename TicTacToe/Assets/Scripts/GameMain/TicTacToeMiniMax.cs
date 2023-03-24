using UnityEngine;

public partial class TicTacToe
{
    private static PawnType[,] _boardData;
    
    private Vector2Int _bestMove = Vector2Int.zero;

    private void BestMove()
    {
        // AI: Max Value
        var bestScore = int.MinValue;
        var bestMove = Vector2Int.zero;
        
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (_boardData[x,y] == PawnType.Unknown)
                {
                    _boardData[x, y] = PawnType.ComputePawn;
                    var score = Minimax(new Vector2Int(x, y), _boardData[x, y], 0, false);
                    _boardData[x, y] = PawnType.Unknown;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        _bestMove = bestMove = new Vector2Int(x, y);
                    }
                }
            }
        }

        _boardData[bestMove.x, bestMove.y] = PawnType.ComputePawn;
        
        // Turn
    }

    private int Minimax(Vector2Int coord, PawnType pawnType, int depth, bool isMaximizingNext)
    {
        var score = CheckWinner(coord, pawnType);
        if (score < 999)
        {
            return score;
        }
        
        if (isMaximizingNext)
        {
            for (int y = 0; y < boardSize; y++)
            {
                for (int x = 0; x < boardSize; x++)
                {
                    
                }
            }
        }
        return 1;
    }

    

    private Grid GetBestGrid()
    {
        BestMove();
        return _grids[_bestMove.x, _bestMove.y];
    }
    
    public void SetBoardDate(Vector2Int coord, PawnType pawnType)
    {
        _boardData[coord.x, coord.y] = pawnType;
    }
}
