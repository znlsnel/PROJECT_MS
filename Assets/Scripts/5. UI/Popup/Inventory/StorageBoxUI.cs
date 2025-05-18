using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class StorageBoxUI : StorageUI
{
    public void Setup(Storage storage)
    {

        InventorySlotUI[] storageSlots = storageRoot.GetComponentsInChildren<InventorySlotUI>();
        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i].UnSetup();

            ItemSlot itemSlot = storage.GetSlotByIdx(i);
            if (itemSlot == null)
                itemSlot = storage.CreateSlot();

            storageSlots[i].Setup(itemSlot); 
        }

    }
}
