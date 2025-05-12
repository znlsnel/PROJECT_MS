using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlacementTrigger : MonoBehaviour
{
    public PlacementHandler placementSystem;
    public QuickSlotHandler quickSlotHandler;
    private bool isPlacementModeActive = false; // 배치 모드 상태 추적
    private ItemData currentItemData;

    private void Start()
    {
        quickSlotHandler.onSelectItem += (itemData) =>
        {
            currentItemData = itemData;
        };

        placementSystem.OnPlacementComplete += () => isPlacementModeActive = false;
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPlacementModeActive)
            {
                if (currentItemData != null && currentItemData.ItemType == EItemType.Placeable)
                {
                    placementSystem.StartPlacement(currentItemData.PrefabPath);
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