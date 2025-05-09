using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class StorageUI : PopupUI
{

    [SerializeField] private Transform storageRoot;
    [SerializeField] private Transform inventoryRoot;
    [SerializeField] private Transform quickSlotRoot;

    private void Start()
    {
        SetInventory(); 
    }

    public void Setup(Storage storage)
    {

        InventorySlotUI[] storageSlots = storageRoot.GetComponentsInChildren<InventorySlotUI>();
        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i].UnSetup();

            ItemSlot itemSlot = storage.GetSlotByIdx(i);
            if (itemSlot == null)
                itemSlot = storage.CreateSlot();

            storageSlots[i].Setup(itemSlot); 
        }

    }

    private void SetInventory()
    {

        Storage inventory = Managers.UserData.Inventory.ItemStorage;
        Storage quickSlot = Managers.UserData.Inventory.QuickSlotStorage;
        InventorySlotUI[] inventorySlots = inventoryRoot.GetComponentsInChildren<InventorySlotUI>();
        InventorySlotUI[] quickSlots = quickSlotRoot.GetComponentsInChildren<InventorySlotUI>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].gameObject.SetActive(i < inventory.Size);
            if (i >= inventory.Size)
                continue; 

            inventorySlots[i].UnSetup();
            inventorySlots[i].Setup(inventory.GetSlotByIdx(i));
        }

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].gameObject.SetActive(i < quickSlot.Size); 
            if (i >= quickSlot.Size)
                continue;

            quickSlots[i].UnSetup();
            quickSlots[i].Setup(quickSlot.GetSlotByIdx(i));
        } 
    }
    
}
