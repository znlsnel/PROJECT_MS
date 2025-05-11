using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class QuestStorageSlot : ItemSlot
{
    private int targetAmount;
    public override int MaxStack => targetAmount;

    public QuestStorageSlot(int MaxAmount)
    {
        targetAmount = MaxAmount;
    }
}
