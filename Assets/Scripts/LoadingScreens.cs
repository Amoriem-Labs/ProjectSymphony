using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreens : MonoBehaviour
{
    public bool loaded;
    private GameStateManager GSM;
    // Start is called before the first frame update
    void Start()
    {
        GSM = FindAnyObjectByType<GameStateManager>();
        loaded = true;
    }

    private void Update()
    {
        if (!loaded)
        {
            Debug.Log("he222re");

            int SceneToLoad = PlayerPrefs.GetInt("SceneToLoad");
            switch (SceneToLoad)
            {
                case 0:
                    Debug.Log("here");
                    // Load VN
                    new WaitForSeconds(3f);
                    GSM.LoadVN();

                    break;
                case 1:
                    // Load Map

                    break;
                case 2:
                    // Load Rhythm 

                    break;
                case 3:
                    SceneManager.LoadScene("MapScreen");
                    break;
                default:
                    Debug.LogError($"Scene to load recieved an unexpected value: {SceneToLoad}. Expected int 0,1, or 2.");
                    break;
               
            }

            loaded = true;
        }
    }
}
