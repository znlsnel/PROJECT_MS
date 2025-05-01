using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;


public static class CraftingUIHandler
{
    public static event Action<ItemData> onCrafting;
    public static event Action<CraftingData> onSlotClick;

    public static void ClickCraftingSlot(CraftingData data)
    {
        onSlotClick?.Invoke(data); 
    }

    public static void ClickCrafting(CraftingData data)
    {  
        for (int i = 0; i < data.requiredItems.Length; i++){
            if (data.requiredItems[i] == null)
                continue;

            if (InventoryDataHandler.GetItemAmount(data.requiredItems[i].itemData.Id) < data.requiredItems[i].amount)
                return;
        }

        onCrafting?.Invoke(data.itemData);
        Managers.UserData.Inventory.AddItem(data.itemData); 

        for (int i = 0; i < data.requiredItems.Length; i++)
        {
            if (data.requiredItems[i] == null)
                continue;

            Managers.UserData.Inventory.RemoveItem(data.requiredItems[i].itemData, data.requiredItems[i].amount); 
        }

        Debug.Log("ClickCrafting");
    }
}
