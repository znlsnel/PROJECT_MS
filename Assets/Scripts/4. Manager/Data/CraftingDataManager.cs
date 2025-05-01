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

    public RequireItem[] requiredItems = new RequireItem[3];

    public CraftingData(GameData.Crafting crafting)
    {
        index = crafting.index;
        itemData = Managers.Data.items.GetByIndex(crafting.ItemIdx);
        
        ItemData item1 = Managers.Data.items.GetByIndex(crafting.item1.Item1);
        ItemData item2 = Managers.Data.items.GetByIndex(crafting.item2.Item1);
        ItemData item3 = Managers.Data.items.GetByIndex(crafting.item3.Item1);

        int amount1 = crafting.item1.Item2;
        int amount2 = crafting.item2.Item2;
        int amount3 = crafting.item3.Item2;

        if (item1 != null)
            requiredItems[0] = new RequireItem(item1, amount1); 
        if (item2 != null)
            requiredItems[1] = new RequireItem(item2, amount2); 
        if (item3 != null)
            requiredItems[2] = new RequireItem(item3, amount3);  
    }
}


public class CraftingDataManager : BaseDataHandler<CraftingData>
{
    private DataHandler<GameData.Crafting> craftingTable;


    protected override void Init()
    {
        if (craftingTable != null)
            return;

        craftingTable = new DataHandler<GameData.Crafting>();
        List<GameData.Crafting> craftings = craftingTable.GetAll();
        foreach (GameData.Crafting crafting in craftings) 
        {
            CraftingData craftingData = new CraftingData(crafting);
        
            datas.Add(crafting.index, craftingData);
            dataList.Add(craftingData);  
        }
    }
}
