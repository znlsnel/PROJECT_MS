using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBase : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
	private bool blockRaycasts = true;
	private bool interactable = true;
	public bool IsOpen { get; private set; }


    protected virtual void Awake()
    {
        canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>(); 
        blockRaycasts = canvasGroup.blocksRaycasts;
		interactable = canvasGroup.interactable; 
    }

    public virtual void Show()
    {
        IsOpen = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = blockRaycasts;
        canvasGroup.interactable = interactable;
    }
    public virtual void Hide()
    {
        IsOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
