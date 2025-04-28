using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBase : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
	private bool blockRaycasts = true;
	private bool interactable = true;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        blockRaycasts = canvasGroup.blocksRaycasts;
		interactable = canvasGroup.interactable; 
    }

    public virtual void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = blockRaycasts;
        canvasGroup.interactable = interactable;
    }
    public virtual void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
