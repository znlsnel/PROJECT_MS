using UnityEngine;

public abstract class ItemController : MonoBehaviour
{
    protected ItemSlot itemSlot;
    protected ItemData itemData;

    public void Setup(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        this.itemData = itemSlot.Data;
    }

    public abstract void OnAction();
}