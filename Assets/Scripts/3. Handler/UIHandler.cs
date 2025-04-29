using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    private PopupUI inventory; 

    private void Awake()
    {
        inventory = Managers.Resource.Instantiate("UI/Inventory/Inventory.prefab").GetComponent<PopupUI>();
        inventory.Hide();

        Managers.Input.GetInput(EPlayerInput.Inventory).started += ToggleInventory;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (inventory.IsOpen)
            inventory.Hide();
        else
            inventory.Show(); 
    }
 
}
