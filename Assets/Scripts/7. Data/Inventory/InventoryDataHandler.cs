using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public static class InventoryDataHandler
{
    public static Action onItemAmountUpdate;
    public static Dictionary<int, HashSet<ItemSlot>> itemAmounts {get; private set;} = new Dictionary<int, HashSet<ItemSlot>>();

    public static int GetItemAmount(int id)
    {
        if (itemAmounts.ContainsKey(id))
            return itemAmounts[id].Sum(slot => slot.Stack);
        return 0;
    }

    public static void ItemAmountUpdate(int id, ItemSlot itemSlot)
    {
        if (itemAmounts.ContainsKey(id))
        {
            if (itemSlot.Stack == 0)
                itemAmounts[id].Remove(itemSlot);
            else
                itemAmounts[id].Add(itemSlot);
        }
        else if (itemSlot.Stack > 0)
            itemAmounts[id] = new HashSet<ItemSlot> {itemSlot}; 

        onItemAmountUpdate?.Invoke();
        Debug.Log($"ItemAmountUpdate: {id} {itemAmounts[id].Sum(slot => slot.Stack)}"); 
    }
}
