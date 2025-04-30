using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using GameData;

public class ItemDataManager
{
    private Dictionary<int, ItemData> ItemDatas = new Dictionary<int, ItemData>();
    private List<ItemData> ItemDataList = new List<ItemData>();
    private DataHandler<GameData.Item> itemTable;
 
    public ItemDataManager()
    {
        if (itemTable != null)
            return;

        Managers.SubscribeToInit(Init); 
    }  
    
    public void Init()
    {
        itemTable = new DataHandler<GameData.Item>();
        List<Item> items = itemTable.GetAll();

        foreach (Item item in items)
        {
            ItemData itemData = new ItemData(item);
            ItemDatas.Add(item.index, itemData);
            ItemDataList.Add(itemData);
        }
    }

    public List<ItemData> GetAll()
    {
        return ItemDataList;
    }

    public ItemData GetByIndex(int index)
    {
        if (ItemDatas.TryGetValue(index, out ItemData itemData))
            return itemData;

        return null;
    }

    public ItemData GetByCondition(Func<ItemData, bool> condition)
    {
        foreach (ItemData itemData in ItemDataList)
        {
            if (condition(itemData))
                return itemData;
        }

        return null;
    }

    public List<ItemData> GetAllByCondition(Func<ItemData, bool> condition)
    {
        List<ItemData> items = new List<ItemData>();
        foreach (ItemData itemData in ItemDataList)
        {
            if (condition(itemData))
                items.Add(itemData);
        }

        return items;
    }
}
