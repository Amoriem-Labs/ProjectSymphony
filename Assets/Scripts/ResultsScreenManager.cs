using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;
using Unity.VisualScripting;


public class ResultsScreenManager : MonoBehaviour
{
    public Image gradeImage;
    public Sprite SS_Grade, S_Grade, A_Grade, B_Grade, C_Grade, D_Grade, F_Grade;

    public TextMeshProUGUI favCharText;
    public TextMeshProUGUI ComboText;

    //public TextMeshProUGUI RatioText;

    public TextMeshProUGUI StellarText;
    public TextMeshProUGUI GoodText;
    public TextMeshProUGUI MissText;

    public Image header;
    public Image body;

    public GameObject AllStellar;
    public GameObject FullCombo;

    [SerializeField]
    private float jumpingWait = 0.2f;

    public AudioSource ResultsAudio;
    public AudioClip[] SFX;

    public Button nextButton;

    public ComboBar ChemistryBar;
    public void ShowResultsScreen(int finalScore, int perfectHits, int goodHits, int missedHits, int largestCombo, Dictionary<CharacterRole, List<int>> notes, Dictionary<CharacterRole, float> timeSpentOnCharacter)
    {
        gameObject.SetActive(true);
        Debug.Log("Average Prior Chem:" + AverageChemistry());
        ChemistryBar.SetScore(AverageChemistry()); // PLACEHOLDER: NEED TO CHANGE TO CURRENT CHEMISTRY

        int totalNotes = perfectHits + goodHits + missedHits;

        favCharText.text = GetFavoriteCharacter(timeSpentOnCharacter);
        ComboText.text = largestCombo.ToString();
        StellarText.text = perfectHits.ToString();
        GoodText.text = goodHits.ToString();
        MissText.text = missedHits.ToString();

        double chemistry = UpdateAffections(notes, timeSpentOnCharacter);

        StartCoroutine(ResultsAnimate((float)chemistry));

        UpdateGradeImage(CalculateOverallGrade(notes));

        nextButton.onClick.AddListener(LeaveResults);


    }

    public float AverageChemistry()
    {
        CharacterData[] selected = GameStateManager.Instance.selectedCharacters;

        float totalAffection = 0f;
        int count = 0;

        foreach (CharacterData cd in selected)
        {
            totalAffection += cd.affection;
            count++;
        }

        return totalAffection / count;
    }
    IEnumerator ResultsAnimate(float chemistry)
    {

        LeanTween.scale(StellarText.GetComponent<RectTransform>(), Vector3.zero, 0.1f);
        LeanTween.scale(GoodText.GetComponent<RectTransform>(), Vector3.zero, 0.1f);
        LeanTween.scale(MissText.GetComponent<RectTransform>(), Vector3.zero, 0.1f);

        LeanTween.scale(ComboText.GetComponent<RectTransform>(), Vector3.zero, 0.1f);
        LeanTween.scale(favCharText.GetComponent<RectTransform>(), Vector3.zero, 0.1f);

        RectTransform headerRect = header.GetComponent<RectTransform>();
        RectTransform bodyRect = body.GetComponent<RectTransform>();

        // Start positions (off-screen)
        headerRect.localPosition = new Vector3(-Screen.width, -Screen.height, 0); // Lower left
        bodyRect.localPosition = new Vector3(Screen.width, Screen.height, 0);     // Upper right

        // Scale down to zero initially
        headerRect.localScale = Vector3.zero;
        bodyRect.localScale = Vector3.zero;

        // Move and scale header first
        LeanTween.moveLocal(header.gameObject, new Vector3(-472, 230, 0), 0.3f)
                .setEase(LeanTweenType.easeOutQuart);
        LeanTween.scale(header.gameObject, new Vector3(10, 10, 10), 0.3f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {

                    // Move and scale body after header animation
                    LeanTween.moveLocal(body.gameObject, new Vector3(148, 30, 0), 0.3f)
                            .setEase(LeanTweenType.easeOutQuart);
                    LeanTween.scale(body.gameObject, new Vector3(16.5f, 16.5f, 16.5f), 0.3f)
                            .setEase(LeanTweenType.easeOutBack);
                });


        yield return new WaitForSeconds(0.6f);

        LeanTween.scale(StellarText.GetComponent<RectTransform>(), Vector3.one, jumpingWait)
            .setEase(LeanTweenType.easeInBounce);
        ResultsAudio.PlayOneShot(SFX[0]);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(GoodText.GetComponent<RectTransform>(), Vector3.one, jumpingWait)
            .setEase(LeanTweenType.easeInBounce);
        ResultsAudio.PlayOneShot(SFX[0]);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(MissText.GetComponent<RectTransform>(), Vector3.one, jumpingWait);
        ResultsAudio.PlayOneShot(SFX[0]);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(ComboText.GetComponent<RectTransform>(), new Vector3(0.1f, 0.1f, 0.1f), jumpingWait);
        ResultsAudio.PlayOneShot(SFX[0]);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(favCharText.GetComponent<RectTransform>(), new Vector3(0.1f, 0.1f, 0.1f), jumpingWait);
        ResultsAudio.PlayOneShot(SFX[0]);

        yield return new WaitForSeconds(0.5f);

        if (chemistry < 0)
        {
            ChemistryBar.ChangeColorBlue();
        }
        ChemistryBar.SetScore(chemistry);
        // change chemistry here PLACEHOLDER: CHEMISTRY HERE IS RETURNED AS THE NET CHANGE

        ResultsAudio.PlayOneShot(SFX[2]);

        yield return null;
    }
    public void HideResultsScreen()
    {
        gameObject.SetActive(false);
    }

