using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialougeTrigger : MonoBehaviour
{
    public Dialouge dialouge;
    public Button continuebutton;
    public TextMeshProUGUI dialougeBox;
    public TextMeshProUGUI nameText;

    private DialougeManager dialougeManager;

    private void Start()
    {
        dialougeManager = FindObjectOfType<DialougeManager>();
    }

    public void TriggerDialouge()
    {
        if(dialougeManager == null)
        {
            Debug.LogError("Dialogue Manager not found!");
            return;
        }
        continuebutton.gameObject.SetActive(true);
        dialougeBox.enabled = true;
        nameText.enabled = true;


        dialougeManager.StartDialouge(dialouge);
    }
}
