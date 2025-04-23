using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUIManager : MonoBehaviour
{
    /*
    private Dictionary<Billboard, BillboardUI> activeUIs = new Dictionary<Billboard, BillboardUI>();
    private List<Billboard> trackedBillboard = new List<Billboard>();

    private Plane[] frustumPlanes;

    public BillboardUI BillboardPrefab;

    public float maxDistance = 20f;

    public void RegisterBillboard(Billboard billboard)
    {
        if(trackedBillboard.Contains(billboard)) return;

        trackedBillboard.Add(billboard);

        BillboardUI ui = Managers.Pool.Get<BillboardUI>(BillboardPrefab.gameObject, gameObject.transform);
        ui.Bind(billboard);
        ui.SetActive(false);
        activeUIs[billboard] = ui;
    }

    public void UnregisterBillboard(Billboard billboard)
    {
        if(!trackedBillboard.Contains(billboard)) return;

        if(activeUIs.TryGetValue(billboard, out BillboardUI ui))
        {
            activeUIs.Remove(billboard);
            Destroy(ui.gameObject);
        }
    }

    private void LateUpdate()
    {
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        
        // 카메라 위치에서 최대 거리까지의 경계를 계산
        Bounds maxDistanceBounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(maxDistance * 2, maxDistance * 2, maxDistance * 2)
        );

        foreach(var billboard in trackedBillboard)
        {
            // 빌보드가 카메라 시야 내에 있고 최대 거리 내에 있는지 확인
            bool visible = GeometryUtility.TestPlanesAABB(frustumPlanes, billboard.GetWorldBounds) &&
                          maxDistanceBounds.Contains(billboard.GetUIAnchor);

            if(!activeUIs.TryGetValue(billboard, out BillboardUI ui)) continue;

            if(visible)
            {
                ui.SetActive(true);
                ui.UpdatePosition(Camera.main.WorldToScreenPoint(billboard.GetUIAnchor));
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }
    */
}
