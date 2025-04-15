using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IPointerClickHandler
{
    public CharacterData characterData;
    // public GameObject available;
    // public GameObject required;

    public bool thisSelected;
    public bool isAvailable;
    public bool isRequired;

    Image characterImage;

    // Object References//
    public PartyManager partyManager;

    private void Start()
    {

        characterImage = GetComponent<Image>();
        // Allow changing of the material without affecting others that use it
        characterImage.material = Instantiate(characterImage.material);

        RectTransform rectTransform = characterImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(220f, 220f); // Set UI size

        characterImage.sprite = (characterData.isUnlocked && isAvailable) ? characterData.character.spriteUnlocked : characterData.character.spriteLocked;
        // required.SetActive(isRequired);
        // available.SetActive(isAvailable);

        characterImage.material.SetFloat("_AlphaOutlineBlend", 0f);
        characterImage.material.SetFloat("_HologramBlend", 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

    }

    public void OnLeftClick()
    {
        if (!characterData.isUnlocked || !isAvailable) return;
        partyManager.DeselectAllSlots(characterData.character.role);

        characterImage.material.SetFloat("_AlphaOutlineBlend", 1f);
        characterImage.material.SetFloat("_HologramBlend", 0.1f);
        thisSelected = true;

        partyManager.SelectCharacter(characterData);
    }

    public void Deselect()
    {
        thisSelected = false;
        characterImage.material.SetFloat("_AlphaOutlineBlend", 0f);
        characterImage.material.SetFloat("_HologramBlend", 0f);
    }
    public void hoverBigger()
    {
        if (characterData.isUnlocked && isAvailable)
        {
            // Bouncy scale up to 1.2 over 0.4 seconds
            LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.4f)
                .setEase(LeanTweenType.easeOutBack);
        }
    }
    public void hoverSmaller()
    {
        if (characterData.isUnlocked && isAvailable)
        {
            // Scale back to original size
            LeanTween.scale(gameObject, Vector3.one, 0.2f)
                .setEase(LeanTweenType.easeInOutQuad);
        }
    }
}
