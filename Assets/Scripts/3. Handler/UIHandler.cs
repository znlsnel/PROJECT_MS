using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    private InventoryUI inventory; 

    private void Awake()
    {
        inventory = Managers.Resource.Instantiate("UI/Inventory/Inventory.prefab").GetComponent<InventoryUI>();
        inventory.Hide();

        Managers.Input.GetInput(EPlayerInput.Inventory).started += ToggleInventory;
        Managers.Input.GetInput(EPlayerInput.TurnOffPopup).started += TurnOffPopup;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (inventory.IsOpen)
            Managers.UI.ClosePopupUI(inventory); 
        else
            Managers.UI.ShowPopupUI<InventoryUI>(inventory);    
    }

    private void TurnOffPopup(InputAction.CallbackContext context)
    {
        Managers.UI.ClosePopupUI();
    }
 
}
