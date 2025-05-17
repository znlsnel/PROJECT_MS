using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlacementTrigger : MonoBehaviour
{
    public PlacementHandler placementSystem;
    public QuickSlotHandler quickSlotHandler;
    private bool isPlacementModeActive = false;
    private ItemSlot currentItemSlot;

    private void Start()
    {
        QuickSlotHandler.onSelectItem += (itemSlot, selectedItemObject) =>
        {
            currentItemSlot = itemSlot;
        };

        placementSystem.OnPlacementComplete += () => isPlacementModeActive = false;
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPlacementModeActive)
            {
                if (currentItemSlot != null && currentItemSlot.Data != null && 
                    currentItemSlot.Data.ItemType == EItemType.Placeable)
                {
                    placementSystem.StartPlacement(currentItemSlot.Data.PrefabPath);
                    isPlacementModeActive = true;
                }
            }
            else
            {
                placementSystem.CancelPlacement();
                isPlacementModeActive = false;
            }
        }
    }
}