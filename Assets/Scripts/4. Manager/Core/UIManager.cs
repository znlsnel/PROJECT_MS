using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

[Serializable]
public class UIManager : IManager
{
    private Dictionary<string, UIBase> _activeUIs = new Dictionary<string, UIBase>();
    private UIBase _hudUI;

    public void Clear()
    {
        
    }

    public void Init()
    {
        
    }

    // HUD UI 관리
    public void SetHUD<T>(string key) where T : UIBase
    {
        if (_hudUI != null)
            GameObject.Destroy(_hudUI.gameObject); 

        _hudUI = Managers.Resource.Instantiate(key).GetComponent<T>();
    }

    public void CloseHUD()
    {
        _hudUI?.gameObject.SetActive(false);
    }

    public void OpenHUD()
    {
        _hudUI?.gameObject.SetActive(true); 
    }

    public void OpenPopupUI<T>(string key) where T : UIBase
    {
        if(!_activeUIs.TryGetValue(key, out UIBase ui))
        {
            T obj = Managers.Resource.Instantiate(key).GetComponent<T>();
            _activeUIs.Add(key, obj);
        }

        _activeUIs[key].gameObject.SetActive(true);
    }
 
    public void CloseUI<T>(string key) where T : UIBase
    {
        if(_activeUIs.TryGetValue(key, out UIBase ui))
            ui.gameObject.SetActive(false);
        
    }
}
