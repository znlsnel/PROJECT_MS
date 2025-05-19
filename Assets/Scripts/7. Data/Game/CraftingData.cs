using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class CraftingItemData : ItemData
{
    public Storage requiredStorage {get; private set;} = new Storage(3);

    public CraftingItemData(GameData.Crafting crafting)
    {
        var itemData = Managers.Data.items.GetByIndex(crafting.ItemIdx);
        Setup(itemData);

        ItemData item1 = Managers.Data.items.GetByIndex(crafting.item1.Item1);
        ItemData item2 = Managers.Data.items.GetByIndex(crafting.item2.Item1);
        ItemData item3 = Managers.Data.items.GetByIndex(crafting.item3.Item1);

        int amount1 = crafting.item1.Item2;
        int amount2 = crafting.item2.Item2;
        int amount3 = crafting.item3.Item2;

        int idx = 0; 

        if (item1 != null)
            requiredStorage.GetSlotByIdx(idx++).Setup(item1, amount1);
        if (item2 != null)
            requiredStorage.GetSlotByIdx(idx++).Setup(item2, amount2);
        if (item3 != null)
            requiredStorage.GetSlotByIdx(idx).Setup(item3, amount3);  
    }

}
