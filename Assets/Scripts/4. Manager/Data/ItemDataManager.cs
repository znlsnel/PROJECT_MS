using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using GameData;


public class ItemDataManager : DataManagerHandler<ItemData>
{
    private DataHandler<GameData.Item> itemTable;
 
    protected override void Init()
    {
        if (itemTable != null)
            return;

        itemTable = new DataHandler<GameData.Item>();
        List<Item> items = itemTable.GetAll();

        foreach (Item item in items)
        {
            ItemData itemData = new ItemData(item);
            datas.Add(item.index, itemData);
            dataList.Add(itemData);
        }
    }
}
