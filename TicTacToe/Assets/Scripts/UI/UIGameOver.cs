using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGameOver : MonoBehaviour, IPointerClickHandler
{
    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }
}
