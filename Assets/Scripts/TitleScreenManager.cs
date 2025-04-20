using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public Canvas LoadingScreen;
    public Button StartButton, LoadButton, FreePlayButton, SettingsButton, QuitButton;
    private LoadingScreens LS;
    void Start()
    {
        LoadingScreen.enabled = false;
        LS = FindAnyObjectByType<LoadingScreens>();
        QuitButton.onClick.AddListener(QuitGame);
        LoadButton.onClick.AddListener(LoadGame);
        FreePlayButton.onClick.AddListener(LoadFreePlay);
        StartButton.onClick.AddListener(StartGame);
    }

    public void Settings()
    {
        //TODO: implement this
    }

    public void LoadFreePlay()
    {
        GameStateManager.Instance.LoadNewScene("FreePlayScene");
        //SceneManager.LoadScene("FreePlayScene");
    }
    public void StartGame()
    {
        PlayerPrefs.SetInt("SceneIndex.", 0);
        PlayerPrefs.SetInt("SceneToLoad", 0);

        LoadingScreen.enabled = true;
        LS.loaded = false;
    }
    public void LoadGame()
    {
        GameStateManager.Instance.LoadNewScene("SaveScreen");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application Quit (doesnt work in unity editor)");
    }
}
