using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class QuestStorageSlot : ItemSlot
{
    private int targetAmount;
    private bool isSuccess = false;
    public override int MaxStack => targetAmount;     
    public event Action onSuccess;
    

    public QuestStorageSlot(int MaxAmount, int slotIdx) : base(slotIdx)
    {
        targetAmount = MaxAmount; 
    }

    public void Init(ItemData itemData)
    {
        Data = itemData;
        Setup(itemData, 0);  
    }

    public override void Setup(ItemData itemData, int amount = 0, int durability = 0, bool isServer = false)
    {
        if (itemData.Id != Data.Id)
            return;

        base.Setup(itemData, amount, durability, isServer); 
        if (Stack >= targetAmount && !isSuccess) 
        {
            isSuccess = true; 
            onSuccess?.Invoke(); 
        }  
    }

    public override bool AddStack(ItemData itemData, int amount = 1, int durability = 0, bool isServer = false)
    {
        if (itemData.Id != Data.Id)
            return false;

        bool ret = base.AddStack(itemData, amount, durability, isServer); 
        if (Stack >= targetAmount && !isSuccess)
        {
            isSuccess = true; 
            onSuccess?.Invoke();
        }
        return ret;
    }
}