    public void TryAgain()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Updates character affections and calculates total chemistry.
    /// </summary>
    /// <returns>The total chemistry.</returns>
    public double UpdateAffections(Dictionary<CharacterRole, List<int>> notes, Dictionary<CharacterRole, float> timeSpentOnCharacter)
    {
        double totalChemistryChange = 0.0;
        int characterCount = 0;
        float totalTime = 0f;

        foreach (var character in timeSpentOnCharacter.Values)
        {
            totalTime += character;
        }


        foreach (CharacterRole character in notes.Keys)
        {
            float timeSpent = timeSpentOnCharacter[character];
            List<int> hitCounts = notes[character];

            int grade = (int)Math.Round(CalculateGrade(hitCounts));
            float timeSpentPercentage = timeSpent / totalTime;

            float affectionChange = CalculateAffectionChange(timeSpentPercentage, grade);
            GameStateManager.Instance.GetSelectedCharacterWithRole(character).affection += affectionChange;
            totalChemistryChange += affectionChange;
            characterCount++;
        }

        return totalChemistryChange / characterCount;
    }

    public double CalculateGrade(List<int> hitCounts)
    {
        int stellar = hitCounts[0];
        int good = hitCounts[1];
        int missed = hitCounts[2];
        int totalHits = stellar + good + missed;

        if (totalHits == 0)
        {
            return 0;
        }

        float stellarRatio = (float)stellar / totalHits * 100;
        float goodRatio = (float)good / totalHits * 100;
        float missRatio = (float)missed / totalHits * 100;


        if (stellarRatio >= 85 && missRatio == 0)
        {
            return 5; // S
        }
        else if (stellarRatio >= 65 && missRatio < 5)
        {
            return 4; // A
        }
        else if (stellarRatio >= 40 && missRatio < 10)
        {
            return 3; // B
        }
        else if (stellarRatio >= 20 && missRatio < 20)
        {
            return 2; // C
        }
        else if (stellarRatio >= 10 && (goodRatio > 75 || (missRatio >= 20 && missRatio <= 30)))
        {
            return 1; // D
        }
        else
        {
            return 0; // F
        }
    }

