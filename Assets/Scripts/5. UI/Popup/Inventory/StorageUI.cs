using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class StorageUI : PopupUI
{

    [SerializeField] protected Transform storageRoot;
    [SerializeField] private Transform inventoryRoot;
    [SerializeField] private Transform quickSlotRoot;

    private void Start()
    {
        SetInventory(); 
    }

    private void SetInventory()
    {

        Storage inventory = Managers.UserData.Inventory.ItemStorage;
        Storage quickSlot = Managers.UserData.Inventory.QuickSlotStorage;
        ItemSlotUI[] inventorySlots = inventoryRoot.GetComponentsInChildren<ItemSlotUI>();
        ItemSlotUI[] quickSlots = quickSlotRoot.GetComponentsInChildren<ItemSlotUI>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].gameObject.SetActive(i < inventory.Count);
            if (i >= inventory.Count)
                continue; 

            inventorySlots[i].UnSetup();
            inventorySlots[i].Setup(inventory.GetSlotByIdx(i));
        }

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].gameObject.SetActive(i < quickSlot.Count); 
            if (i >= quickSlot.Count)
                continue;

            quickSlots[i].UnSetup();
            quickSlots[i].Setup(quickSlot.GetSlotByIdx(i));
        } 
    }
    
}
