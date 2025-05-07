using UnityEngine;

public class TestPlacementTrigger : MonoBehaviour
{
    public PlacementHandler placementSystem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            placementSystem.StartPlacement("DropItem/Consumable/TestCube.prefab");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            placementSystem.CancelPlacement();
        }
    }
}