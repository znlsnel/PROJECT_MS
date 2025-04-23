using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowManager : UIBase
{
    [Serializable]
    public class WindowItem
    {
        public string windowName;
        public UIBase WindowObject;
        public Button[] ButtonObjects;

        #region Methods
        public void AddListener(UnityAction action)
        {
            foreach(var button in ButtonObjects)
            {
                button.onClick.AddListener(action);
            }
        }

        public void DisableButton(int index)
        {
            ButtonObjects[index].interactable = false;
        }

        public void EnableButton(int index)
        {
            ButtonObjects[index].interactable = true;
        }

        public void DisableAllButtons()
        {
            for(int i = 0; i < ButtonObjects.Length; i++)
            {
                DisableButton(i);
            }
        }

        public void EnableAllButtons()
        {
            for(int i = 0; i < ButtonObjects.Length; i++)
            {
                EnableButton(i);
            }
        }
        #endregion
    }

    public enum ManagementType
    {
        Standard,
        Group,
    }

    public ManagementType managementType = ManagementType.Standard;

    public int currentWindowIndex = 0;

    [SerializeField] private List<WindowItem> windowItems = new List<WindowItem>();

    public UnityEvent onChangeWindow;
    public UnityEvent onOpenWindow;
    public UnityEvent onCloseWindow;
    
    public void Start()
    {
        foreach(var window in windowItems)
        {
            window.AddListener(() => ShowWindow(windowItems.IndexOf(window)));
        }

        HideAllWindows();
        ShowWindow(currentWindowIndex);
    }

    public void ShowWindow(string windowName)
    {
        ShowWindow(windowItems.FindIndex(window => window.windowName == windowName));
    }

    public void ShowWindow(int Windowindex)
    {
        HideAllWindows();
        windowItems[Windowindex].WindowObject.Show();
        windowItems[Windowindex].DisableAllButtons();
        currentWindowIndex = Windowindex;
    }

    private void HideAllWindows()
    {
        for(int i = 0; i < windowItems.Count; i++)
        {
            HideWindow(i);
        }
    }

    private void HideWindow(int Windowindex)
    {
        windowItems[Windowindex].WindowObject.Hide();
        windowItems[Windowindex].EnableAllButtons();
    }
}