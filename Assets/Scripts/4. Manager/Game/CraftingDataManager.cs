using System.Collections.Generic;
using GameData;
using UnityEngine;

public class RequireItem
{
    public ItemData itemData;
    public int amount;

    public RequireItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        this.amount = amount;
    }
}

public class CraftingData
{
    public int index;
    public ItemData itemData;
    public RequireItem item1;
    public RequireItem item2;
    public RequireItem item3;
}

public class CraftingDataManager
{
    public static List<CraftingData> CraftingDataList = new List<CraftingData>();
    public CraftingDataManager()
    {
        if (CraftingDataList.Count > 0)
            return;

        Managers.SubscribeToInit(Init);
    }

    public void Init()
    {
        List<Crafting> craftings = Managers.Data.craftings.GetAll();
        foreach (Crafting crafting in craftings)
        {
            CraftingData craftingData = new CraftingData();
            craftingData.index = crafting.index;
            craftingData.itemData = Managers.Data.items.GetByIndex(crafting.ItemIdx);

            ItemData item1 = Managers.Data.items.GetByIndex(crafting.item1.Item1);
            ItemData item2 = Managers.Data.items.GetByIndex(crafting.item2.Item1);
            ItemData item3 = Managers.Data.items.GetByIndex(crafting.item3.Item1);

            int amount1 = crafting.item1.Item2;
            int amount2 = crafting.item2.Item2;
            int amount3 = crafting.item3.Item2;

            if (item1 != null)
                craftingData.item1 = new RequireItem(item1, amount1); 
            if (item2 != null)
                craftingData.item2 = new RequireItem(item2, amount2); 
            if (item3 != null)
                craftingData.item3 = new RequireItem(item3, amount3);

            CraftingDataList.Add(craftingData);
        }
    }
}
