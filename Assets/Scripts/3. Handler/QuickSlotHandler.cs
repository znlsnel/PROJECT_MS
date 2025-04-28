using UnityEngine;

public class QuickSlotHandler : MonoBehaviour
{
    private Storage quickSlotStorage;
    private void Awake()
    {
        quickSlotStorage = Managers.UserData.Inventory.QuickSlotStorage;
    }

}
