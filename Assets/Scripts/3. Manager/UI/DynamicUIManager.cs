using System;
using UnityEngine;

public class DynamicUIManager : Singleton<DynamicUIManager>
{
    private Canvas _dynamicCanvas;
    private Camera _mainCamera;

    private Plane[] frustumPlanes;
    public static Plane[] FrustumPlanes => Instance.frustumPlanes;

    protected override void Awake()
    {
        base.Awake();
        _dynamicCanvas = GetComponent<Canvas>();
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
    }

    public IBillboardable GetBillboardUI(GameObject prefab)
    {
        GameObject obj = Managers.Pool.Get(prefab.gameObject);
        obj.transform.SetParent(_dynamicCanvas.transform);
        IBillboardable ui = obj.GetComponent<IBillboardable>();
        
        return ui;
    }

    public void ReleaseBillboardUI(GameObject ui)
    {
        Managers.Pool.Release(ui);
    }
}
