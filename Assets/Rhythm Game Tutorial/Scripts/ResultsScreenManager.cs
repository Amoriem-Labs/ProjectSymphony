using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System;
public class ResultsScreenManager : MonoBehaviour
{
    public Image gradeImage;
    public Sprite SS_Grade, S_Grade, A_Grade, B_Grade, C_Grade, D_Grade;

    public TextMeshProUGUI favCharText;
    public TextMeshProUGUI ComboText;
    
    public TextMeshProUGUI RatioText;
    public void ShowResultsScreen(int finalScore, int perfectHits, int goodHits, int missedHits, int largestCombo, Dictionary<CharacterRole, List<int>> notes, Dictionary<CharacterRole, float> timeSpentOnCharacter)
    {
        gameObject.SetActive(true);

        int totalNotes = perfectHits + goodHits + missedHits;

        favCharText.text = GetFavoriteCharacter(timeSpentOnCharacter);
        ComboText.text = largestCombo.ToString();
        RatioText.text = perfectHits.ToString() + "/" + goodHits.ToString() + "/" + missedHits.ToString();

        foreach (var character in notes.Keys)
        {
            if (notes[character].Count > 0) 
            {
                Debug.Log($"{character}: {notes[character][0]}/{notes[character][1]}/{notes[character][2]} Time: {timeSpentOnCharacter[character]} Grade: {CalculateGrade(notes[character])}");
            }
        }
        Debug.Log($"Largest Combo: {largestCombo}");
        Debug.Log($"Combined Overall Ratio: {perfectHits}/{goodHits}/{missedHits}");
        Debug.Log($"Overall Chem: {CalculateChemistry(notes, timeSpentOnCharacter)}");
        Debug.Log($"Fav Charcter: {GetFavoriteCharacter(timeSpentOnCharacter)}");

        UpdateGradeImage(CalculateOverallGrade(notes));
    }

    public void HideResultsScreen()
    {
        gameObject.SetActive(false);
    }

    public void TryAgain()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public double CalculateChemistry(Dictionary<CharacterRole, List<int>> notes, Dictionary<CharacterRole, float> timeSpentOnCharacter)
    {
        double totalChemistryChange = 0.0;
        int characterCount = 0;
        float totalTime = 0f;

        foreach (var character in timeSpentOnCharacter.Values)
        {
            totalTime += character;
        }


        foreach (var character in notes.Keys)
        {
            float timeSpent = timeSpentOnCharacter[character]; 
            List<int> hitCounts = notes[character];

            int grade = (int)Math.Round(CalculateGrade(hitCounts));
            float timeSpentPercentage = timeSpent / totalTime;
            
            float affectionChange = CalculateAffectionChange(timeSpentPercentage, grade);
            Debug.Log($"Time%: {timeSpentPercentage}, Grade: {grade}, affchange: {affectionChange}");
            totalChemistryChange += affectionChange;
            characterCount++;
        }

        Debug.Log($"TotalChem: {totalChemistryChange}, characterCount: {characterCount}");
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

    public String GetFavoriteCharacter(Dictionary<CharacterRole, float> timeSpentOnCharacter){
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
        return favCharacter.ToString();
    }

    private void UpdateGradeImage(int grade)
    {
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
                gradeImage.sprite = SS_Grade;
                break;
        }
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
}


