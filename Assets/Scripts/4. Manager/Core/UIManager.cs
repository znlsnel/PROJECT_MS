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
    private Stack<PopupUI> _popupStack = new Stack<PopupUI>();
    private SceneUI _sceneUI = null; 

    private int _order = 10;


    public void Clear()
    {
        
    }

    public void Init()
    {
        
    }


    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
		if (parent != null)
			go.transform.SetParent(parent);

		return Util.GetOrAddComponent<T>(go); 
	}

    public T ShowSceneChildUI<T>(string name = null) where T : SceneUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
      // go.transform.SetParent(_sceneUIParent.transform, false);
 
        return Util.GetOrAddComponent<T>(go); 
    }

	public T ShowSceneUI<T>(string name = null) where T : SceneUI
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI; 
  
		//go.transform.SetParent(_sceneUIParent.transform, false);

		return sceneUI; 
	}

    public T ShowPopupUI<T>(T popup) where T : PopupUI
    {
        _popupStack.Push(popup);

      //  go.transform.SetParent(_popupUIParent.transform, false);
 
        popup.Init();
        popup.Show();
		return popup; 
    }

	public T ShowPopupUI<T>(string name = null) where T : PopupUI
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
    

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        return ShowPopupUI(popup);
    }

    public void ClosePopupUI(PopupUI popup, float time = 0.0f)
    {
		if (_popupStack.Count == 0)
			return;

        if (_popupStack.Peek() != popup)
        {
            Debug.LogError("Close Popup Failed!");
            return;
        }

        ClosePopupUI(time);
    }

    public void ClosePopupUI(float time = 0.0f)
    {
        if (_popupStack.Count == 0)
            return;

        PopupUI popup = _popupStack.Pop();
        popup.Hide();
        _order--; 

    }


    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}
