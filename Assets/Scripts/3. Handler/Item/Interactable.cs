using FishNet.Object;
using UnityEngine;

public abstract class Interactable : NetworkBehaviour
{
    [field: SerializeField] public AnimationClip interactAnimation { get; private set; }
    [field: SerializeField] public float interactAnimationSpeed { get; private set; } = 1f;

    public abstract void Interact(GameObject obj);
}
