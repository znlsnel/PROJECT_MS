using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishNet.Component.Prediction;

public class ItemSlot
{
    public EItemType slotItemType {get; set;} = EItemType.None;
    public EEquipType slotEquipType {get; set;} = EEquipType.None;
    public ItemData Data {get; protected set;}
    public int Stack {get; private set;}
    public int Durability {get; private set;} 
 

    public Func<ItemData, bool> slotCondition;
    public Action<ItemSlot> onChangeStack;
    public Action<int, ItemSlot> onAddItem;
    public Action<int> onUpdateSlot;



    public bool IsFull() => Stack >= MaxStack;
    public bool IsEmpty() => Stack <= 0;
    public virtual int MaxStack => Data.MaxStack;
    public virtual int MaxDurability => (int)Data.MaxDurability;  


    private int _slotIdx = -1;

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

    public ItemSlot(int slotIdx)
    {
        _slotIdx = slotIdx;
    }




    public void UseDurability(int amount)
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

    public virtual bool AddStack(ItemData itemData, int amount = 1, int durability = 0, bool isServer = false)
    { 
        if ((Data == null && itemData.Id != Data.Id) || !IsAddable(amount))
            return false; 
        
        Durability = durability;
        Data = itemData; 
        Stack += amount;

        if (Stack == 0) 
            Data = null;  
        
        if (_slotIdx != -1 && !isServer)  
            onUpdateSlot?.Invoke(_slotIdx);  

        onChangeStack?.Invoke(this);
        onAddItem?.Invoke(itemData.Id, this);
        return true;
    } 

    public virtual void Setup(ItemData itemData, int amount = 0, int durability = 0, bool isServer = false)
    {
        Stack = amount;
        Durability = durability;
        
        if (Data != null)
            onAddItem?.Invoke(Data.Id, this); 

        this.Data = itemData;  

        if (_slotIdx != -1 && !isServer) 
            onUpdateSlot?.Invoke(_slotIdx);    

        onChangeStack?.Invoke(this); 
    }
    
}
