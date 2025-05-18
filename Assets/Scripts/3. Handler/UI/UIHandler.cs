using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    private InventoryUI inventory; 
    private ItemPickupUI itemPickup;

    private void Awake()
    {
        inventory = Managers.Resource.Instantiate("UI/Inventory/Inventory.prefab").GetComponent<InventoryUI>();
        inventory.Hide();

        itemPickup = Managers.Resource.Instantiate("UI/Inventory/ItemPickupUI.prefab").GetComponent<ItemPickupUI>();
        itemPickup.Hide();

        Managers.Input.GetInput(EPlayerInput.Inventory).started += ToggleInventory; 
        Managers.Input.GetInput(EPlayerInput.TurnOffPopup).started += TurnOffPopup;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        Managers.UI.ShowPopupUI<InventoryUI>(inventory);     
    }

    private void TurnOffPopup(InputAction.CallbackContext context) 
    {
        Managers.UI.ClosePopupUI();
    }
 
}
