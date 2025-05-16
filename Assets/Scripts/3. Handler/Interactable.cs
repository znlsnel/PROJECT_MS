using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [field: SerializeField] public AnimationClip interactAnimation { get; private set; }
    [field: SerializeField] public float interactAnimationSpeed { get; private set; } = 1f;

    public int AnimationIndex { get; private set; }

    public void Awake()
    {
        AnimationIndex = AnimationDataManager.AddAnimationClip(interactAnimation);
    }

    public abstract void Interact(GameObject obj);
}
