using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectionManager : MonoBehaviour
{
    private const string HOWARD_AFFECTION_KEY = "HowardAffection.";
    private const string SAM_AFFECTION_KEY = "SamAffection.";
    private const string DAYLO_AFFECTION_KEY = "DayloAffection.";
    private const string CARTER_AFFECTION_KEY = "CarterAffection.";
    private const string PAULINE_AFFECTION_KEY = "PaulineAffection.";
    private const string SCENE_INDEX_KEY = "SceneIndex.";
    private const string HOWARD_UNLOCKED_KEY = "HowardUnlocked.";
    private const string SAM_UNLOCKED_KEY = "SamUnlocked.";
    private const string CARTER_UNLOCKED_KEY = "CarterUnlocked.";
    private const string PAULINE_UNLOCKED_KEY = "PaulineUnlocked.";
    private const string DAYLO_UNLOCKED_KEY = "DayloUnlocked.";
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("getting affection:");
        //float affection = GetCharacterAffection("Carter");
        //Debug.Log("updating affection:");
        //UpdateCharacterAffection("Carter", 10);
        //Debug.Log("getting unlock:");
        //bool unlocked = CharacterIsUnlocked("Carter");
        //Debug.Log("updating unlock: (ADD NEXT)");
        //UpdateCharacterUnlock("Carter", false);
    }

    // Update is called once per frame
    void Update()
    {
        //GameStateManager.Instance.characterData.ElementAt(0);
    }

    public void UpdatePlayerPrefAffections()
    {
        int affection = (int)GetCharacterAffection("Carter");
        if(affection != -1)
        {
            PlayerPrefs.SetInt(CARTER_AFFECTION_KEY, affection);
        }
        else
        {
            Debug.Log("Error, affection failed retrieval");
        }

        affection = (int)GetCharacterAffection("Sam");
        if (affection != -1)
        {
            PlayerPrefs.SetInt(SAM_AFFECTION_KEY, affection);
        }
        else
        {
            Debug.Log("Error, affection failed retrieval");
        }

        affection = (int)GetCharacterAffection("Howard");
        if (affection != -1)
        {
            PlayerPrefs.SetInt(HOWARD_AFFECTION_KEY, affection);
        }
        else
        {
            Debug.Log("Error, affection failed retrieval");
        }

        affection = (int)GetCharacterAffection("Daylo");
        if (affection != -1)
        {
            PlayerPrefs.SetInt(DAYLO_AFFECTION_KEY, affection);
        }
        else
        {
            Debug.Log("Error, affection failed retrieval");
        }

        affection = (int)GetCharacterAffection("Pauline");
        if (affection != -1)
        {
            PlayerPrefs.SetInt(PAULINE_AFFECTION_KEY, affection);
        }
        else
        {
            Debug.Log("Error, affection failed retrieval");
        }
        //TODO: add unlocked to this
    }

    public void UpdateGsmAffection()
    {
        Debug.Log("here??");
        SetCharacterAffection("Carter", PlayerPrefs.GetInt(CARTER_AFFECTION_KEY));
        SetCharacterAffection("Sam", PlayerPrefs.GetInt(SAM_AFFECTION_KEY));
        SetCharacterAffection("Howard", PlayerPrefs.GetInt(HOWARD_AFFECTION_KEY));
        SetCharacterAffection("Daylo", PlayerPrefs.GetInt(DAYLO_AFFECTION_KEY));
        SetCharacterAffection("Pauline", PlayerPrefs.GetInt(PAULINE_AFFECTION_KEY));
        int newUnlock;
        UpdateCharacterUnlock("Carter", (newUnlock = PlayerPrefs.GetInt(CARTER_UNLOCKED_KEY)) == 1);
        UpdateCharacterUnlock("Sam", (newUnlock = PlayerPrefs.GetInt(SAM_UNLOCKED_KEY)) == 1);
        UpdateCharacterUnlock("Pauline", (newUnlock = PlayerPrefs.GetInt(PAULINE_UNLOCKED_KEY)) == 1);
        UpdateCharacterUnlock("Daylo", (newUnlock = PlayerPrefs.GetInt(DAYLO_UNLOCKED_KEY)) == 1);
        UpdateCharacterUnlock("Howard", (newUnlock = PlayerPrefs.GetInt(HOWARD_UNLOCKED_KEY)) == 1);
    }
    public float GetCharacterAffection(string searchName)
    {

        // Find the Character object by name
        Character[] charArr = GameStateManager.Instance.characters;
        Character characterSought = null;
        for (int index = 0; index < charArr.Length; index++ )
        {
            if(charArr[index].name == searchName)
            {
                characterSought = charArr[index];
            }
        }
        
            //.FirstOrDefault(c => c.name == searchName);

        if (characterSought != null && GameStateManager.Instance.characterData.TryGetValue(characterSought, out CharacterData data))
        {
            Debug.Log($"Affection: {data.affection}, Unlocked: {data.isUnlocked}");
            return data.affection;
        }
        else
        {
            Debug.LogWarning("Character not found!");
            return -1;
        }

    }

    public void SetCharacterAffection(string searchName, float incrementValue)
    {

        // Find the Character object by name
        Character[] charArr = GameStateManager.Instance.characters;
        Character characterSought = null;
        for (int index = 0; index < charArr.Length; index++)
        {
            if (charArr[index].name == searchName)
            {
                characterSought = charArr[index];
            }
        }

        //.FirstOrDefault(c => c.name == searchName);

        if (characterSought != null && GameStateManager.Instance.characterData.TryGetValue(characterSought, out CharacterData data))
        {
            Debug.Log($"Prior Affection: {data.affection}");
            data.affection = incrementValue;
            Debug.Log($"Current Affection: {data.affection}");

        }
        else
        {
            Debug.LogWarning("Character not found!");
        }

    }
    public void UpdateCharacterAffection(string searchName, float incrementValue)
    {

        // Find the Character object by name
        Character[] charArr = GameStateManager.Instance.characters;
        Character characterSought = null;
        for (int index = 0; index < charArr.Length; index++)
        {
            if (charArr[index].name == searchName)
            {
                characterSought = charArr[index];
            }
        }

        //.FirstOrDefault(c => c.name == searchName);

        if (characterSought != null && GameStateManager.Instance.characterData.TryGetValue(characterSought, out CharacterData data))
        {
            Debug.Log($"Prior Affection: {data.affection}");
            data.affection += incrementValue;
            Debug.Log($"Current Affection: {data.affection}");

        }
        else
        {
            Debug.LogWarning("Character not found!");
        }

    }

    public bool CharacterIsUnlocked(string searchName)
    {

        // Find the Character object by name
        Character[] charArr = GameStateManager.Instance.characters;
        Character characterSought = null;
        for (int index = 0; index < charArr.Length; index++)
        {
            if (charArr[index].name == searchName)
            {
                characterSought = charArr[index];
            }
            
        }

        //.FirstOrDefault(c => c.name == searchName);

        if (characterSought != null && GameStateManager.Instance.characterData.TryGetValue(characterSought, out CharacterData data))
        {
            Debug.Log($"{data.character.name}Unlocked: {data.isUnlocked}");
            if (data.isUnlocked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogWarning("Character not found!");
            return false;
        }

    }

    public void UpdateCharacterUnlock(string searchName, bool unlockValue)
    {

        // Find the Character object by name
        Character[] charArr = GameStateManager.Instance.characters;
        Character characterSought = null;
        for (int index = 0; index < charArr.Length; index++)
        {
            if (charArr[index].name == searchName)
            {
                characterSought = charArr[index];
            }

        }

        //.FirstOrDefault(c => c.name == searchName);

        if (characterSought != null && GameStateManager.Instance.characterData.TryGetValue(characterSought, out CharacterData data))
        {
            Debug.Log($"{data.character.name}, prior unlocked: {data.isUnlocked}");
            data.isUnlocked = unlockValue;
            Debug.Log($"{data.character.name}, current unlocked: {data.isUnlocked}");

        }
        else
        {
            Debug.LogWarning("Character not found!");
            
        }

    }

}


