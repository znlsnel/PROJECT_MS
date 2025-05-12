using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using FishNet.Object;
using System;
using System.Collections.Generic;

public class PlacementHandler : MonoBehaviour
{
    [Header("설정")]
    public LayerMask placementLayer; // 배치 가능 레이어
    public Material validMaterial; // 유효 머티리얼
    public Material invalidMaterial; // 유효하지 않은 머티리얼
    public float rotateSpeed = 10f; // 마우스 휠 회전 속도

    [Header("무시 조건")]
    public LayerMask ignoredLayers; // 무시 레이어
    public string[] ignoredTags; // 무시 태그

    private GameObject previewObject; // 미리보기 오브젝트
    private bool isPlacing = false; // 배치 모드 여부
    private bool isValidPosition = false; // 유효 위치 여부
    private Renderer[] previewRenderers; // 미리보기 렌더러 목록
    private Material[][] originalMaterials; // 원본 머티리얼 백업
    private string currentPrefabAddress; // 배치할 프리팹 주소
    private float yRotationOffset = 0f; // 수동 회전값

    private Camera mainCamera;
    private Dictionary<Renderer, Material[]> materialCache = new Dictionary<Renderer, Material[]>();
    public event Action OnPlacementComplete; // 배치 완료 이벤트

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!isPlacing || previewObject == null) return;

        float scrollDelta = Mouse.current.scroll.ReadValue().y;  // 마우스 휠 회전
        yRotationOffset += scrollDelta * rotateSpeed * Time.deltaTime;

        UpdatePreview();

        if (Mouse.current.leftButton.wasPressedThisFrame && isValidPosition)
        {
            Vector3 position = previewObject.transform.position;
            Quaternion rotation = previewObject.transform.rotation;
            Addressables.InstantiateAsync(currentPrefabAddress, position, rotation);
            CancelPlacement();

            OnPlacementComplete?.Invoke(); // 배치 완료 이벤트 호출
        }
    }

    public void StartPlacement(string prefabAddress) // 오브젝트 배치
    {
        if (isPlacing) CancelPlacement(); // 중복 방지

        currentPrefabAddress = prefabAddress; // 프리팹 주소 저장

        Addressables.InstantiateAsync(prefabAddress).Completed += handle =>
        {
            previewObject = handle.Result;

            previewObject.SetActive(true);

            previewRenderers = previewObject.GetComponentsInChildren<Renderer>();

            originalMaterials = new Material[previewRenderers.Length][];
            for (int i = 0; i < previewRenderers.Length; i++)
            {
                originalMaterials[i] = previewRenderers[i].materials;
            }

            ApplyPreviewMaterial(invalidMaterial);
            DisableColliders(previewObject);
            isPlacing = true;
            yRotationOffset = 0f;
        };
    }

    void UpdatePreview() // 위치 및 회전 업데이트
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ApplyPreviewMaterial(invalidMaterial);
            isValidPosition = false;
            return;
        }

        previewObject.transform.position = hit.point;
        Quaternion surfaceRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(mainCamera.transform.forward, hit.normal),
            hit.normal
        );
        Quaternion userRotation = Quaternion.Euler(0, yRotationOffset, 0);
        previewObject.transform.rotation = surfaceRotation * userRotation;

        if (!CanPlaceOnHit(hit, out string reason))
        {
            ApplyPreviewMaterial(invalidMaterial);
            isValidPosition = false;
            return;
        }

        ApplyPreviewMaterial(validMaterial);
        isValidPosition = true;
    }

    bool CanPlaceOnHit(RaycastHit hit, out string reason) // 배치 가능 여부
    {
        GameObject hitObject = hit.collider.gameObject;
        var rules = previewObject.GetComponent<PlacementCheck>();

        if (((1 << hitObject.layer) & placementLayer) == 0) // 배치 레이어
        {
            reason = "배치 불가능 레이어";
            return false;
        }

        if (((1 << hitObject.layer) & ignoredLayers) != 0) // 무시 레이어
        {
            reason = "무시 레이어";
            return false;
        }

        if (Array.Exists(ignoredTags, tag => hitObject.CompareTag(tag))) // 무시 태그
        {
            reason = "무시 태그";
            return false;
        }

        if (rules.slopeLimit) // 경사도
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > rules.maxSlopeAngle)
            {
                reason = "가파른 경사";
                return false;
            }
        }

        if (rules.overlapCheck) // 중첩
        {
            Bounds bounds = GetBounds(previewObject);
            Collider[] overlaps = Physics.OverlapBox(bounds.center, bounds.extents * 0.9f,
                previewObject.transform.rotation, ~0, QueryTriggerInteraction.Ignore);

            if (overlaps.Length > 0)
            {
                reason = "중첩";
                return false;
            }
        }

        reason = null;
        return true;
    }

    public void CancelPlacement() // 배치 취소
    {
        if (previewObject != null)
            Destroy(previewObject);

        isPlacing = false;
        previewObject = null;
        previewRenderers = null;
        originalMaterials = null;
        currentPrefabAddress = null;
    }

    void ApplyPreviewMaterial(Material mat) // 미리보기 오브젝트 머티리얼 변경
    {
        foreach (var renderer in previewRenderers)
        {
            if (!materialCache.ContainsKey(renderer))
            {
                materialCache[renderer] = new Material[renderer.materials.Length];
            }

            for (int i = 0; i < materialCache[renderer].Length; i++)
                materialCache[renderer][i] = mat;

            renderer.materials = materialCache[renderer];
        }
    }

    void DisableColliders(GameObject go) // 미리보기 충돌 방지
    {
        foreach (var col in go.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    Bounds GetBounds(GameObject go) // 오브젝트 바운드 계산
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(go.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }
}