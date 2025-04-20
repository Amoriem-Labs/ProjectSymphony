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
    private AffectionManager affectionManager;
    public GameObject ButtonManager;

    ButtonManager buttonManager;
    bool oldCarter;
    bool oldDaylo;
    bool oldPauline;
    void Start()
    {
        affectionManager = FindAnyObjectByType<AffectionManager>();
        oldCarter = affectionManager.CharacterIsUnlocked("Carter");
        oldDaylo = affectionManager.CharacterIsUnlocked("Daylo");
        oldPauline = affectionManager.CharacterIsUnlocked("Pauline");
        affectionManager.UpdateCharacterUnlock("Carter", true);
        affectionManager.UpdateCharacterUnlock("Daylo", true);
        affectionManager.UpdateCharacterUnlock("Pauline", true);

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
        affectionManager.UpdateCharacterUnlock("Carter", oldCarter);
        affectionManager.UpdateCharacterUnlock("Daylo", oldDaylo);
        affectionManager.UpdateCharacterUnlock("Pauline", oldPauline);
        GameStateManager.Instance.LoadNewScene("TitleScene");
    }
}
