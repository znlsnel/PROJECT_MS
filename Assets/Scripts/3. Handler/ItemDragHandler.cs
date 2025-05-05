using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public static class ItemDragHandler
{
    private static readonly string MovingSlotKey = "UI/Inventory/MovingSlot.prefab";

    private static MovingSlotUI movingSlotUI;
    private static ItemSlot selectedItemSlot;
    
    public static void SelectItemSlot(ItemSlotUI itemSlotUI)
    {   
        // 이미 선택한 슬롯이 있는 경우
        if (selectedItemSlot != null) 
        { 
            if (!selectedItemSlot.CheckSlotCondition(itemSlotUI.ItemSlot.Data) 
            || !itemSlotUI.ItemSlot.CheckSlotCondition(selectedItemSlot.Data))
                return;

            Inventory.SwapItem(selectedItemSlot, itemSlotUI.ItemSlot); 

            // itemSlotUI가 비어있었을 경우
            if (selectedItemSlot.Data == null)
                SetupMovingSlot(null);

            // 데이터의 교환이 일어난 경우
            else
            {
                SelectSlot(selectedItemSlot);  
                SetupMovingSlot(selectedItemSlot); 
            }
        } 

        // 처음 선택한 경우
        else if (itemSlotUI.ItemSlot.Data != null)
        {
            SelectSlot(itemSlotUI.ItemSlot); 
            SetupMovingSlot(selectedItemSlot); 
        }
    } 

    public static void DropItem()
    {
        // TODO
        // 아이템 버리기 로직
    }

    private static void SelectSlot(ItemSlot itemSlot)
    {
        selectedItemSlot = new ItemSlot(itemSlot);
        itemSlot.Setup(null); 
    }

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
                movingSlotUI.SetItem(selectedItemSlot.Data, selectedItemSlot.Stack);
                Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI);
            });
        } 
        else 
        {
            Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI);  
            movingSlotUI.SetItem(selectedItemSlot.Data, selectedItemSlot.Stack);
        }
    }

}
