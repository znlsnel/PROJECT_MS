using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 오브젝트 탐색
                instance = FindAnyObjectByType<T>();
            }
            
            return instance;
        }
    }
    public static bool IsNull => instance == null; 
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // 부모 오브젝트가 있을 경우 부모 해제
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
    }
}
