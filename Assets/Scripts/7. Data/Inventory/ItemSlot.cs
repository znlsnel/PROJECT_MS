using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishNet.Component.Prediction;

public class ItemSlot
{
    public EItemType slotItemType {get; set;} = EItemType.None;
    public EEquipType slotEquipType {get; set;} = EEquipType.None;
    public ItemData Data {get; private set;}
    public int Stack {get; private set;}
 

    public Func<ItemData, bool> slotCondition;
    public Action<ItemSlot> onChangeStack;

    public bool IsFull() => Stack >= MaxStack;
    public bool IsEmpty() => Stack <= 0;
    public int MaxStack => Data.MaxStack;

    public ItemSlot() {}
    public ItemSlot(ItemSlot itemSlot)
    {
        Data = itemSlot.Data;
        Stack = itemSlot.Stack;
    }

    public void Setup(ItemData itemData)
    {
        this.Data = itemData;
        Stack = 0;
        onChangeStack?.Invoke(this); 
    }
     
    public void Clear()
    {
        Data = null;
        Stack = 0;
        onChangeStack?.Invoke(this);
    } 

    public bool IsAddable(int amount = 1)
    {

        return Data != null && Stack + amount <= MaxStack;
    } 

    public bool CheckSlotCondition(ItemData itemData)
    {
        return slotCondition == null ? true : slotCondition.Invoke(itemData); 
    }  

    public bool AddStack(ItemData itemData, int amount = 1)
    { 
        if ((Data != null && itemData.Id != Data.Id) || !IsAddable(amount))
            return false;
        
        Data = itemData;
        Stack += amount;

        if (Stack <= 0)
            Clear();

        onChangeStack?.Invoke(this);
        return true;
    }
    
    public bool ModifyStack(ItemData itemData, int amount = 1)
    {
        if (Data != null && itemData.Id != Data.Id) 
            return false;

        Stack = 0;
        return AddStack(itemData, amount);
    }
    
}
