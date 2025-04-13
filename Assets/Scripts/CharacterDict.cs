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
    // public TextMeshProUGUI CharName;
    // public TextMeshProUGUI CharBio;
    // public Image CharSkill1;
    // public Image CharSkill2;
    // public Image CharSkill3;
    // public Image CharSkill4;
    // public Image CharSkill5;
    //public Image CharSkill6;
    //public Image CharIm;

    public Image background;
    CanvasGroup backgroundGroup;
    public Transform left;
    public Transform right;
    public GameObject profile;

    Image profileImg;
    public GameObject bio;

    public GameObject nameText;
    public GameObject bioText;
    public GameObject instrumentText;
    public GameObject roleText;

    public Button forward;
    public Button back;
    public Button on;
    public Button close;
    private int currPageNum = 0;
    private int characterNum = 0;
    //public Sprite Carter;
    //create dictionary array type
    private Dictionary<string, object>[] Dict;

    public string[] characterNames;
    void Start()
    {
        profileImg = profile.GetComponent<Image>();
        backgroundGroup = background.GetComponent<CanvasGroup>();
        canvas.enabled = false;
        //Sprite Carter = Resources.Load<Sprite>("CarterTemp.png");
        //CharIm = GetComponent<Image>();
        //initialize dictionarry array
        Dict = new Dictionary<string, object>[characterNum];

        // //fill in dictionary entries
        // for(int i = 0; i < Dict.Length; i++)
        // {
        //     Dict[i] = new Dictionary<string, object>();
        //     if(i > 0)
        //     {
        //         Dict[i].Add("name", "Character " + i);
        //         Dict[i].Add("bio", "Bio..." + i);
        //         Dict[i].Add("Percussion", Random.Range(0,6));
        //         Dict[i].Add("Melody", Random.Range(0, 6));
        //         Dict[i].Add("Harmony", Random.Range(0, 6));
        //         Dict[i].Add("Counter-Melody", Random.Range(0, 6));
        //         Dict[i].Add("Affection", Random.Range(0, 6));
        //         Dict[i].Add("Popularity", Random.Range(0, 6));
        //     }


        // }

        // Dict[0].Add("name", "Carter");
        // Dict[0].Add("bio", "Coming from a world with eldritch horrors, Carter seems pretty relaxed. " +
        //             "Math homework is easy when you don’t have to worry about monsters. He seems like " +
        //             "he's half convinced he’s in the middle of a dream.");
        // Dict[0].Add("Percussion", 1);
        // Dict[0].Add("Melody", 1);
        // Dict[0].Add("Harmony", 2);
        // Dict[0].Add("Counter-Melody", 4);
        // Dict[0].Add("Affection", 3);
        // Dict[0].Add("Popularity", 5);

        forward.onClick.AddListener(ForwardClicked);
        back.onClick.AddListener(BackClicked);
        UpdateDict();
        on.onClick.AddListener(OnClicked);
        close.onClick.AddListener(OnClicked);



    }
    // Update is called once per frame
    void Update()
    {

        //UpdateDict();
    }

    private void ForwardClicked()
    {
        Debug.Log("move foward one");
        currPageNum += 1;

        UpdateDict();
    }

    private void BackClicked()
    {
        Debug.Log("move back one");
        currPageNum -= 1;
        if (currPageNum == -1)
        {
            currPageNum = characterNum - 1;
        }
        UpdateDict();


    }



    private void UpdateDict()
    {
        if (characterNames == null || characterNames.Length == 0)
        {
            Debug.LogWarning("Character names array is empty.");
            return;
        }

        if (currPageNum >= characterNames.Length)
        {
            currPageNum = 0;
        }

        string charName = characterNames[currPageNum];

        CharacterData cd = null;

        // Find the character by name
        foreach (var kvp in GameStateManager.Instance.characterData)
        {
            if (kvp.Key.name == charName)
            {
                cd = kvp.Value;
                break;
            }
        }

        if (cd == null)
        {
            Debug.LogWarning($"No character data found for name: {charName}");
            return;
        }

        Character character = cd.character;

        // Set visuals
        profileImg.sprite = character.backgroundHero;

        bioText.GetComponent<TextMeshPro>();

        // Set text fields
        nameText.GetComponent<TextMeshProUGUI>().text = character.name;
        bioText.GetComponent<TextMeshProUGUI>().text = character.bio;
        instrumentText.GetComponent<TextMeshProUGUI>().text = character.instrument;
        roleText.GetComponent<TextMeshProUGUI>().text = character.role.ToString();
    }




    private void OnClicked()
    {

        if (canvas.enabled)
        {
            Debug.Log("Clicked off");
            StartCoroutine(ReverseAnimateSequence());
            //canvas.enabled = false;
        }
        else
        {
            left.localPosition = new Vector2(-Screen.width - 500, -70);
            right.localPosition = new Vector2(Screen.width + 500, -36);
            StartCoroutine(AnimateSequence());


        }
    }

    private IEnumerator AnimateSequence()
    {
        canvas.enabled = true;
        Debug.Log("Clicked on");

        // Fade in background
        backgroundGroup.alpha = 0;
        LeanTween.alphaCanvas(backgroundGroup, 1, 0.5f);
        //yield return new WaitForSeconds(0.2f); // Small delay before next animation

        // Move in left panel

        LeanTween.moveLocalX(left.gameObject, -377.86f, 0.5f).setEase(LeanTweenType.easeInElastic);
        //yield return new WaitForSeconds(0.1f); // Delay before moving right panel

        // Move in right panel

        LeanTween.moveLocalX(right.gameObject, 626.43f, 0.5f).setEase(LeanTweenType.easeInElastic);
        yield return new WaitForSeconds(0.3f); // Slight delay before scaling animations

        // Scale profile image
        LeanTween.scale(profileImg.rectTransform, Vector3.zero, 0.1f)

            .setOnComplete(() =>
            {
                LeanTween.scale(profileImg.rectTransform, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutElastic);
            });
        //yield return new WaitForSeconds(0.1f); // Delay before bio scaling

        // Scale bio
        LeanTween.scale(bio, Vector3.zero, 0.1f)

            .setOnComplete(() =>
            {
                LeanTween.scale(bio, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutElastic);
            });
    }

    private IEnumerator ReverseAnimateSequence()
    {
        // Scale down bio
        LeanTween.scale(bio, Vector3.zero, 0.1f)
            .setOnComplete(() =>
            {
                // Scale down profile image
                LeanTween.scale(profileImg.rectTransform, Vector3.zero, 0.1f)
                    .setOnComplete(() =>
                    {
                        // Move out right panel
                        LeanTween.moveLocalX(right.gameObject, Screen.width + 500, 0.5f).setEase(LeanTweenType.easeInElastic);
                        // Move out left panel
                        LeanTween.moveLocalX(left.gameObject, -Screen.width - 500, 0.5f).setEase(LeanTweenType.easeInElastic);

                        StartCoroutine(FadeOutAndDisable());
                    });
            });
        yield return null;

    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return new WaitForSeconds(0.3f); // Slight delay before fade out

        // Fade out background
        LeanTween.alphaCanvas(backgroundGroup, 0, 0.5f).setOnComplete(() =>
        {
            canvas.enabled = false;
        });
    }







}
