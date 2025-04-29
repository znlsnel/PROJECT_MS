using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipStorage : Storage
{
    private Dictionary<EEquipType, ItemSlot> equipSlots = new();
    public EquipStorage() 
    {
        foreach (EEquipType equipType in Enum.GetValues(typeof(EEquipType)))
        {
            ItemSlot itemSlot = new ItemSlot();
            itemSlot.slotEquipType = equipType;
            itemSlots.Add(itemSlot); 
            itemSlot.slotCondition = (itemData) => itemData.EquipType == equipType;

            equipSlots.Add(equipType, itemSlot);
        }
    }

    public ItemSlot GetSlotByType(EEquipType equipType)
    {
        return equipSlots[equipType];
    } 
}
