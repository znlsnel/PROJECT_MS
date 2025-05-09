using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class PlacementHandler : MonoBehaviour
{
    [Header("설정")]
    public LayerMask placementLayer; // 배치 가능 레이어
    public Material validMaterial; // 유효 머티리얼
    public Material invalidMaterial; // 유효하지 않은 머티리얼
    public float rotateSpeed = 10f; // 마우스 휠 회전 속도

    [Header("무시할 조건")]
    public LayerMask ignoredLayers; // 무시 레이어
    public string[] ignoredTags; // 무시 태그

    private GameObject previewObject; // 미리보기 오브젝트
    private bool isPlacing = false; // 배치 모드 여부
    private bool isValidPosition = false; // 유효 위치 여부
    private Renderer[] previewRenderers; // 미리보기 렌더러 목록
    private Material[][] originalMaterials; // 원본 머티리얼 백업
    private string currentPrefabAddress; // 배치할 프리팹 주소
    private float yRotationOffset = 0f; // 수동 회전값

    void Update()
    {
        if (!isPlacing || previewObject == null) return;

        HandleRotationInput(); // 마우스 휠 입력 처리
        UpdatePreview(); // 미리보기 위치 및 상태 업데이트

        if (Mouse.current.leftButton.wasPressedThisFrame && isValidPosition)
        {
            PlaceObject();
        }
    }

    public void StartPlacement(string prefabAddress) // 오브젝트 배치
    {
        if (isPlacing) CancelPlacement(); // 중복 방지

        currentPrefabAddress = prefabAddress; // 프리팹 주소 저장

        // 프리팹 로드 및 인스턴스화
        Addressables.InstantiateAsync(prefabAddress).Completed += handle =>
        {
            previewObject = handle.Result;
            previewRenderers = previewObject.GetComponentsInChildren<Renderer>();

            originalMaterials = new Material[previewRenderers.Length][]; // 원본 머티리얼 저장
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

    public void CancelPlacement()// 배치 취소
    {
        if (previewObject != null)
            Destroy(previewObject);

        isPlacing = false;
        previewObject = null;
        previewRenderers = null;
        originalMaterials = null;
        currentPrefabAddress = null;
    }

    void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());// 미리보기 오브젝트 위치 상태 업데이트

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject hitObject = hit.collider.gameObject;

            previewObject.transform.position = hit.point; // 미리보기 위치 설정

            Quaternion surfaceRotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(Camera.main.transform.forward, hit.normal),
                hit.normal
            );
            Quaternion userRotation = Quaternion.Euler(0, yRotationOffset, 0);
            previewObject.transform.rotation = surfaceRotation * userRotation;

            // 배치 가능 여부 검사
            bool isIgnoredLayer = ((1 << hitObject.layer) & ignoredLayers) != 0;
            bool isIgnoredTag = System.Array.Exists(ignoredTags, tag => hitObject.CompareTag(tag));
            bool isPlaceable = ((1 << hitObject.layer) & placementLayer) != 0;

            // 경사도 검사
            var rules = previewObject.GetComponent<PlacementCheck>();
            bool slopeOkay = true;
            if (rules == null || rules.enforceSlopeLimit)
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                float maxSlope = (rules != null) ? rules.maxSlopeAngle : 20f;
                slopeOkay = slopeAngle <= maxSlope;
            }

            // 중첩 검사
            bool overlapOkay = true;
            if (rules == null || rules.enforceOverlapCheck)
            {
                Bounds bounds = GetBounds(previewObject);
                Collider[] overlaps = Physics.OverlapBox(bounds.center, bounds.extents * 0.9f, 
                    previewObject.transform.rotation, ~0, QueryTriggerInteraction.Ignore);
                overlapOkay = overlaps.Length == 0;
            }

            // 최종 배치 가능 여부 결정
            if (isIgnoredLayer || isIgnoredTag || !isPlaceable || !slopeOkay || !overlapOkay)
            {
                ApplyPreviewMaterial(invalidMaterial); // 빨간색 머터리얼
                isValidPosition = false; //  불가능
            }
            else
            {
                ApplyPreviewMaterial(validMaterial); // 녹색 머터리얼
                isValidPosition = true; // 가능
            }
        }
        else
        {
            ApplyPreviewMaterial(invalidMaterial); // 빨간색 머터리얼
            isValidPosition = false; // 불가능
        }
    }

    void HandleRotationInput() // 오브젝트 회전
    {
        float scrollDelta = Mouse.current.scroll.ReadValue().y;
        yRotationOffset += scrollDelta * rotateSpeed * Time.deltaTime;
    }

    void PlaceObject() // 배치
    {
        Vector3 position = previewObject.transform.position;
        Quaternion rotation = previewObject.transform.rotation;
        Addressables.InstantiateAsync(currentPrefabAddress, position, rotation);
        CancelPlacement();
    }

    void ApplyPreviewMaterial(Material mat) // 미리보기 오브젝트 머티리얼 변경
    {
        foreach (var renderer in previewRenderers)
        {
            Material[] mats = new Material[renderer.materials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = mat;

            renderer.materials = mats;
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