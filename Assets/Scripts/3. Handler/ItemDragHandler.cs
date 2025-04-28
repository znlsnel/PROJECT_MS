using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class ItemDragHandler
{
    private static readonly string MovingSlotKey = "UI/Inventory/MovingSlot.prefab";
    private static MovingSlotUI movingSlotUI;

    private static ItemSlotUI selectedItemSlotUI;
    private static ItemData itemData;
    private static int amount;

    public static void SelectItemSlot(ItemSlotUI itemSlotUI)
    {   

        if (itemSlotUI == null)
        {
            if (selectedItemSlotUI != null)
            {
                // TODO 아이템 버리기
            }
            return;  
        }

        // 이미 선택한 슬롯이 있는 경우
        if (selectedItemSlotUI != null)
        { 
            selectedItemSlotUI.ItemSlot.Setup(itemData);
            selectedItemSlotUI.ItemSlot.ModifyStack(itemData, amount); 

            // 같은 슬롯을 클릭한게 아니라면
            if (selectedItemSlotUI.ItemSlot.Data != itemSlotUI.ItemSlot.Data)
            {
                Inventory.SwapItem(selectedItemSlotUI.ItemSlot, itemSlotUI.ItemSlot); 

                // itemSlotUI가 비어있었을 경우
                if (selectedItemSlotUI.ItemSlot.Data == null)
                    SetupSelectedItemSlot(null);

                // 데이터의 교환이 일어난 경우
                else
                {
                    ClearSlot(selectedItemSlotUI); 
                    SetupSelectedItemSlot(selectedItemSlotUI); 
                }
            }
            else
            {
                SetupSelectedItemSlot(null);
            }
                
        } 

        // 데이터가 있는 슬롯을 선택했을 경우
        else if (itemSlotUI.ItemSlot.Data != null)
        {
            ClearSlot(itemSlotUI); 
            SetupSelectedItemSlot(itemSlotUI);
        }
    } 

    private static void ClearSlot(ItemSlotUI itemSlotUI)
    {
        itemData = new ItemData(itemSlotUI.ItemSlot.Data);
        amount = itemSlotUI.ItemSlot.Stack;

        itemSlotUI.ItemSlot.Setup(null); 
    }

    private static void SetupSelectedItemSlot(ItemSlotUI itemSlotUI)
    {
        selectedItemSlotUI = itemSlotUI;
        if (itemSlotUI == null)
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
                movingSlotUI.SetItem(itemData, amount);
                Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI);
            });
        } 
        else 
        {
            Managers.UI.ShowPopupUI<MovingSlotUI>(movingSlotUI); 
            movingSlotUI.SetItem(itemData, amount);
        }
    }

}
