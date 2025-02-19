using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Reflection;
using Unity.VisualScripting;

public class CharacterSlot : MonoBehaviour, IPointerClickHandler
{
    // == Character Data == //

    public string characterName;
    public string instrument;

    public int role; // 1 = Melody, 2 = CounterMelody, 3 = Percussion, 4 = Harmony

    public float affection; 
    public Sprite unlockedSprite;
    public Sprite emptySprite;


    // UI // 
    public bool thisSelected;
    public bool isUnlocked;

    public GameObject available;
    public bool isAvailable;

    public GameObject required;
    public bool isRequired;

    Image characterImage;
    Material mat;

    // Object References//

    public GameObject PartyManager;

    PartyManager partyManager;



    private void Start()
    {
        characterImage = GetComponent<Image>();    

        partyManager = PartyManager.GetComponent<PartyManager>();

        mat = Instantiate(characterImage.material);
        characterImage.material = mat; // Assign new instance of material
    }

    public void UnlockCharacter(string instrument, int role, float affection, bool isUnlocked, bool isRequired, bool isAvailable)
    {

        Debug.Log("This Character is Unlocked");

        characterImage.sprite = unlockedSprite;

        // Adjust the RectTransform to 261x261
        RectTransform rectTransform = characterImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(220f, 220f); // Set UI size

        this.instrument = instrument; 
        this.role = role;
        this.affection = affection;
        this.isUnlocked = isUnlocked;
        this.isRequired = isRequired;
        this.isAvailable = isAvailable;

        if (isRequired)
        {
            required.SetActive(true);
        }
        if (isAvailable)
        {
            available.SetActive(true);
        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("right click");
        }

    }

    public void OnLeftClick()
    {
        if(isUnlocked && isAvailable) 
        {
            Debug.Log("selected");
            partyManager.DeselectAllSlots(role);

            mat.SetFloat("_OutlineAlpha", 1f);
            //characterImage.material.SetFloat("_OutlineAlpha", 1f);

            partyManager.SelectCharacter(role, characterName, instrument);
            // selected.SetActive(true);
            thisSelected = true;
        }
        else
        {
            Debug.Log("not unlocked yet");
        }


    }
  
    



}
