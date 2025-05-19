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
    public float Durability {get; private set;}
 

    public Func<ItemData, bool> slotCondition;
    public Action<ItemSlot> onChangeStack;
    public Action<int, ItemSlot> onAddItem;

    public bool IsFull() => Stack >= MaxStack;
    public bool IsEmpty() => Stack <= 0;
    public virtual int MaxStack => Data.MaxStack;
    public virtual float MaxDurability => Data.MaxDurability;

    public ItemSlot() {}
    public ItemSlot(ItemSlot itemSlot)
    {
        Data = itemSlot.Data;
        Stack = itemSlot.Stack;
        Durability = itemSlot.Durability;
        slotCondition = itemSlot.slotCondition;  
        slotItemType = itemSlot.slotItemType; 
        slotEquipType = itemSlot.slotEquipType;
    }

    public void Setup(ItemData itemData, int amount = 0)
    {
        Stack = amount;
        
        if (Data != null)
            onAddItem?.Invoke(Data.Id, this); 
        
        if (itemData != null)
            Durability = itemData.MaxDurability;

        this.Data = itemData; 
        onChangeStack?.Invoke(this); 
    }

    public void UseDurability(float amount)
    {
        Durability -= amount;

        if (Durability <= 0)
            Data = null;

        onChangeStack?.Invoke(this);
    }
    
    public bool IsAddable(int amount = 1)
    {

        return Data != null && Stack + amount <= MaxStack && Stack + amount >= 0;  
    }    

    public bool CheckSlotCondition(ItemData itemData)
    {
        return slotCondition == null ? true : slotCondition.Invoke(itemData);
    }

    public virtual bool AddStack(ItemData itemData, int amount = 1)
    { 
        if ((Data != null && itemData.Id != Data.Id) || !IsAddable(amount))
            return false;
        
        Data = itemData;
        Stack += amount;

        if (Stack == 0)
            Data = null; 

        onChangeStack?.Invoke(this);
        onAddItem?.Invoke(itemData.Id, this);
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
