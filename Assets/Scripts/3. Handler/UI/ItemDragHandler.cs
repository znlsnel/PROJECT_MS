using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishNet.Object;
using UnityEngine;


public static class ItemDragHandler
{
    private static readonly string MovingSlotKey = "UI/Inventory/MovingSlot.prefab";

    private static MovingSlotUI movingSlotUI;
    private static ItemSlot selectedItemSlot;
    
    public static bool SelectItemSlot(ItemSlotUI itemSlotUI)
    {   
        // 처음 선택한 경우
        if (selectedItemSlot == null && itemSlotUI.ItemSlot.Data != null)
        {
            SelectSlot(itemSlotUI.ItemSlot); 
            SetupMovingSlot(selectedItemSlot); 
            return true;
        }

        return false;
    } 

    public static void SwapItem(ItemSlotUI targetSlotUI)
    {
        if (selectedItemSlot == null)
            return;


        if (!selectedItemSlot.CheckSlotCondition(targetSlotUI.ItemSlot.Data) 
            || !targetSlotUI.ItemSlot.CheckSlotCondition(selectedItemSlot.Data))
            return;

            Inventory.SwapItem(selectedItemSlot, targetSlotUI.ItemSlot); 

            // itemSlotUI가 비어있었을 경우
            if (selectedItemSlot.Data == null)
                SetupMovingSlot(null);

            // 데이터의 교환이 일어난 경우
            else
            {
                SelectSlot(selectedItemSlot);  
                SetupMovingSlot(selectedItemSlot); 
            }

        return;
    }

    public static void MoveItem(ItemSlotUI targetSlotUI)
    {
        if (selectedItemSlot == null)
            return;

        if (!targetSlotUI.ItemSlot.CheckSlotCondition(selectedItemSlot.Data))
            return;

        int amount = selectedItemSlot.Stack;
        if (targetSlotUI.ItemSlot.Data != null)
        {
            amount = Math.Min(amount, targetSlotUI.ItemSlot.MaxStack - targetSlotUI.ItemSlot.Stack);
        }

        targetSlotUI.ItemSlot.AddStack(selectedItemSlot.Data, amount);
        selectedItemSlot.AddStack(selectedItemSlot.Data, -amount);

        if (selectedItemSlot.Data == null)
            SetupMovingSlot(null);
    }


    public static void DropItem()
    {
        if (selectedItemSlot == null || Managers.Player == null)
            return;

        GameObject dropItem = Managers.Resource.Instantiate(selectedItemSlot.Data.DropPrefabPath);

        Vector3 forward = Managers.Player.transform.forward;
        Vector3 pos = Managers.Player.transform.position + forward * 1.5f;

        dropItem.transform.position = pos;
        
        NetworkCommandSystem.Instance.RequestSpawnObject(dropItem.GetComponent<NetworkObject>(), pos, Quaternion.identity);

        dropItem.GetOrAddComponent<Rigidbody>().AddForce(Vector3.Lerp(forward, Vector3.up, 0.5f) * 5f, ForceMode.Impulse);

        selectedItemSlot.AddStack(selectedItemSlot.Data, -1);
        if (selectedItemSlot.Stack <= 0)
            CloseMovingSlot(); 
    }

    private static void SelectSlot(ItemSlot itemSlot)
    {
        selectedItemSlot = new ItemSlot(itemSlot);
        itemSlot.Setup(null); 
    }

    private static void CloseMovingSlot() => SetupMovingSlot(null);
    private static void SetupMovingSlot(ItemSlot itemSlot)
    {
        selectedItemSlot = itemSlot;
        if (itemSlot == null)
        {
            if (movingSlotUI != null)
            { 
                Managers.UI.ClosePopupUI(movingSlotUI);
                movingSlotUI = null;
            } 
            return;
        }


        if (movingSlotUI == null)
        {

            Managers.Resource.LoadAsync<GameObject>(MovingSlotKey, (prefab) =>
            {
                GameObject go = GameObject.Instantiate(prefab); 
                movingSlotUI = go.GetComponent<MovingSlotUI>();
                movingSlotUI.SetItem(selectedItemSlot);
                Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI);
            });
        } 
        else 
        {
            Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI);   
            movingSlotUI.SetItem(selectedItemSlot);
        }
    }

}
