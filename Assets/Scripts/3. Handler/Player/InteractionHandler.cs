using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{ 
    private HashSet<Interactable> interactables = new HashSet<Interactable>();
    public event Action onInputInteract;

    private void Awake() 
    {
        Managers.SubscribeToInit(Init);
    }

    private void Init()
    {
        Managers.Input.Interact.started += InputInteract;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        { 
            interactables.Add(interactable); 
            MyDebug.Log($"Trigger Enter: {other.gameObject.name}"); // 일단 로그는 간단하게 유지
        }
    } 

    public virtual void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            interactables.Remove(interactable);
            MyDebug.Log($"Trigger Exit: {other.gameObject.name}"); // 일단 로그는 간단하게 유지
        }
    }

    private void OnDestroy()
    {
        if (Managers.IsNull) 
            return;
        
        Managers.Input.Interact.started -= InputInteract;
    }
    
    private void InputInteract(InputAction.CallbackContext context)
    {
        if(interactables.Count == 0) 
        {
            Debug.Log("No interactable");
            return;
        }

        onInputInteract?.Invoke();
    }

    public void OnInteract()
    {         

        Interactable interactable = GetInteractObject();  
        if(interactable == null) return;

        interactable.Interact(gameObject);
    }

 
    public Interactable GetInteractObject()
    {
        // 외부에서 파괴되었을 수 있는 null 요소 제거
        // 두 컬렉션을 모두 확인해야 하지만, 정렬을 위해 리스트를 정리하는 것이 주 목적임
        bool hasEmptySlot = GetComponent<AlivePlayer>().Inventory.HasEmptySlot();
        Interactable interactable = null;
        float dist = -1;

        interactables.ToList().ForEach(item => {
            
            if (item == null) 
                return;

            float d = Vector3.SqrMagnitude(item.transform.position - transform.position); 

            if ((interactable == null && item.isActive) ||  
                (d < dist && item.isActive))
            {
                if (item is ItemObject && hasEmptySlot == false)
                    return; 

                interactable = item;
                dist = d;
            }
        }); 

        return interactable;
    }
}