using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlacementTrigger : MonoBehaviour
{
    public PlacementHandler placementSystem;
    public QuickSlotHandler quickSlotHandler;
    private bool isPlacementModeActive = false; // 배치 모드 상태 추적

    private void Start()
    {
        quickSlotHandler.onSelectItem += (itemData) =>
        {
            placementSystem.StartPlacement(itemData.PrefabPath);
        };

        placementSystem.OnPlacementComplete += () => isPlacementModeActive = false;
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (!isPlacementModeActive)
            {
                string prefabPath = "DropItem/Consumable/TestCube.prefab";
                placementSystem.StartPlacement(prefabPath);
                isPlacementModeActive = true;
            }
            else
            {
                placementSystem.CancelPlacement();
                isPlacementModeActive = false;
            }
        }
    }

    // void Update()
    // {
    //     if (Keyboard.current.pKey.wasPressedThisFrame)
    //     {
    //         if (isPlacementModeActive)
    //         {
    //             placementSystem.CancelPlacement();
    //             isPlacementModeActive = false;
    //         }
    //     }
    // }
}