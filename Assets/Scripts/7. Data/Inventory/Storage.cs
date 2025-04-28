using System;
using System.Collections.Generic;
using System.Linq;

public class Storage
{
    protected List<ItemSlot> itemSlots = new();
    public Action<List<ItemSlot>> onChangeStorage;


    /// <summary>
    /// 인덱스로 슬롯을 찾음
    /// </summary>
    public ItemSlot GetSlotByIdx(int idx)
    {
        if (idx < 0 || idx >= itemSlots.Count)
            return null;

        return itemSlots[idx];
    }

    public List<ItemSlot> GetSlotsByCondition(Func<ItemSlot, bool> condition)
    {
        return itemSlots.Where(condition).ToList(); 
    }

    public ItemSlot CreateSlot()
    {
        ItemSlot slot = new ItemSlot(); 
        itemSlots.Add(slot);
        return slot; 
    }


    public bool AddItem(int idx, int amount)
    {
        if (idx < 0 || idx >= itemSlots.Count)
            return false;

        ItemSlot slot = itemSlots[idx];
        return AddItem(slot, amount); 
    }

    public bool AddItem(ItemData itemData, int amount = 1)
    {
        ItemSlot slot = FindSlotByItemData(itemData, amount); 
        if (slot == null)
        {
            slot = FindFirstEmptySlot();
            if (slot == null)
                return false;

            slot.Setup(itemData);
        }
        
        return AddItem(slot, amount);
    }

    public bool AddItem(ItemSlot slot, int amount = 1)
    {
        if (slot.AddStack(slot.Data, amount))
        {
            onChangeStorage?.Invoke(itemSlots); // 물을 가지고 오다 (11:11 ~)
            return true;
        }
        return false;
    }

    public bool RemoveItem(int idx, int amount = 1)
    {
        return AddItem(idx, -amount);
    }

    public bool RemoveItem(ItemData data, int amount = 1)
    {
        return AddItem(data, -amount);
    }

    public bool RemoveItem(ItemSlot slot, int amount = 1)
    {
        return AddItem(slot, -amount);
    }

    public ItemSlot FindSlotByItemData(ItemData itemData, int amount = 1)
    {
        return itemSlots.Find(x => x.Data != null && x.Data.Id == itemData.Id && x.IsAddable(amount));   
    }

    public ItemSlot FindFirstEmptySlot()
    {
        return itemSlots.Find(x => x.Data == null);
    }
}
