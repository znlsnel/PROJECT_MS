using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    [field: SerializeField] public AnimationClip interactAnimation { get; private set; }

    public GameObject Interact(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out AlivePlayer alivePlayer))
        {
            alivePlayer.SetInteractAnimation(interactAnimation);
        }
        return gameObject;
    }
}
