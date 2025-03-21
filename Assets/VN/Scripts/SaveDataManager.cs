using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveDataManager : MonoBehaviour
{
    private DialougeManager dialogueManager;
    private MapManager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialougeManager>();
        mapManager = FindAnyObjectByType<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
