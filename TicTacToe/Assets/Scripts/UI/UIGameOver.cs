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
    private Text restartTipText;
    
    [SerializeField]
    private Image playerPawn;
    
    [SerializeField]
    private Image computePawn;

    private void Start()
    {
        TicTacToe.Instance.OnGameOver += Show;
    }

    private void OnDestroy()
    {
        TicTacToe.Instance.OnGameOver -= Show;
    }

    public void Show(GameResult gameResult)
    {
        switch (gameResult)
        {
            case GameResult.Tie:
                playerPawn.gameObject.SetActive(true);
                computePawn.gameObject.SetActive(true);
                resultText.text = "TIE";
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
        GetComponent<CanvasGroup>().DOFade(1, GameConstant.UIFadeDuration).SetDelay(1f);
        
        restartTipText.GetComponent<Animation>().Stop();
        restartTipText.color = Color.white;
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().DOFade(0, 0.8f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            TicTacToe.Instance.InitChessboard();
        }).SetEase(Ease.OutCubic);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
}
