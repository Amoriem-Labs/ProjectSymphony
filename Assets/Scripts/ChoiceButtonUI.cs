using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChoiceButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image background;
    // Start is called before the first frame update
    public Vector3 position;
    void Start()
    {
        position = transform.position;
        background.gameObject.SetActive(false);

    }
    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBigger();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverSmaller();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Your click logic here
        hoverSmaller();
    }

    public void hoverBigger()
    {

        Debug.Log("hover bigger");
        background.gameObject.SetActive(true);
        LeanTween.moveX(gameObject, position.x - 1f, 0.4f)
    .setEase(LeanTweenType.easeOutBack);
    }
    public void hoverSmaller()
    {
        Debug.Log("hover smaller");
        background.gameObject.SetActive(false);
        LeanTween.moveX(gameObject, position.x, 0.4f)
    .setEase(LeanTweenType.easeOutBack);
    }
}
