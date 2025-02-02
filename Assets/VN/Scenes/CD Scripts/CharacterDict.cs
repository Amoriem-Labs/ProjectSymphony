using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterDict : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public TextMeshProUGUI CharName;
    public TextMeshProUGUI CharBio;
    public Image CharSkill1;
    public Image CharSkill2;
    public Image CharSkill3;
    public Image CharSkill4;
    public Image CharSkill5;
    public Image CharSkill6;
    public Image CharIm;
    public Button forward;
    public Button back;
    public Button on;
    private int currPageNum = 0;
    private int characterNum = 5;
    public Sprite Carter;
    //create dictionary array type
    private Dictionary<string, object>[] Dict;
    void Start()
    {
        canvas.enabled = false;
        //Sprite Carter = Resources.Load<Sprite>("CarterTemp.png");
        //CharIm = GetComponent<Image>();
        //initialize dictionarry array
        Dict = new Dictionary<string, object>[characterNum];

        //fill in dictionary entries
        for(int i = 0; i < Dict.Length; i++)
        {
            Dict[i] = new Dictionary<string, object>();
            if(i > 0)
            {
                Dict[i].Add("name", "Character " + i);
                Dict[i].Add("bio", "Bio..." + i);
                Dict[i].Add("Percussion", Random.Range(0,6));
                Dict[i].Add("Melody", Random.Range(0, 6));
                Dict[i].Add("Harmony", Random.Range(0, 6));
                Dict[i].Add("Counter-Melody", Random.Range(0, 6));
                Dict[i].Add("Affection", Random.Range(0, 6));
                Dict[i].Add("Popularity", Random.Range(0, 6));
            }
            

        }

        Dict[0].Add("name", "Carter");
        Dict[0].Add("bio", "Coming from a world with eldritch horrors, Carter seems pretty relaxed. " +
                    "Math homework is easy when you don’t have to worry about monsters. He seems like " +
                    "he's half convinced he’s in the middle of a dream.");
        Dict[0].Add("Percussion", 1);
        Dict[0].Add("Melody", 1);
        Dict[0].Add("Harmony", 2);
        Dict[0].Add("Counter-Melody", 4);
        Dict[0].Add("Affection", 3);
        Dict[0].Add("Popularity", 5);
        forward.onClick.AddListener(ForwardClicked);
        back.onClick.AddListener(BackClicked);
        UpdateDict();
        on.onClick.AddListener(OnClicked);

    }
    // Update is called once per frame
    void Update()
    {
       
        //UpdateDict();
    }

    private void ForwardClicked()
    {
        currPageNum += 1;
        if (currPageNum == characterNum)
        {
            currPageNum = 0;
        }
        UpdateDict();
    }

    private void BackClicked()
    {
        
        currPageNum -= 1;
        if (currPageNum == -1)
        {
            currPageNum = characterNum -1;
        }
            UpdateDict();
        

    }

    private void UpdateDict()
    {
        Debug.Log("Index is: " + currPageNum);
        CharName.text = Dict[currPageNum]["name"].ToString();
        CharBio.text = Dict[currPageNum]["bio"].ToString();
        float fill1 = (float)((int)Dict[currPageNum]["Percussion"]) / 5f;
        float fill2 = (float)((int)Dict[currPageNum]["Melody"]) / 5f;
        float fill3 = (float)((int)Dict[currPageNum]["Harmony"]) / 5f;
        float fill4 = (float)((int)Dict[currPageNum]["Counter-Melody"]) / 5f;
        float fill5 = (float)((int)Dict[currPageNum]["Affection"]) / 5f;
        float fill6 = (float)((int)Dict[currPageNum]["Popularity"]) / 5f;
        CharSkill1.fillAmount = fill1;
        CharSkill2.fillAmount = fill2;
        CharSkill3.fillAmount = fill3;
        CharSkill4.fillAmount = fill4;
        CharSkill5.fillAmount = fill5;
        CharSkill6.fillAmount = fill6;
        if (Dict[currPageNum]["name"].ToString() == "Carter")
        {
            Debug.Log("Image is Carter!");
            CharIm.sprite = Carter;
        }
        else
        {
            CharIm.sprite = null;
            Debug.Log("Image is null!");

        }

    }
    private void OnClicked()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
    }

}
