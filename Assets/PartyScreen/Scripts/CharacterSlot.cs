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
    // == Item Data == //

    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;


    // == Item Slot == //
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    // == Item Description == //

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;


    //Other//

    public GameObject selected;
    public bool thisSelected;

    public GameObject available;

    public bool thisAvailable;

    public GameObject required;

    public bool thisRequired;

    PartyManager partyManager;

    private void Start()
    {
        //inventoryManager = GameObject.Find("Inventory Canvas").GetComponent<InventoryManager>();
    }

 
    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {

        Debug.Log("Item slot now has new item inside");
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        this.isFull = true;

        quantityText.text = quantity.ToString();
        Debug.Log("quantityText enabled status: " + quantityText.enabled);
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }

    }

    public void OnLeftClick()
    {
        partyManager.DeselectAllSlots();
        selected.SetActive(true);
        thisSelected = true;

        // itemDescriptionNameText.text = itemName;
        // itemDescriptionText.text = itemDescription;
        // itemDescriptionImage.sprite = itemSprite;

        // if(itemDescriptionImage.sprite == null)
        // {
        //     itemDescriptionImage.sprite = emptySprite;
        // }

        // set array here


    }
    public void OnRightClick()
    {
        
    }



}
