using UnityEngine;
using UnityEngine.InputSystem;

public class UIHandler : MonoBehaviour
{
    private InventoryUI inventory; 

    private void Awake() 
    {
        Managers.onChangePlayer += Setup;
    }

    private void OnDestroy()
    {
        Managers.onChangePlayer -= Setup;
    }
 
    private void Setup(AlivePlayer player)
    {
        if (player != gameObject.GetComponent<AlivePlayer>()) 
            return;

        inventory = Managers.Resource.Instantiate("UI/Inventory/Inventory.prefab").GetComponent<InventoryUI>();
        inventory.Setup(player); 
        inventory.Hide();

        Managers.Input.GetInput(EPlayerInput.Inventory).started += ToggleInventory; 
        Managers.Input.GetInput(EPlayerInput.TurnOffPopup).started += TurnOffPopup; 
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (!inventory.IsOpen) 
            Managers.UI.ShowPopupUI<InventoryUI>(inventory);     
        else
            inventory.Close();    
    }

    private void TurnOffPopup(InputAction.CallbackContext context) 
    {
        Managers.UI.ClosePopupUI();
    }
 
}
