using System;
using TMPro;
using UnityEngine;

public class QuestStorageSlotUI : ItemSlotUI
{
    private static readonly string _clickSound = "Sound/UI/Click_01.mp3";
    [SerializeField] private TextMeshProUGUI maxAmountText; 

    public override void UpdateSlotState(ItemSlot itemSlot)
    {
        base.UpdateSlotState(itemSlot);

        itemAmountText.gameObject.SetActive(true);
        itemAmountText.text = itemSlot.Stack.ToString(); 
        maxAmountText.text = itemSlot.MaxStack.ToString();
    }

    protected override void ClickAction()
    {
        ItemDragHandler.MoveItem(this);
        Managers.Sound.Play(_clickSound); 
    }
}
