using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FishNet.Object;
using UnityEngine;


public static class ItemDragHandler
{
    private static readonly string MovingSlotKey = "UI/Inventory/MovingSlot.prefab";

    private static MovingSlotUI movingSlotUI;
    private static ItemSlot movingSlot;
    private static ItemSlotUI selectedSlotUI;
    
    public static bool SelectItemSlot(ItemSlotUI itemSlotUI)
    {   
        // 처음 선택한 경우
        if (movingSlot == null && itemSlotUI.ItemSlot.Data != null)
        {
            if (selectedSlotUI != null)
                selectedSlotUI.onDisable = null;

            selectedSlotUI = itemSlotUI;

            SelectSlot(itemSlotUI.ItemSlot); 
            SetupMovingSlot(movingSlot); 

            // itemSlotUI.onDisable += ()=> {
            //     if (movingSlotUI.gameObject.activeSelf)
            //     {
            //         selectedSlotUI.ItemSlot.Setup(movingSlot.Data, movingSlot.Stack); 
            //         CloseMovingSlot();  
            //     }
            // };
            return true;
        }

        return false;
    } 

    public static void SwapItem(ItemSlotUI targetSlotUI)
    {
        if (movingSlot == null)
            return;


        if (!movingSlot.CheckSlotCondition(targetSlotUI.ItemSlot.Data) 
            || !targetSlotUI.ItemSlot.CheckSlotCondition(movingSlot.Data))
            return;

        Inventory.SwapItem(movingSlot, targetSlotUI.ItemSlot); 

        // itemSlotUI가 비어있었을 경우
        if (movingSlot.Data == null)
            SetupMovingSlot(null);

        // 데이터의 교환이 일어난 경우
        else
        {
            SelectSlot(movingSlot);  
            SetupMovingSlot(movingSlot); 
        }

        return;
    }


    public static void MoveItem(ItemSlotUI targetSlotUI)
    {
        if (movingSlot == null)
            return;

        if (!targetSlotUI.ItemSlot.CheckSlotCondition(movingSlot.Data))
            return;

        int amount = movingSlot.Stack;

        if (targetSlotUI.ItemSlot.Data != null)
        {
            amount = Math.Min(amount, targetSlotUI.ItemSlot.MaxStack - targetSlotUI.ItemSlot.Stack);
        }

        int durability = movingSlot.Durability;
        int targetDurability = targetSlotUI.ItemSlot.Durability;

        targetSlotUI.ItemSlot.AddStack(movingSlot.Data, amount, durability);
        movingSlot.AddStack(movingSlot.Data, -amount, targetDurability);   

        if (movingSlot.Data == null)
            SetupMovingSlot(null);
    }

 
    public static void DropItem()
    {
        if (movingSlot == null || Managers.Player == null)
            return;

        GameObject dropItem = Managers.Resource.Instantiate(movingSlot.Data.DropPrefabPath);

        Vector3 forward = Managers.Player.transform.forward; 
        Vector3 pos = Managers.Player.transform.position + forward * 1.5f;

        dropItem.transform.position = pos;
        
        NetworkCommandSystem.Instance.RequestDropItem(dropItem.GetComponent<NetworkObject>(), pos, Quaternion.identity, movingSlot.Durability);
 

        movingSlot.AddStack(movingSlot.Data, -1);
        if (movingSlot.Stack <= 0)
            CloseMovingSlot(); 
    }

    private static void SelectSlot(ItemSlot itemSlot)
    {
        movingSlot = new ItemSlot(itemSlot);
        itemSlot.Setup(null); 
    }

    private static void CloseMovingSlot() => SetupMovingSlot(null);
    
    private static void SetupMovingSlot(ItemSlot itemSlot)
    {
        movingSlot = itemSlot;
        if (itemSlot == null)
        {
            if (movingSlotUI != null)
            { 
                movingSlotUI.Hide();
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
                movingSlotUI.SetItem(movingSlot);
                Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI, true); 
                
            });
        } 
        else 
        { 
            Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI, true);   
            movingSlotUI.SetItem(movingSlot);
        }
    }

}
