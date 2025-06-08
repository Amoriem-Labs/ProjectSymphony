using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartyClose : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;

        if (GameStateManager.Instance.freePlay)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ClosePartyScreen()
    {
        if (GameStateManager.Instance.freePlay)
        {
            GameStateManager.Instance.freePlay = false;
        }
        GameStateManager.Instance.LoadNewScene("TitleScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.moveLocalY(gameObject, originalPosition.y - 20f, 0.2f).setEaseOutBack();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.moveLocalY(gameObject, originalPosition.y, 0.2f).setEaseInOutSine();
    }
}
