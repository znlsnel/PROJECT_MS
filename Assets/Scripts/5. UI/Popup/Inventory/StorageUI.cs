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

        _canStack = false; 

    }

    private void OnDestroy()
    {
        Managers.onChangePlayer -= SetInventory;
    }

    private void SetInventory(AlivePlayer player)
    {
        if (player == null || inventoryRoot == null || quickSlotRoot == null)
            return;

        Storage inventory = player.Inventory.ItemStorage;
        Storage quickSlot = player.Inventory.QuickSlotStorage;
        
        if (inventoryRoot != null)
        {
            ItemSlotUI[] inventorySlots = inventoryRoot.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i] == null)
                    continue;
                    
                inventorySlots[i].gameObject.SetActive(i < inventory.Count);
                if (i >= inventory.Count)
                    continue; 

                inventorySlots[i].UnSetup();
                inventorySlots[i].Setup(inventory.GetSlotByIdx(i));
            }
        }

        if (quickSlotRoot != null)
        {
            ItemSlotUI[] quickSlots = quickSlotRoot.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < quickSlots.Length; i++)
            {
                if (quickSlots[i] == null)
                    continue;
                    
                quickSlots[i].gameObject.SetActive(i < quickSlot.Count); 
                if (i >= quickSlot.Count)
                    continue;

                quickSlots[i].UnSetup();
                quickSlots[i].Setup(quickSlot.GetSlotByIdx(i));
            } 
        }
    }
    
}
