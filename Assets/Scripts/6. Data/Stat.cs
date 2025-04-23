using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


[System.Serializable]
public class Stat
{
    public event Action<float> OnValueChanged;
    protected Dictionary<Object, float> additionalValues = new Dictionary<Object, float>();  // 소스별 추가 스탯 값
    protected Dictionary<Object, float> multiplierValues = new Dictionary<Object, float>();  // 소스별 곱셈 스탯 값

    protected float baseValue;        // 기본 스탯 값

    public float Value => (baseValue + GetTotalAdditionalValue()) * GetTotalMultiplierValue();
    public float BaseValue => baseValue;

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
        UpdateValue();
    }

    // 기본 스탯 설정
    public void SetBaseValue(float value)
    {
        baseValue = value;
        UpdateValue();
    }

    // 추가 스탯 설정
    public void AddAdditionalValue(Object source, float value)
    {
        if (additionalValues.ContainsKey(source))
        {
            additionalValues[source] += value;
        }
        else
        {
            additionalValues[source] = value;
        }
        UpdateValue();
    }

    // 곱셈 스탯 설정
    public void AddMultiplierValue(Object source, float value)
    {
        if (multiplierValues.ContainsKey(source))
        {
            multiplierValues[source] += value;
        }
        else
        {
            multiplierValues[source] = value;
        }
        UpdateValue();
    }


    // 특정 소스의 추가 스탯 제거
    public void RemoveAdditionalValue(Object source)
    {
        if (additionalValues.ContainsKey(source))
        {
            additionalValues.Remove(source);
            UpdateValue();
        }
    }

    // 특정 소스의 곱셈 스탯 제거
    public void RemoveMultiplierValue(Object source)
    {   
        if (multiplierValues.ContainsKey(source))
        {
            multiplierValues.Remove(source);
            UpdateValue();
        }
    }

    // 총 추가 스탯 값 계산
    private float GetTotalAdditionalValue()
    {
        float total = 0f;
        foreach (var value in additionalValues.Values)
        {
            total += value;
        }
        return total;
    }

    // 총 곱셈 스탯 값 계산
    private float GetTotalMultiplierValue()
    {
        float total = 1f;
        foreach (var value in multiplierValues.Values)
        {
            total += value;
        }
        return total;
    }

    // 스탯 업데이트
    private void UpdateValue()
    {
        OnValueChanged?.Invoke(Value);   
    }

}
