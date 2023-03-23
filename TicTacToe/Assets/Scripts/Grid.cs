using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    public GridData GridData = null;
    
    public Transform imageO;
    public Transform imageX;

    public void InitGrid(Vector2Int coord, GridType gridType)
    {
        ResetGrid();
        GridData = new GridData
        {
            Coordinate = coord,
            GridType = gridType,
        };
    }

    private void ResetGrid()
    {
        imageO.gameObject.SetActive(false);
        imageX.gameObject.SetActive(false);
        GridData = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TrySetPlayerPawn();
    }

    public bool TrySetPlayerPawn()
    {
        if (!TicTacToe.Instance.IsPlayerTurn)
        {
            // TODO: 非玩家回合
        }
        
        if (GridData.GridType != GridType.Empty)
        {
            Debug.LogWarning($"Grid {GridData.Coordinate} is {GridData.GridType}");
            return false;
        }
        
        GridData.GridType = GridType.Pawn;
        GridData.PawnType = PawnType.PlayerPawn;
        
        imageO.gameObject.SetActive(true);
        
        TicTacToe.Instance.SetLastPlacedGrid(this);
        TicTacToe.Instance.CheckWinner();
        TicTacToe.Instance.IsPlayerTurn = false;
        return true;
    }

    public bool TrySetComputePawn()
    {
        if (TicTacToe.Instance.IsPlayerTurn)
        {
            // TODO: 非电脑回合
        }
        
        if (GridData.GridType != GridType.Empty)
        {
            Debug.LogWarning($"Grid {GridData.Coordinate} is {GridData.GridType}");
            return false;
        }
        
        GridData.GridType = GridType.Pawn;
        GridData.PawnType = PawnType.ComputePawn;
        
        imageX.gameObject.SetActive(true);
        
        TicTacToe.Instance.SetLastPlacedGrid(this);
        TicTacToe.Instance.CheckWinner();
        TicTacToe.Instance.IsPlayerTurn = true;
        return true;
    }
}
