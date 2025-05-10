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




public class CraftingDataManager : DataHandler<CraftingItemData>
{
    private UGSDataHandler<GameData.Crafting> craftingTable;


    protected override void Init()
    {
        if (craftingTable != null)
            return;

        craftingTable = new UGSDataHandler<GameData.Crafting>();
        List<GameData.Crafting> craftings = craftingTable.GetAll();
        foreach (GameData.Crafting crafting in craftings) 
        {
            CraftingItemData craftingData = new CraftingItemData(crafting);
        
            datas.Add(crafting.index, craftingData);
            dataList.Add(craftingData);  
        }
    }
}
