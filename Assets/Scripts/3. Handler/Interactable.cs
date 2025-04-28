using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [field: SerializeField] public AnimationClip interactAnimation { get; private set; }
    [field: SerializeField] public float interactAnimationSpeed { get; private set; } = 1f;

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out InteractionHandler interactionHandler))
        {
            interactionHandler.AddInteractable(this);
            Debug.Log("Trigger Enter");
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out InteractionHandler interactionHandler))
        {
            interactionHandler.RemoveInteractable(this);
            Debug.Log("Trigger Exit");
        }
    }

    public virtual void OnDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Player"));
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out InteractionHandler interactionHandler))
            {
                interactionHandler.RemoveInteractable(this);
            }
        }
    }

    public abstract void Interact(GameObject obj);
}
