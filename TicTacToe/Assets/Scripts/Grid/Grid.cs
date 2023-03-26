using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    public GridData GridData = null;

    [SerializeField]
    private Transform imageO;
    
    [SerializeField]
    private Transform imageX;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="gridType"></param>
    public void Init(Vector2Int coord, GridType gridType)
    {
        Reset();
        GridData = new GridData
        {
            Coordinate = coord,
            GridType = gridType,
        };
    }

    /// <summary>
    /// 重置
    /// </summary>
    private void Reset()
    {
        GetComponent<Image>().color = GameConstant.GridColor;
        imageO.gameObject.SetActive(false);
        imageX.gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        GridData = null;
    }

    /// <summary>
    /// 显示
    /// </summary>
    public void ShowGrid()
    {
        transform.DOScale(Vector3.one, GameConstant.GridInitShowDuration).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 平局高亮
    /// </summary>
    public void TiesHighlight()
    {
        GetComponent<Image>().DOColor(GameConstant.GridTiesColor, GameConstant.GridCompletedHighlightDuration);
    }

    /// <summary>
    /// 完成高亮
    /// </summary>
    public void CompletedHighlight()
    {
        GetComponent<Image>().DOColor(GameConstant.GridHighlightColor, GameConstant.GridCompletedHighlightDuration);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        TrySetPlayerPawn();
    }

    /// <summary>
    /// 尝试放置玩家棋子
    /// </summary>
    /// <returns></returns>
    public bool TrySetPlayerPawn()
    {
        if (!TicTacToe.IsPlayerTurn)
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

        TicTacToe.Instance.LastPlacedGrid = this;

        float timer = 0;
        DOTween.To(() => timer, a => timer = a, 1f, 0.7f).OnComplete(() =>
        {
            TicTacToe.Instance.AIMove();
        });
        
        return true;
    }

    /// <summary>
    /// 尝试放置电脑棋子
    /// </summary>
    /// <returns></returns>
    public bool TrySetComputePawn()
    {
        if (TicTacToe.IsPlayerTurn)
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

        TicTacToe.Instance.LastPlacedGrid = this;
        return true;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
}