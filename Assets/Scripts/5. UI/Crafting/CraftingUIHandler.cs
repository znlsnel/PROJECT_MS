using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;


public static class CraftingUIHandler
{
    public static event Action<CraftingData> onCrafting;
    public static event Action<CraftingData> onSlotClick;

    public static void ClickCraftingSlot(CraftingData data)
    {
        onSlotClick?.Invoke(data); 
    }

    public static void ClickCrafting(CraftingData data)
    {
        onCrafting?.Invoke(data);
    }
}
