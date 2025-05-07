using UnityEngine;

public abstract class ItemController : MonoBehaviour
{
    private ItemSlot itemSlot;
    protected ItemData itemData;

    public void Setup(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        this.itemData = itemSlot.Data;
    }

    public abstract void OnAction();
}
