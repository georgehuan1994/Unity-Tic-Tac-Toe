using System;
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

    [SerializeField] 
    private Button restartButton;

    [SerializeField] 
    private Button closeButton;

    [SerializeField]
    private Text difficultyText;
    
    private void Start()
    {
        TicTacToe.Instance.OnGameStart += OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced += OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart += OnRoundStartEventHandler;
        TicTacToe.Instance.OnGameOver += OnGameOverEventHandler;
        
        restartButton.onClick.AddListener(TicTacToe.Instance.InitChessboard);
        closeButton.onClick.AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        TicTacToe.Instance.OnGameStart -= OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced -= OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart -= OnRoundStartEventHandler;
        TicTacToe.Instance.OnGameOver -= OnGameOverEventHandler;
        
        restartButton.onClick.RemoveListener(TicTacToe.Instance.InitChessboard);
        closeButton.onClick.RemoveListener(QuitGame);
    }

    private void OnPawnPlacedEventHandler()
    {
        HideStartTipText();
        GetComponent<AudioSource>().Play();
    }

    private void OnGameStartEventHandler()
    {
        Invoke(nameof(AddDifficulty), 0.5f);
        Invoke(nameof(ShowStartTipText), 0.6f);
        
        var layoutGroup = transform.Find("Panel_Chessboard").GetComponent<GridLayoutGroup>();
        if (TicTacToe.BoardSize == 3)
        {
            layoutGroup.cellSize = Vector2.one * GameConstant.GridCellSize3X3;
            layoutGroup.spacing = Vector2.one * GameConstant.GridCellSpacing3X3;
        }
        if (TicTacToe.BoardSize == 4)
        {
            layoutGroup.cellSize = Vector2.one * GameConstant.GridCellSize4X4;
            layoutGroup.spacing = Vector2.one * GameConstant.GridCellSpacing4X4;
        }
    }

    private void OnRoundStartEventHandler(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            playerPawn.color = GameConstant.PlayerThemeColor;
            playerPawn.transform.GetChild(0).GetComponent<Text>().color = GameConstant.PlayerThemeColor;
            computePawn.color = Color.white;
            computePawn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            
            playerPawn.transform.localScale = Vector3.one * 1.25f;
            playerPawn.transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            playerPawn.color = Color.white;
            playerPawn.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            computePawn.color = GameConstant.ComputeThemeColor;
            computePawn.transform.GetChild(0).GetComponent<Text>().color = GameConstant.ComputeThemeColor;
        }
    }

    private void OnGameOverEventHandler(GameResult result)
    {
        
    }

    private void ShowStartTipText()
    {
        startTipsText.color = new Color(1, 1, 1, 0);
        startTipsText.gameObject.SetActive(true);
        startTipsText.DOFade(1, 0.2f);
    }

    private void HideStartTipText()
    {
        if (startTipsText.gameObject.activeInHierarchy)
        {
            startTipsText.DOFade(0, 0.2f).OnComplete(() =>
            {
                startTipsText.gameObject.SetActive(false);
            });
        }
    }

    private void AddDifficulty()
    {
        Debug.Log($"Difficulty: {TicTacToe.GameDifficulty}");
        
        var diffGroup = transform.Find("DiffGroup");
        var easy = diffGroup.GetChild(1).GetChild(1);
        var mid = diffGroup.GetChild(2).GetChild(1);
        var hard = diffGroup.GetChild(3).GetChild(1);
        
        switch (TicTacToe.GameDifficulty)
        {
            case GameDifficulty.Easy:
                difficultyText.text = "EASY";
                easy.gameObject.SetActive(true);
                break;
            case GameDifficulty.Mid:
                difficultyText.text = "MID";
                easy.gameObject.SetActive(true);
                mid.gameObject.SetActive(true);
                break;
            case GameDifficulty.Hard:
                difficultyText.text = "HARD";
                easy.gameObject.SetActive(true);
                mid.gameObject.SetActive(true);
                hard.gameObject.SetActive(true);
                break;
        }
    }

    private void QuitGame()
    {
        transform.parent.Find("UI_Quit").gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            QuitGame();
        }
    }
}
