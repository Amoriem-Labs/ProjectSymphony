using System.Collections;
using System.Collections.Generic;
using RhythMidi;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FreePlayManger : MonoBehaviour
{
    public GameObject songButtonPrefab;
    public Transform buttonContainer;

    public GameObject ButtonManager;

    ButtonManager buttonManager;

    void Start()
    {
        GameStateManager.Instance.freePlay = true;
        List<ChartResource> loadedCharts = RhythMidiController.Instance.GetAllCharts();
        buttonManager = ButtonManager.GetComponent<ButtonManager>();

        foreach (var chart in loadedCharts)
        {
            GameObject button = Instantiate(songButtonPrefab, buttonContainer);

            Debug.Log(chart.Title);
            button.GetComponentInChildren<TextMeshProUGUI>().text = chart.Title;

            string songName = chart.Title;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameStateManager.Instance.LoadCharacterSelect(songName);
            });

            // Add ButtonHover sound on click
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                buttonManager.MapPullUp();
            });

            // Add EventTrigger for pointer enter (hover)
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((eventData) =>
            {
                buttonManager.ButtonHover();
            });
            trigger.triggers.Add(entry);
        }
    }
    public void ExitScreen()
    {
        GameStateManager.Instance.freePlay = false;
        GameStateManager.Instance.LoadNewScene("TitleScene");
    }
}
