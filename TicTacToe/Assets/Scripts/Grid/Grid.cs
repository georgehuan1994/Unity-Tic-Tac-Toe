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
            return false;
        }

        if (GridData.GridType != GridType.Empty)
        {
            Debug.LogWarning($"Grid {GridData.Coordinate} is {GridData.GridType}");
            return false;
        }
        
        imageO.transform.localScale = Vector3.one * 1.25f;
        imageO.transform.DOScale(Vector3.one, 0.3f);
        imageO.gameObject.SetActive(true);

        GridData.GridType = GridType.Pawn;
        GridData.PawnType = PawnType.PlayerPawn;

        TicTacToe.Instance.SetLastPlacedGrid(this);
        TicTacToe.Instance.CheckWinner();
        TicTacToe.Instance.OnPawnPlaced.Invoke();

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
            return false;
        }

        if (GridData.GridType != GridType.Empty)
        {
            Debug.LogWarning($"Grid {GridData.Coordinate} is {GridData.GridType}");
            return false;
        }
        
        imageX.transform.localScale = Vector3.one * 1.25f;
        imageX.transform.DOScale(Vector3.one, 0.3f);
        imageX.gameObject.SetActive(true);

        GridData.GridType = GridType.Pawn;
        GridData.PawnType = PawnType.ComputePawn;

        TicTacToe.Instance.SetLastPlacedGrid(this);
        TicTacToe.Instance.CheckWinner();
        TicTacToe.Instance.OnPawnPlaced.Invoke();
        return true;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
}