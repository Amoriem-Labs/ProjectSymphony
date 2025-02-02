using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public Transform box; // object to be animated
    public CanvasGroup background;

     
    // Start is called before the first frame update

    private void OnEnable() // once enabled 
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f); // gradually transitions transparency; value, time

        box.localPosition = new Vector2(0, -Screen.height); // start below the screen
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f; // bring it up quickly
    }
    public void CloseDialog()
    {
        background.LeanAlpha(0, 0.5f); //
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
        // call the OnComplete method on completion 
    }
    public void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
