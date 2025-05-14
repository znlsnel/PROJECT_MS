using UnityEngine;
using System;
using System.Collections.Generic;

public class QuestStorageData
{
    public List<RequireItem> items {get; private set;} = new List<RequireItem>();

    public QuestStorageData(GameData.QuestStorage questStorage)
    {
        Action<(int, int)> Create = ((int, int) requireItem) => {
            ItemData itemData = Managers.Data.items.GetByIndex(requireItem.Item1);

            if (itemData == null)
                return;

            items.Add(new RequireItem(itemData, requireItem.Item2));
        };
        
        Create(questStorage.item1);
        Create(questStorage.item2);
        Create(questStorage.item3);
        Create(questStorage.item4);
        Create(questStorage.item5);
    }
}