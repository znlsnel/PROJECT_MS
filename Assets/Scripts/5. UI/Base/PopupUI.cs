using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PopupUI : UIBase
{
	[SerializeField] private bool _isPlaySound = true;
    private static readonly string _popupOpenSound = "Sound/UI/Popup_02.mp3";
    private static readonly string _popupCloseSound = "Sound/UI/PopupClose_01.mp3";

    protected override void Awake()
    {
        base.Awake();

    }

    public void Init()
	{
		Managers.UI.SetCanvas(gameObject);
	}

	public override void Show()
	{
		base.Show();
		if (_isPlaySound)
			Managers.Sound.Play(_popupOpenSound);
	}

    public override void Hide()
    {
        base.Hide();
		if (_isPlaySound)
			Managers.Sound.Play(_popupCloseSound);
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
