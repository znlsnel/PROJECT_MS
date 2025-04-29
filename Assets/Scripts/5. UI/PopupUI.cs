using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class PopupUI : UIBase
{

    protected override void Awake()
    {
        base.Awake();

    }

    public void Init()
	{
		Managers.UI.SetCanvas(gameObject);
	}


	private IEnumerator Fade(float target, float duration)
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
