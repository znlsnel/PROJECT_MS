using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public int ItemId { get; private set; }


    public GameObject Interact(GameObject player)
    {
        Managers.Quest.ReceiveReport(ETaskCategory.Pickup, ItemId);
        Destroy(gameObject);
        return null;
    } 

    public void Setup(int itemId)
    {
        ItemId = itemId;
    } 
}
