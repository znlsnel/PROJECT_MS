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
    

    public QuestStorageSlot(int MaxAmount)
    {
        targetAmount = MaxAmount;
    }

    public override bool AddStack(ItemData itemData, int amount = 1)
    {
        bool ret = base.AddStack(itemData, amount);
        if (Stack >= targetAmount && !isSuccess)
        {
            //isSuccess = true;
            onSuccess?.Invoke();
        }
        return ret;
    }
}
