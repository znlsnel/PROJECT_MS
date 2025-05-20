using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class StorageUI : PopupUI
{

    [SerializeField] protected Transform storageRoot;
    [SerializeField] private Transform inventoryRoot;
    [SerializeField] private Transform quickSlotRoot;

    protected override void Awake()
    {
        base.Awake();
        Managers.onChangePlayer += SetInventory; 
    }

    private void SetInventory(AlivePlayer player)
    {

        Storage inventory = player.Inventory.ItemStorage;
        Storage quickSlot = player.Inventory.QuickSlotStorage;
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
