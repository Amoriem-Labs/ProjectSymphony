using System.Collections;
using System.Collections.Generic;
using RhythMidi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    void Start()
    {
        RhythMidiController.Instance.onFinishedLoading.AddListener(OnRhythMidiLoaded);
    }

    void OnRhythMidiLoaded()
    {
        GameStateManager.Instance.LoadNewScene("TitleScene");
    }
}
