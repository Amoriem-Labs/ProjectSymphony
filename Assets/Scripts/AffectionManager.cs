using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("getting affection:");
        float affection = GetCharacterAffection("Carter");
        Debug.Log("updating affection:");
        UpdateCharacterAffection("Carter", 10);
        Debug.Log("getting unlock:");
        bool unlocked = CharacterIsUnlocked("Carter");
        Debug.Log("updating unlock: (ADD NEXT)");
        UpdateCharacterUnlock("Carter", false);
    }

    // Update is called once per frame
    void Update()
    {
        //GameStateManager.Instance.characterData.ElementAt(0);
    }



    float GetCharacterAffection(string searchName)
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


    void UpdateCharacterAffection(string searchName, float incrementValue)
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

    bool CharacterIsUnlocked(string searchName)
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

    void UpdateCharacterUnlock(string searchName, bool unlockValue)
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


