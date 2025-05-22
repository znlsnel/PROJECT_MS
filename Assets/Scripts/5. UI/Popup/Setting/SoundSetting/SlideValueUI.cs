using System;
using UnityEngine;
using UnityEngine.UI;

public class SlideValueUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] float startValue = 1f;

    private float rate = 0.1f;
    private float value;

    public Action<float> onValueChanged;

    private void Awake()
    {
        plusButton.onClick.AddListener(() => AddValue(true));
        minusButton.onClick.AddListener(() => AddValue(false));

        fillImage.fillAmount = startValue;  
    }
 

    private void AddValue(bool isPlus)
    {
        value += isPlus ? rate : -rate;
        value = Mathf.Clamp(value, 0, 1);
        fillImage.fillAmount = value;
        onValueChanged?.Invoke(value);
    }
}
