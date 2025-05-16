using System;
using TMPro;
using UnityEngine;

public class QuestStorageSlotUI : InventorySlotUI
{
    [SerializeField] private TextMeshProUGUI maxAmountText;

    public Action onSuccess;
    private bool isSuccess = false;

    public override void UnSetup()
    {
        base.UnSetup();
        onSuccess = null;
        isSuccess = false;
    }

    public override void UpdateSlotState(ItemSlot itemSlot)
    {
        base.UpdateSlotState(itemSlot);

        itemAmountText.gameObject.SetActive(true);
        itemAmountText.text = itemSlot.Stack.ToString(); 
        
        maxAmountText.text = itemSlot.MaxStack.ToString();

        if (itemSlot.Stack >= itemSlot.MaxStack)
        {
            if (isSuccess)
                return;
            isSuccess = true; 
            onSuccess?.Invoke();
        }
    }

    protected override void ClickAction()
    {
        ItemDragHandler.MoveItem(this);
    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }
}
