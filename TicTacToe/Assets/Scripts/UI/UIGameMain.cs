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

    private void Start()
    {
        TicTacToe.Instance.OnGameStart += OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced += OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart += OnRoundStartEventHandler;
        TicTacToe.Instance.OnGameOver += OnGameOverEventHandler;
        
        restartButton.onClick.AddListener(TicTacToe.Instance.InitChessboard);
    }

    private void OnDestroy()
    {
        TicTacToe.Instance.OnGameStart -= OnGameStartEventHandler;
        TicTacToe.Instance.OnPawnPlaced -= OnPawnPlacedEventHandler;
        TicTacToe.Instance.OnRoundStart -= OnRoundStartEventHandler;
        TicTacToe.Instance.OnGameOver -= OnGameOverEventHandler;
        
        restartButton.onClick.RemoveListener(TicTacToe.Instance.InitChessboard);
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
    
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
