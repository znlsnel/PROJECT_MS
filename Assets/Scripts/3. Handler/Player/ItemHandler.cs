using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmplifyShaderEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHandler : MonoBehaviour
{
    public static event Action<ItemController> onAction;

    private ItemController currentItemController;
    private ItemSlot currentItemSlot;

    private AlivePlayer _owner;
    private void Awake()
    {
        _owner = GetComponent<AlivePlayer>();

        Managers.SubscribeToInit(()=>
        {
            QuickSlotHandler.onSelectItem += SelectItem;
            Managers.Input.LeftMouse.started += InputUse;
        });
    }

    private void OnDisable()
    {
        QuickSlotHandler.onSelectItem -= SelectItem;
        Managers.Input.LeftMouse.started -= InputUse;
    }

    private void SelectItem(ItemSlot itemSlot, GameObject selectedItemObject)
    {
        if(itemSlot.Data == null)
        {
            currentItemController = null;
            currentItemSlot = null;
            return;
        }

        currentItemSlot = itemSlot;
        if (selectedItemObject != null)
        {
            currentItemController = selectedItemObject.GetComponent<ItemController>();
            currentItemController.Setup(_owner, itemSlot);
        }
    }

    private void InputUse(InputAction.CallbackContext context)
    {
        if(currentItemController == null || currentItemSlot.Stack <= 0)
            return;

        currentItemController.OnAction();
        onAction?.Invoke(currentItemController);
    }

}

