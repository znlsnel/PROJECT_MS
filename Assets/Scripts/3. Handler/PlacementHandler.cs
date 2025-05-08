using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class PlacementHandler : MonoBehaviour
{
    [Header("설정")]
    public LayerMask placementLayer; // 설치 가능한 표면 레이어
    public Material validMaterial; // 설치 가능 머터리얼
    public Material invalidMaterial; // 설치 불가 머터리얼
    public float rotateSpeed = 10f; // 회전 속도

    [Header("무시할 조건")]
    public LayerMask ignoredLayers; // 무시할 레이어
    public string[] ignoredTags; // 무시할 태그 목록

    private GameObject previewObject; // 프리뷰 오브젝트
    private bool isPlacing = false; // 설치 중 여부
    private bool isValidPosition = false; // 현재 위치 설치 가능 여부
    private Renderer[] previewRenderers; // 프리뷰 렌더러 목록
    private Material[][] originalMaterials; // 원본 머터리얼 백업
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

    public void StartPlacement(string prefabAddress) // 설치 시작 - 어드레서블 프리팹 주소 로딩
    {
    if (isPlacing) CancelPlacement(); // 중복 방지

    currentPrefabAddress = prefabAddress; // 현재 설치할 프리팹 주소 저장

    // Addressables 프리팹 비동기 로드 및 인스턴스 생성
    Addressables.InstantiateAsync(prefabAddress).Completed += handle =>
    {
        previewObject = handle.Result; // 로드된 프리팹 인스턴스 미리보기 오브젝트로 설정
        previewRenderers = previewObject.GetComponentsInChildren<Renderer>(); // 모든 렌더러 컴포넌트를 가져옴

        originalMaterials = new Material[previewRenderers.Length][]; // 원본 머터리얼 백업
        for (int i = 0; i < previewRenderers.Length; i++)
        {
            originalMaterials[i] = previewRenderers[i].materials; // 각 렌더러의 머터리얼 배열 복사
        }

        ApplyPreviewMaterial(invalidMaterial); // 빨간색 머터리얼 적용
        DisableColliders(previewObject); // 설치 전 미리보기 충돌 방지
        isPlacing = true; // 설치 중 상태로 전환
        yRotationOffset = 0f; // 사용자 회전 각도 초기화
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
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // 마우스 위치 기준으로 레이 생성

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject hitObject = hit.collider.gameObject; // 충돌한 오브젝트 참조

            previewObject.transform.position = hit.point; // 미리보기 오브젝트 위치 이동

            Quaternion surfaceRotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(Camera.main.transform.forward, hit.normal),
                hit.normal
            );
            Quaternion userRotation = Quaternion.Euler(0, yRotationOffset, 0); // 사용자 회전
            previewObject.transform.rotation = surfaceRotation * userRotation; // 최종 회전 적용

            bool isIgnoredLayer = ((1 << hitObject.layer) & ignoredLayers) != 0; // 무시할 레이어 확인
            bool isIgnoredTag = System.Array.Exists(ignoredTags, tag => hitObject.CompareTag(tag)); // 무시할 태그 확인
            bool isPlaceable = ((1 << hitObject.layer) & placementLayer) != 0; // 설치 가능한 레이어 확인

            if (isIgnoredLayer || isIgnoredTag || !isPlaceable)
            {
                ApplyPreviewMaterial(invalidMaterial); // 빨간색 머터리얼 적용
                isValidPosition = false; // 설치 불가 상태
            }
            else
            {
                ApplyPreviewMaterial(validMaterial); // 초록색 머터리얼 적용
                isValidPosition = true; // 설치 가능 상태
            }
        }
        else
        {
            ApplyPreviewMaterial(invalidMaterial); // 빨간색 머터리얼 적용
            isValidPosition = false; // 설치 불가 상태
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

    void ApplyPreviewMaterial(Material mat) // 프리뷰 오브젝트에 머터리얼 일괄 적용
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