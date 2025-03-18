using System.Collections;
using System.Collections.Generic;
using RhythMidi;
using UnityEngine;

public class Splash : MonoBehaviour
{
    void Start()
    {
        RhythMidiController.Instance.onFinishedLoading.AddListener(OnRhythMidiLoaded);
    }

    void OnRhythMidiLoaded()
    {
        GameStateManager.Instance.LoadCharacterSelect("Week 2");
    }
}
