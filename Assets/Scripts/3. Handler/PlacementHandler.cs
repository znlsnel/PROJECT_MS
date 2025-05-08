using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class PlacementHandler : MonoBehaviour
{
    [Header("설정")]
    public LayerMask placementLayer; // 설치 가능한 표면 레이어
    public Material validMaterial; // 설치 가능 머티리얼
    public Material invalidMaterial; // 설치 불가 머티리얼
    public float rotateSpeed = 10f; // 회전 속도

    private GameObject previewObject; // 프리뷰 오브젝트
    private bool isPlacing = false; // 설치 중 여부
    private bool isValidPosition = false; // 현재 위치 설치 가능 여부
    private Renderer[] previewRenderers; // 프리뷰 렌더러 목록
    private Material[][] originalMaterials; // 원본 머티리얼 백업
    private string currentPrefabAddress; // 설치할 프리팹 주소
    private float yRotationOffset = 0f; // 수동 회전 각도

    void Update()
    {
        if (!isPlacing || previewObject == null)
            return;

        HandleRotationInput();
        UpdatePreview();

        if (Mouse.current.leftButton.wasPressedThisFrame && isValidPosition)
        {
            PlaceObject();
        }
    }

    public void StartPlacement(string prefabAddress) // 설치 시작 - 어드레서블 프리팹 주소로 로딩
    {
        if (isPlacing) CancelPlacement();

        currentPrefabAddress = prefabAddress;

        Addressables.InstantiateAsync(prefabAddress).Completed += handle =>
        {
            previewObject = handle.Result;
            previewRenderers = previewObject.GetComponentsInChildren<Renderer>();

            // 원래 머티리얼 백업
            originalMaterials = new Material[previewRenderers.Length][];
            for (int i = 0; i < previewRenderers.Length; i++)
            {
                originalMaterials[i] = previewRenderers[i].materials;
            }

            ApplyPreviewMaterial(invalidMaterial);
            DisableColliders(previewObject);
            isPlacing = true;
            yRotationOffset = 0f; // 초기화
        };
    }

    public void CancelPlacement() // 설치 취소
    {
        if (previewObject != null)
            Destroy(previewObject);

        isPlacing = false;
        previewObject = null;
        previewRenderers = null;
        originalMaterials = null;
        currentPrefabAddress = null;
    }

    void UpdatePreview() // 프리뷰 위치 및 색상 업데이트
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            previewObject.transform.position = hit.point;

            Quaternion surfaceRotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(Camera.main.transform.forward, hit.normal),
                hit.normal
            );
            Quaternion userRotation = Quaternion.Euler(0, yRotationOffset, 0);
            previewObject.transform.rotation = surfaceRotation * userRotation;

            // 설치 가능 레이어 체크
            bool isPlaceable = ((1 << hit.collider.gameObject.layer) & placementLayer) != 0;

            ApplyPreviewMaterial(isPlaceable ? validMaterial : invalidMaterial);
            isValidPosition = isPlaceable;
        }
        else
        {
            ApplyPreviewMaterial(invalidMaterial);
            isValidPosition = false;
        }
    }

    void HandleRotationInput() // 오브젝트 회전
    {
        float scrollDelta = Mouse.current.scroll.ReadValue().y;
        yRotationOffset += scrollDelta * rotateSpeed * Time.deltaTime;
    }

    void PlaceObject() // 설치
    {
        Vector3 position = previewObject.transform.position;
        Quaternion rotation = previewObject.transform.rotation;

        Addressables.InstantiateAsync(currentPrefabAddress, position, rotation);
        CancelPlacement();
    }

    void ApplyPreviewMaterial(Material mat) // 프리뷰 오브젝트에 머티리얼 일괄 적용
    {
        foreach (var renderer in previewRenderers)
        {
            Material[] mats = new Material[renderer.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = mat;
            }
            renderer.materials = mats;
        }
    }

    void DisableColliders(GameObject go) // 프리뷰 충돌 제거
    {
        foreach (var col in go.GetComponentsInChildren<Collider>())
            col.enabled = false;
    }
}