using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public Button StartButton, FreePlayButton, SettingsButton, QuitButton;
    void Start()
    {
        QuitButton.onClick.AddListener(QuitGame);
        FreePlayButton.onClick.AddListener(LoadFreePlay);
        FreePlayButton.onClick.AddListener(() => Debug.Log("Free Play clicked"));
    }

    public void Settings(){
        //TODO: implement this
    }

    public void LoadFreePlay(){
        Debug.Log("Started Free Play");
        SceneManager.LoadScene("FreePlayScene");
    }
    public void StartGame(){
        //TODO: implement this
    }
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Application Quit (doesnt work in unity editor)");
    }
}
