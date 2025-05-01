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

    public static void ClickCrafting(ItemData data)
    {  
        onCrafting?.Invoke(data);
        Managers.UserData.Inventory.AddItem(data); 
        Debug.Log("ClickCrafting");
    }
}
