using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PopupUI : UIBase
{
	[SerializeField] protected bool _canStack = true; 
	[SerializeField] private bool _isPlaySound = true; 
    private static readonly string _popupOpenSound = "Sound/UI/Popup_02.mp3";
    private static readonly string _popupCloseSound = "Sound/UI/PopupClose_01.mp3";

	public bool CanStack => _canStack;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        Managers.Resource.LoadAsync<AudioClip>(_popupOpenSound);
        Managers.Resource.LoadAsync<AudioClip>(_popupCloseSound);
    }


    public void Init(bool pinned = false)
	{
		Managers.UI.SetCanvas(gameObject, pinned); 
	}

	public void HideWithDoTween(Transform panel)
    { 
		panel.transform.DOKill(); 
        panel.transform.DOScale(0.5f, 0.3f).SetEase(Ease.OutCubic).onComplete += () => {
            base.Hide();  
			panel.transform.localScale = Vector3.one;     
        };

		if (_isPlaySound)
		{ 
			Managers.Resource.LoadAsync<AudioClip>(_popupCloseSound, (audioClip) =>
			{
				Managers.Sound.Play(audioClip);
			}); 
		}  
    }

	public override void Show()
	{
		base.Show();

		if (_isPlaySound)
		{ 
			Managers.Resource.LoadAsync<AudioClip>(_popupOpenSound, (audioClip) =>
			{
				Managers.Sound.Play(audioClip);
			}); 
		}
	}  
 
    protected IEnumerator Fade(float target, float duration)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime;
			canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / duration);
			yield return null;
		}

		canvasGroup.alpha = target;
	}
}
