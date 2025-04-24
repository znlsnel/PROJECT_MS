using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

[Serializable]
public class UIManager : Manager
{
    private Dictionary<string, UIBase> activeUIs = new Dictionary<string, UIBase>();

    public override void Clear()
    {
        
    }

    protected override void Init()
    {
        
    }

    public void PopupUI<T>(string key) where T : UIBase
    {
        if(!activeUIs.TryGetValue(key, out UIBase ui))
        {
            ui = Managers.Resource.Load<T>(key);

            if(ui == null)
            {
                Debug.LogError($"UI 로드 실패: {key}");
                return;
            }

            UIBase obj = GameObject.Instantiate(ui);
            activeUIs.Add(key, obj);
        }

        activeUIs[key].gameObject.SetActive(true);
    }

    public void CloseUI<T>(string key, bool destroy = false) where T : UIBase
    {
        if(activeUIs.TryGetValue(key, out UIBase ui))
        {
            activeUIs.Remove(key);
            if(destroy)
                GameObject.Destroy(ui.gameObject);
            else
                ui.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"UI 찾을 수 없음: {key}");
        }
    }
}
