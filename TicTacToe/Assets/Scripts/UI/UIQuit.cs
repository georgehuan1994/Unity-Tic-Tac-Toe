using System;
using DG.Tweening;
using UnityEngine;

public class UIQuit : MonoBehaviour
{
    public void OnStayButtonClicked()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().DOFade(0, GameConstant.UIFadeDuration)
            .OnComplete(() => { gameObject.SetActive(false); })
            .OnKill(() => gameObject.SetActive(false));
    }

    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetDelay(0.1f).OnKill(() => GetComponent<CanvasGroup>().alpha = 1);
    }
}