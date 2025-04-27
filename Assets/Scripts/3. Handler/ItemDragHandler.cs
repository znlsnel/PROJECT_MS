using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class ItemDragHandler : MonoBehaviour
{
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

            if (selectedItemSlotUI.ItemSlot.Data != itemSlotUI.ItemSlot.Data)
            {
                Inventory.SwapItem(selectedItemSlotUI.ItemSlot, itemSlotUI.ItemSlot); 

                // itemSlotUI가 비어있는 경우
                if (selectedItemSlotUI.ItemSlot.Data == null)
                
                    selectedItemSlotUI = null;
                else
                {
                    ClearSlot(selectedItemSlotUI); 
                }
            }
            else
            {
                selectedItemSlotUI = null;
            }
                
        }

        // 데이터가 있는 슬롯을 선택했을 경우
        else if (itemSlotUI.ItemSlot.Data != null)
        {
            selectedItemSlotUI = itemSlotUI;

            ClearSlot(selectedItemSlotUI);
        }
    } 

    private static void ClearSlot(ItemSlotUI itemSlotUI)
    {
        itemData = new ItemData(itemSlotUI.ItemSlot.Data);
        amount = itemSlotUI.ItemSlot.Stack;

        selectedItemSlotUI.ItemSlot.Setup(null);
    }
}
