using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Text resultText;
    
    [SerializeField]
    private Image playerPawn;
    
    [SerializeField]
    private Image computePawn;

    private void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    private void Start()
    {
        TicTacToe.Instance.OnGameOver += Show;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TicTacToe.Instance.OnGameOver -= Show;
    }

    public void Show(GameResult gameResult)
    {
        switch (gameResult)
        {
            case GameResult.Draw:
                playerPawn.gameObject.SetActive(true);
                computePawn.gameObject.SetActive(true);
                resultText.text = "Draw";
                break;
            case GameResult.PlayerWin:
                playerPawn.gameObject.SetActive(true);
                computePawn.gameObject.SetActive(false);
                resultText.text = "You Win!";
                break;
            case GameResult.ComputeWin:
                playerPawn.gameObject.SetActive(false);
                computePawn.gameObject.SetActive(true);
                resultText.text = "You Lose!";
                break;
        }
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetDelay(1f);
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            TicTacToe.Instance.InitChessboard();
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
}
