using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{
    private HashSet<GameObject> interactables = new HashSet<GameObject>();
    private SphereCollider sphereCollider;

    private string testKey = "UI/InteractionUI.prefab";
    private void Awake()
    {
        Managers.Input.SubscribeToInit(InitInput);
        Managers.Resource.SubscribeToInit(InitResource);

        sphereCollider = gameObject.GetOrAddComponent<SphereCollider>();
        sphereCollider.radius = 3f;
        sphereCollider.isTrigger = true;
    }

    private void InitResource()
    {
        Managers.Resource.LoadAsync<GameObject>(testKey);
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
        Managers.Resource.Release(testKey);
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
        {
            if (interactables.Count == 0)
                Managers.UI.OpenPopupUI<UIBase>(testKey);
            interactables.Add(other.gameObject);
        }

        
    }
 
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractable interactable))
        {
            interactables.Remove(other.gameObject); 

            if (interactables.Count == 0)
                Managers.UI.CloseUI<UIBase>(testKey);
        }
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
