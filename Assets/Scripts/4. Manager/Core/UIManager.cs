using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIManager : IManager
{
    private Stack<PopupUI> _popupStack = new Stack<PopupUI>();
    private UIBase _sceneUI = null; 

    private int _order = 10;
    private int _pinnedOrder = 100;


    public void Clear()
    {
        
    }

    public void Init()
    {
        
    }


    public void SetCanvas(GameObject go, bool pinned = false)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (pinned)
        {
            if (_pinnedOrder < _order)
                _pinnedOrder = _order + 100;
                
            canvas.sortingOrder = _pinnedOrder;
            _pinnedOrder++;
        }
        else
        {
            canvas.sortingOrder = _order;
            _order++;

        }

    }

    public void RegisterSceneUI(SceneUI sceneUI)
    {
        if (_sceneUI != null)
            _sceneUI.Hide();

        _sceneUI = sceneUI;
    }

    public void HideSceneUI()
    {
        if (_sceneUI == null)
            return;

        _sceneUI.Hide();
    }

    public void ShowSceneUI()
    {
        if (_sceneUI == null)
            return;

        _sceneUI.Show();
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

    public T ShowPopupUI<T>(T popup, bool pinned = false) where T : PopupUI
    {
        if (!pinned) 
            _popupStack.Push(popup);   
 
      //  go.transform.SetParent(_popupUIParent.transform, false);
        popup.Show(); 
        popup.Init(pinned);  
		return popup; 
    }



    public void ClosePopupUI(PopupUI popup, float time = 0.0f)
    {
		if (_popupStack.Count == 0)
			return; 

        if (_popupStack.Peek() != popup)
        {
            return;
        }

        ClosePopupUI(time);
    }

    public void ClosePopupUI(float time = 0.0f)
    {
        if (_popupStack.Count == 0) 
            return;

        PopupUI popup = _popupStack.Pop();
        while (_popupStack.Count > 0 && (popup == null || popup.IsOpen == false))
            popup = _popupStack.Pop();
        

        if (popup != null)
        {
            popup.Hide();
            _order--; 
        } 
        

    }


    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}
