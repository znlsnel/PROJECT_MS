using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{
    private HashSet<GameObject> interactables = new HashSet<GameObject>();
    private SphereCollider sphereCollider;

    private void Awake()
    {
        Managers.Input.SubscribeToInit(InitInput);
        sphereCollider = gameObject.GetOrAddComponent<SphereCollider>();
        sphereCollider.radius = 3f;
        sphereCollider.isTrigger = true;
    }

    private void InitInput()
    {
        Managers.Input.Interact.started += InputInteract;
    }

    private void OnDestroy()
    {
        if (Managers.IsNull) 
            return;
        
        Managers.Input.Interact.started -= InputInteract;
    }
    
    private void InputInteract(InputAction.CallbackContext context)
    {
        IInteractable nearestInteractable = FindNearestInteractable();
        if(nearestInteractable != null)
            nearestInteractable.Interact(); 
    }
 

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable))
            interactables.Add(other.gameObject);
    }
 
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable))
            interactables.Remove(other.gameObject);
    }

    private IInteractable FindNearestInteractable()
    {
        IInteractable nearestInteractable = null;
        float nearestDistance = float.MaxValue;

        foreach (var interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if(distance < nearestDistance)
            {
                nearestInteractable = interactable.GetComponent<IInteractable>();
                nearestDistance = distance; 
            }
        }

        return nearestInteractable;
    }
}
