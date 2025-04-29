using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InteractionHandler : MonoBehaviour
{ 
    // HashSet을 사용하여 빠른 조회 및 중복 방지
    private HashSet<Interactable> uniqueInteractables = new HashSet<Interactable>();
    // List를 사용하여 순서 지정 접근 및 효율적인 정렬
    private List<Interactable> sortedInteractables = new List<Interactable>();
    private bool isDirty = true; // 필요할 경우 초기 정렬을 위해 dirty 상태로 시작
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        { 
            AddInteractable(interactable);
            Debug.Log($"Trigger Enter: {other.gameObject.name}"); // 일단 로그는 간단하게 유지
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Interactable interactable))
        {
            RemoveInteractable(interactable);
            Debug.Log($"Trigger Exit: {other.gameObject.name}"); // 일단 로그는 간단하게 유지
        }
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
        if (isDirty) SortInteractables();
        if(sortedInteractables.Count == 0) 
        {
            Debug.Log("No interactable");
            return;
        }
        onInputInteract?.Invoke();
    }

    public void OnInteract()
    {        
        if (isDirty) SortInteractables(); 
        if(sortedInteractables.Count == 0) return;

        Interactable interactable = sortedInteractables[0];
        interactable.Interact(gameObject);
        CheckInteractable(interactable).Forget();
    }

    public void AddInteractable(Interactable interactable)
    {
        // 중복 처리를 위해 HashSet에 먼저 추가 시도
        if (uniqueInteractables.Add(interactable))
        {
            // 성공적으로 추가되면(중복이 아니면) 리스트에 추가하고 정렬 대상으로 표시
            sortedInteractables.Add(interactable);
            isDirty = true; 
        }
    }

    public void RemoveInteractable(Interactable interactable)
    {
        // HashSet에서 먼저 제거 시도
        if (uniqueInteractables.Remove(interactable))
        {
            // 성공적으로 제거되면 리스트에서 제거하고 정렬 대상으로 표시
            sortedInteractables.Remove(interactable); // List.Remove는 O(n) 비용
            isDirty = true; 
        }
    }

    public Interactable GetInteractObject()
    {
        if (isDirty) SortInteractables(); 
        if(sortedInteractables.Count == 0) return null;

        return sortedInteractables[0];
    }

    public void SortInteractables()
    {
        // 외부에서 파괴되었을 수 있는 null 요소 제거
        // 두 컬렉션을 모두 확인해야 하지만, 정렬을 위해 리스트를 정리하는 것이 주 목적임
        int removedCount = sortedInteractables.RemoveAll(item => item == null);
        if (removedCount > 0)
        {
             // 리스트에서 null이 제거된 경우, 일관성을 위해 HashSet 재생성
             // 약간 비효율적이지만 두 컬렉션의 동기화를 보장함
             uniqueInteractables = new HashSet<Interactable>(sortedInteractables);
        }

        if(sortedInteractables.Count < 2) 
        {
             isDirty = false;
             return;
        }

        sortedInteractables.Sort((a, b) => {
            if (a == null && b == null) return 0;
            if (a == null) return 1;
            if (b == null) return -1;

            try
            {
                 float distanceA = Vector3.SqrMagnitude(a.transform.position - transform.position);
                 float distanceB = Vector3.SqrMagnitude(b.transform.position - transform.position);
                 return distanceA.CompareTo(distanceB);
            }
            catch (MissingReferenceException)
            {
                 // 정렬 비교 *중에* 객체가 파괴될 수 있는 경우 처리
                 // 파괴된 객체를 '멀리 있는' 것으로 취급하거나 적절히 처리
                 // 0을 반환하면 오류는 피할 수 있지만 논리적으로 맞지 않을 수 있음
                 // 이런 특정 경우가 발생하면 로깅 고려
                 Debug.LogError($"MissingReferenceException during sort comparison. a: {a?.name ?? "null"}, b: {b?.name ?? "null"}");
                 // 안전한 비교 결과 시도
                 if (a == null && b == null) return 0;
                 if (a == null) return 1; // null 또는 파괴된 객체를 맨 뒤로 배치
                 if (b == null) return -1;
                 return 0; // 둘 다 유효해 보이지만 오류를 발생시키는 경우의 대체 처리(드묾)
            }
        });

        isDirty = false;
    }

    async UniTaskVoid CheckInteractable(Interactable interactable)
    {
        await UniTask.WaitForEndOfFrame();

        if(interactable == null || !interactable.gameObject.activeSelf)
        {
            // 상호작용 가능한 객체가 유효하지 않으면 두 컬렉션 모두에서 제거 시도
            if (uniqueInteractables.Remove(interactable))
            {
                sortedInteractables.Remove(interactable);
                isDirty = true;
            }
        }
    }
}