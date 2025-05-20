using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class PoolManager : IManager
{
	#region Pool
	class Pool 
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<GameObject> _poolStack = new Stack<GameObject>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform; 
            Root.name = $"{original.name}_Root";
 
            for (int i = 0; i < count; i++)
                Push(Create());
        }

        GameObject Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go;
        }

        public void Push(GameObject obj)
        {
            if (obj == null)
                return;
            
            obj.transform.parent = Root;
            obj.gameObject.SetActive(false);

            _poolStack.Push(obj);
        }
 
        public GameObject Pop(Transform parent)
        {
            GameObject obj;

            if (_poolStack.Count > 0)
                obj = _poolStack.Pop();
            else
                obj = Create();

            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent, false); 
            obj.transform.localPosition = Vector3.zero; 
            return obj; 
        }
    }
	#endregion

	private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    private Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }


    public void CreatePool(string key, int count = 1)
    {
        GameObject original = Managers.Resource.Load<GameObject>(key);
        if (original == null)
            return;

        CreatePool(original, count); 
    }

    public void CreatePool(GameObject original, int count = 1)
    {
        Pool pool = new Pool(); 
        pool.Init(original, count);  
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Release(GameObject obj, float time)
    {
        Managers.Instance.StartCoroutine(ReleaseCoroutine(obj, time));
    } 
 
    private IEnumerator ReleaseCoroutine(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Release(obj);
    }

    public void Release(GameObject obj)
    {
        string name = obj.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(obj.gameObject);
            return;
        }

        _pool[name].Push(obj);
    } 

    public GameObject Get(string key, Transform parent = null)
    {
        GameObject obj = Managers.Resource.Load<GameObject>(key);
        if (obj == null)
            return null;

        return Get(obj, parent); 
    }

    public GameObject Get(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        GameObject obj = _pool[original.name].Pop(parent);
        return obj;
    }

    public T Get<T>(string key, Transform parent = null) where T : Component
    {
        GameObject obj = Get(key, parent);
        return obj.GetComponent<T>();
    }

    public T Get<T>(GameObject original, Transform parent = null) where T : Component
    {
        GameObject obj = Get(original, parent);
        return obj.GetComponent<T>();
    }

    public bool IsPooling(GameObject obj)
    {
        return _pool.ContainsKey(obj.name);
    }
}
