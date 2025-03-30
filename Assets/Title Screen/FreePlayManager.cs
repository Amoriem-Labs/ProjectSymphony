using System.Collections;
using System.Collections.Generic;
using RhythMidi;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreePlayManger : MonoBehaviour
{
    public GameObject songButtonPrefab;
    public Transform buttonContainer;

    void Start(){
        List<ChartResource> loadedCharts = RhythMidiController.Instance.GetAllCharts();
        
        foreach(var chart in loadedCharts){
            GameObject button = Instantiate(songButtonPrefab, buttonContainer);
            Debug.Log(chart.Title);
            button.GetComponentInChildren<TextMeshProUGUI>().text = chart.Title;

            string songName = chart.Title;
            button.GetComponent<Button>().onClick.AddListener(() => {
                GameStateManager.Instance.LoadCharacterSelect(songName);
            });
        }
    }
}
