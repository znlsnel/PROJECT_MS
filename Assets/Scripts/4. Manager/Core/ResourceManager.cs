using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager : Manager
{    
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
    
    protected override void Init() 
    {
        Addressables.InitializeAsync();
    }

 
    public override void Clear()
    {
        foreach (var handle in _resources.Values)
        {
            Addressables.Release(handle); 
        }
        _resources.Clear();
    }

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out Object obj))
        {
            return obj as T;
        }
        return null; 
    }
 
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        { 
            Debug.LogError($"인스턴스화 실패 : {key}");
            return null;
        }

        if (pooling)
            return Managers.Pool.Get(prefab, parent);
        else
            return Object.Instantiate(prefab, parent); 
    }

    public void Destroy(GameObject obj, float time = 0)
    {
        if (Managers.Pool.IsPooling(obj))
            Managers.Pool.Release(obj, time);
        else
        {
            Object.Destroy(obj, time);
        }
    } 
 
    public void Release(Object obj)
    {
        if (_resources.ContainsKey(obj.name))
            _resources.Remove(obj.name);

        Addressables.Release(obj); 
    }

#region Addressable
    public void LoadAsync<T>(string address, Action<T> callback = null) where T : UnityEngine.Object
    {
        // 이미 로드된 핸들이 있는지 확인
        if (_resources.TryGetValue(address, out Object obj))
        {
            callback?.Invoke(obj as T); 
            return;
        }

        // 없으면 새로 로드
        AsyncOperationHandle<T> newHandle = Addressables.LoadAssetAsync<T>(address);
        newHandle.Completed += (op) => {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _resources.Add(address, op.Result);
                callback?.Invoke(op.Result);
            }
            else
            {
                Debug.LogError($"로드 실패 : {address}"); 
            }
        };
    } 

    // TitleScene => [UI TitleUI Label]
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback = null) where T : UnityEngine.Object
    {
        var opHandler = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandler.Completed += (op) => {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) => {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount); 
                });
            }
        };
    }

#endregion
}
