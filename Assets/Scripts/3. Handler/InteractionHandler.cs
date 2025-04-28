using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{ 
    private List<Interactable> interactables = new List<Interactable>();
    public event Action onInputInteract;

    private string testKey = "UI/InteractionUI.prefab";
    private void Awake()
    {
        Managers.SubscribeToInit(Init);
    }

    private void Init()
    {
        Managers.Resource.LoadAsync<GameObject>(testKey);
        Managers.Input.Interact.started += InputInteract;
    }

    private void OnDestroy()
    {
        if (Managers.IsNull) 
            return;
        
        Managers.Input.Interact.started -= InputInteract;
        Managers.Resource.Release(testKey);
    }
    
    private void InputInteract(InputAction.CallbackContext context)
    {
        if(interactables.Count == 0) return;

        onInputInteract?.Invoke();
    }

    public void OnInteract()
    {        
        if(interactables.Count == 0) return;

        Interactable interactable = interactables[0];
        RemoveInteractable(interactable);
        interactable.Interact(gameObject);
    }

    public void AddInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
        SortInteractables();
    }

    public void RemoveInteractable(Interactable interactable)
    {
        interactables.Remove(interactable);
        SortInteractables();
    }

    public Interactable GetInteractObject()
    {
        if(interactables.Count == 0) return null;

        return interactables[0];
    }

    public void SortInteractables()
    {
        if(interactables.Count < 2) return;

        interactables.Sort((a, b) => {
            float distanceA = Vector3.SqrMagnitude(a.transform.position - transform.position);
            float distanceB = Vector3.SqrMagnitude(b.transform.position - transform.position);

            return distanceA.CompareTo(distanceB);
        });
    }
}