using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ItemController : MonoBehaviour
{
    public ItemSlot itemSlot {get; private set;}
    public ItemData itemData {get; private set;}

    public AlivePlayer Owner {get; private set;}
    public virtual void Setup(AlivePlayer owner, ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        this.itemData = itemSlot.Data;
        Owner = owner;
    }

    public abstract void OnAction();
}