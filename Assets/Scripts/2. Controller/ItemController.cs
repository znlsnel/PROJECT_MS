using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ItemController : MonoBehaviour
{
    public ItemSlot itemSlot {get; private set;}
    public ItemData itemData {get; private set;}

    protected AlivePlayer _owner;
    public virtual void Setup(AlivePlayer owner, ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        this.itemData = itemSlot.Data;
        _owner = owner;

    }

    public abstract void OnAction();
}