using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
         Debug.Log("play game");
        //SceneManager.LoadScene(1); // can reference by load index, next in order, or name
    }
    public void QuitGame()
    {
          Debug.Log("quit");
        Application.Quit();
    }
    public void Continue()
    {
        Debug.Log("continue pressed");
        return; // load amnohter scene;
    }
    public void Options()
    {
        Debug.Log("options");

        //SceneManager.LoadScene(); go to settings scene
    }
}
