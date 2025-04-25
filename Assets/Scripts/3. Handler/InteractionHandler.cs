using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{
    public Collider[] Interactables { get; private set; }
    [SerializeField] private float range = 3f;

    public event Action<GameObject> OnInteract;

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

    private void FixedUpdate()
    {
        Interactables = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Interactable"));

        Array.Sort(Interactables, (a, b) => {
            float distanceA = Vector3.SqrMagnitude(a.transform.position - transform.position);
            float distanceB = Vector3.SqrMagnitude(b.transform.position - transform.position);

            return distanceA.CompareTo(distanceB);
        });
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
        if(Interactables.Length == 0) return;

        if(Interactables[0].TryGetComponent(out IInteractable interactable))
        {
            GameObject interactedObject = interactable.Interact(gameObject); 
            OnInteract?.Invoke(interactedObject);
        }
    }
}
