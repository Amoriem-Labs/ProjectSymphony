using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Dialouge
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
    public string sprite;
    public string[] options;
    public int[] nextDialogueIndeces;
    public string background;
    public string sfx;
    public string bgm;
    public string bgm2;
}

