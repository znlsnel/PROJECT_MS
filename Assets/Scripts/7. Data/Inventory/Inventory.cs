using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;


public class Inventory
{
    public InventoryDataHandler inventoryDataHandler {get; private set;}
    public Storage ItemStorage {get; private set;}
    public Storage QuickSlotStorage {get; private set;}
    public EquipStorage EquipStorage {get; private set;}
    
    public Action<ItemData> onAddItem;
 
    public Inventory() 
    {
        ItemStorage = new Storage(10);
        QuickSlotStorage = new Storage(5);    
        EquipStorage = new EquipStorage();
        inventoryDataHandler = new InventoryDataHandler();

        ItemStorage.onAddItem += inventoryDataHandler.ItemAmountUpdate;
        QuickSlotStorage.onAddItem += inventoryDataHandler.ItemAmountUpdate;
    }



    public bool AddItem(ItemData itemData, int amount = 1)
    {
        Managers.Quest.ReceiveReport(ETaskCategory.Pickup, itemData.Id);

        if (QuickSlotStorage.AddItem(itemData, amount))
        {
            onAddItem?.Invoke(itemData);
            return true;
        }

        if (ItemStorage.AddItem(itemData, amount))
        {
            onAddItem?.Invoke(itemData); 
            return true;
        }

        return false;
    } 
     
    public bool RemoveItem(ItemData itemData, int amount)
    {
        if (InventoryDataHandler.GetItemAmount(itemData.Id) < amount)
            return false;

        inventoryDataHandler.RemoveItem(itemData, amount);
        return true;
    }

    public static void SwapItem(ItemSlot from, ItemSlot to)
    {
        // 둘다 비어있을 경우
        if (from.IsEmpty() && to.IsEmpty())
            return;

        // 한쪽이 비어있을 경우 || 서로 다른 아이템일 경우
        if (from.IsEmpty() || to.IsEmpty() || from.Data != to.Data)
        {
            SwapItemSlot(from, to); 
            return;
        }
        
        // 서로 같은 아이템일 경우
        if (from.Data.Id == to.Data.Id)
        {
            if (from.IsFull() || to.IsFull())
                SwapItemSlot(from, to); 
            else
                MergeItemSlot(from, to);
        }
    }

    private static void MergeItemSlot(ItemSlot from, ItemSlot to)
    { 
        int stackDiff = to.MaxStack - to.Stack;
        int mergeAmount = Mathf.Min(from.Stack, stackDiff);

        to.AddStack(to.Data, mergeAmount);
        from.AddStack(from.Data, -mergeAmount);
    }

    private static void SwapItemSlot(ItemSlot slot1, ItemSlot slot2)
    {
        ItemData tempData = slot1.Data;
        int tempStack = slot1.Stack;

        slot1.Setup(slot2.Data); 
        slot1.ModifyStack(slot2.Data, slot2.Stack);

        slot2.Setup(tempData);
        slot2.ModifyStack(tempData, tempStack);
    }

}
