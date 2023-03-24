using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMain : MonoBehaviour
{
    [SerializeField]
    private Text startTipsText;

    [SerializeField]
    private Image playerPawn;
    
    [SerializeField]
    private Image computePawn;

    private static readonly Color PlayerThemeColor = new Color(0.78f, 1, 0.42f, 1);
    private static readonly Color ComputeThemeColor = new Color(1, 0.23f, 0.23f, 1);

    private void Start()
    {
        TicTacToe.Instance.OnGameStart += OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced += OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart += OnRoundStartEventHandler;
    }

    private void OnDestroy()
    {
        TicTacToe.Instance.OnGameStart -= OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced -= OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart += OnRoundStartEventHandler;
    }

    private void OnPawnPlacedEventHandler()
    {
        startTipsText.gameObject.SetActive(false);
    }

    private void OnGameStartEventHandler()
    {
        startTipsText.gameObject.SetActive(true);
    }

    private void OnRoundStartEventHandler(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            playerPawn.color = PlayerThemeColor;
            playerPawn.transform.GetChild(0).GetComponent<Text>().color = PlayerThemeColor;
            computePawn.color = Color.white;
            computePawn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            
            playerPawn.transform.localScale = Vector3.one * 1.25f;
            playerPawn.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            playerPawn.color = Color.white;
            playerPawn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            computePawn.color = ComputeThemeColor;
            computePawn.transform.GetChild(0).GetComponent<Text>().color = ComputeThemeColor;
            
            // computePawn.transform.localScale = Vector3.one * 1.25f;
            // computePawn.transform.DOScale(Vector3.one, 0.5f);
        }
    }
}
