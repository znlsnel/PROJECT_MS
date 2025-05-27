using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlideValueUI : MonoBehaviour
{
    [SerializeField] Slider slider;
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

        value = startValue;
        slider.value = value;    
        slider.onValueChanged.AddListener(OnValueChanged);
    }



    public void SetValue(float value)
    {
        this.value = value;
        slider.value = value; 
    }
  
 
    private void AddValue(bool isPlus)
    {
        value += isPlus ? rate : -rate;
        value = Mathf.Clamp(value, 0f, 1f); 
        slider.DOValue(value, 0.3f).SetEase(Ease.OutCirc); 

        onValueChanged?.Invoke(value);
    }

    private void OnValueChanged(float value)
    {
        this.value = value;
        onValueChanged?.Invoke(value); 
    }
}
