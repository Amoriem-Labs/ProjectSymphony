using System.Collections;
using System.Collections.Generic;
using RhythMidi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    void Start()
    {
        int SceneToLoad = PlayerPrefs.GetInt("SceneToLoad");
        switch (SceneToLoad)
        {
            case 0:
                // Load VN
                new WaitForSeconds(3f);
                GameStateManager.Instance.LoadVN();

                break;
            case 1:
                // Load Map

                break;
            case 2:
                // Load Rhythm 
                RhythMidiController.Instance.onFinishedLoading.AddListener(OnRhythMidiLoaded);
                break;
            case 3:
                SceneManager.LoadScene("MapScreen");
                break;
            default:
                Debug.LogError($"Scene to load recieved an unexpected value: {SceneToLoad}. Expected int 0,1, or 2.");
                break;
        }


    }

    void OnRhythMidiLoaded()
    {
        GameStateManager.Instance.LoadCharacterSelect("Week 2");
    }
}
