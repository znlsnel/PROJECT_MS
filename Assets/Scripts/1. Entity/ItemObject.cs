using UnityEngine;

public class ItemObject : Interactable
{
    public override void Interact(GameObject obj)
    {
        Destroy(gameObject);
    }
}
