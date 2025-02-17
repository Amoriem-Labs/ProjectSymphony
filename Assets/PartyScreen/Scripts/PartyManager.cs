using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// Logic here

// check what characters are unlocked, if they are required for this song, and if they are available for this song 
// call displayItems accordingly, displaying new data

// on character slot click -> set partyManager array 
// update the namecard at the button and update selectedUI
// edit the chemistry bar based on affection
// make sure there is one character per role
// once array is full, set go 

public class PartyManager : MonoBehaviour
{
    // 
    public GameObject InventoryMenu;
    public GameObject[] itemSlot;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onButtonPress()
    {
        DisplayItems();
        Debug.Log("button pressed");

        // can put more UI animations here
    }
   public void DisplayItems()
   {
    // Debug.Log("Display Items is called");
    // int counter = 0;


    // else
    // {
    //     Debug.Log("The dictionary is empty.");
    // }
    
    
    
    // foreach (KeyValuePair<string, GameManager.InventoryEntry> kvp in GameManager.Instance.inventoryDictionary)
    // {
    //     Debug.Log("entered for loop");
       
    //     // GameManager.InventoryEntry entry = kvp.Value;

    //      Debug.Log("first item is called" + entry.ItemName + "with this many" + entry.Quantity);
    //     if (entry.Quantity > 0)
    //     {
    //         Debug.Log("adding entry to Item Slot");
    //         itemSlot[counter].SetActive(true);
    //         ItemSlot slot = itemSlot[counter].GetComponent<ItemSlot>();
    //         if (slot != null)
    //         {
    //             slot.AddItem(entry.ItemName, entry.Quantity, entry.Icon, entry.Description);
    //         }
    //         counter++;
    //     }
    // }

   }

   public void DeselectAllSlots()
   {
    for (int i = 0; i < itemSlot.Length; i++)
    {
        ItemSlot slot = itemSlot[i].GetComponent<ItemSlot>();
        slot.selected.SetActive(false);
        slot.thisSelected = false;
    }
   }
}
