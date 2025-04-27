using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishNet.Component.Prediction;

public class ItemSlot
{
    public ItemData Data {get; private set;}
    public int Stack {get; private set;}

    public Action<ItemSlot> onChangeStack;
 
    public int MaxStack => Data.MaxStack;
    public bool IsFull() => Stack >= MaxStack;
    public bool IsEmpty() => Stack <= 0;
    public bool IsAddable(int amount = 1) => Data != null && Stack + amount <= MaxStack;


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


    /// <summary>
    /// 현재 아이템 Stack에 Amount를 더함
    /// </summary>
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