    public float CalculateAffectionChange(float timeSpentPercentage, int grade)
    {
        Dictionary<int, float> gradeMultipliers = new Dictionary<int, float>
        {
            { 5, 1.0f },   // S
            { 4, 0.75f },  // A
            { 3, 0.5f },   // B
            { 2, 0.0f },   // C
            { 1, -0.5f },  // D
            { 0, -1.0f }   // F
        };

        float affectionChange = timeSpentPercentage * 10 * gradeMultipliers[grade];

        return affectionChange;
    }

    public String GetFavoriteCharacter(Dictionary<CharacterRole, float> timeSpentOnCharacter)
    {
        float maxTime = 0f;
        CharacterRole favCharacter = CharacterRole.None;

        foreach (var pair in timeSpentOnCharacter)
        {
            if (pair.Value > maxTime)
            {
                maxTime = pair.Value;
                favCharacter = pair.Key;
            }
        }

        Character character = GameStateManager.Instance.GetSelectedCharacterWithRole(favCharacter).character;
        return character.name;
    }

    private void UpdateGradeImage(int grade)
    {
        LeanTween.scale(gradeImage.GetComponent<RectTransform>(), Vector3.zero, 0.1f);
        switch (grade)
        {
            case 5:
                gradeImage.sprite = S_Grade;
                break;
            case 4:
                gradeImage.sprite = A_Grade;
                break;
            case 3:
                gradeImage.sprite = B_Grade;
                break;
            case 2:
                gradeImage.sprite = C_Grade;
                break;
            case 1:
                gradeImage.sprite = D_Grade;
                break;
            default:
                gradeImage.sprite = F_Grade;
                break;
        }
        StartCoroutine(AnimateGrade());
    }
    private IEnumerator AnimateGrade()
    {
        yield return new WaitForSeconds(2f);
        ResultsAudio.PlayOneShot(SFX[1]);
        LeanTween.scale(gradeImage.GetComponent<RectTransform>(), new Vector3(0.15f, 0.15f, 0.15f), jumpingWait)
       .setEase(LeanTweenType.easeInBounce);

        yield return null;

    }

    private int CalculateOverallGrade(Dictionary<CharacterRole, List<int>> notes)
    {
        int totalStellar = 0, totalGood = 0, totalMissed = 0;
        int totalNotes = 0;

        foreach (var entry in notes.Values)
        {
            totalStellar += entry[0];
            totalGood += entry[1];
            totalMissed += entry[2];
        }

        totalNotes = totalStellar + totalGood + totalMissed;
        if (totalNotes == 0) return 0;

        float stellarRatio = (float)totalStellar / totalNotes * 100;
        float goodRatio = (float)totalGood / totalNotes * 100;
        float missRatio = (float)totalMissed / totalNotes * 100;

        if (stellarRatio >= 85 && missRatio == 0) return 5;
        if (stellarRatio >= 65 && missRatio < 5) return 4;
        if (stellarRatio >= 40 && missRatio < 10) return 3;
        if (stellarRatio >= 20 && missRatio < 20) return 2;
        if (stellarRatio >= 10 && (goodRatio > 75 || (missRatio >= 20 && missRatio <= 30))) return 1;
        return 0; // F
    }

    public void LeaveResults()
    {
        if (GameStateManager.Instance.freePlay)
        {
            GameStateManager.Instance.LoadNewScene("TitleScene");
            GameStateManager.Instance.freePlay = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("SceneIndex.") == 1)
            {
                GameStateManager.Instance.LoadNewScene("VN");
            }
            else if (PlayerPrefs.GetInt("CurrentWeek.") == 2)
            {
                GameStateManager.Instance.LoadNewScene("MapScreen");
            }
            else if (GameStateManager.Instance.DemoComplete == true)
            {
                GameStateManager.Instance.LoadNewScene("TitleScene");
            }
            else if (PlayerPrefs.GetInt("SceneIndex.") == 12)
            {
                Debug.Log("loading week 3 VN");
                GameStateManager.Instance.LoadNewScene("VN");
            }

        }
    }

}


