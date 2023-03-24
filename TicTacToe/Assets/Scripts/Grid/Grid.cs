using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    public GridData GridData = null;

    public Transform imageO;
    public Transform imageX;

    public Text debugTextConnectedCount;

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
        debugTextConnectedCount.gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        GridData = null;
    }

    public void ShowGrid()
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
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
            return false;
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
        
        float timer = 0;
        DOTween.To(() => timer, a => timer = a, 1f, 0.7f).OnComplete(() =>
        {
            TicTacToe.Instance.AIMove();
        });
        
        return true;
    }

    public bool TrySetComputePawn()
    {
        if (TicTacToe.Instance.IsPlayerTurn)
        {
            // TODO: 非电脑回合
            return false;
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

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
}