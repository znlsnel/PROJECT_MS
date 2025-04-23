using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [Header("HP Fill Bar")]
    [SerializeField] private Image hpFillImage;

    [Header("Damage Text")]
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("설정")]
    [SerializeField] private float maxHP = 100f; // 추후 업데이트 부문
    [SerializeField] private float resetDamageTime = 2f; // 누적 데미지 리셋 시간

    private float currentHP; // 적 HP 할당
    private float accumulatedDamage = 0f; // 누적 데미지
    private Coroutine resetCoroutine;

    public Action<float> OnDamageTaken; // Action 바인딩용

    private void Awake()
    {
        currentHP = maxHP;
        OnDamageTaken += TakeDamage;
        UpdateHPUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // 테스트용
        {
            TakeDamage(UnityEngine.Random.Range(5f, 7f));
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        accumulatedDamage += damage;
        UpdateHPUI();

        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        resetCoroutine = StartCoroutine(ResetDamageText());
    }

    private void UpdateHPUI() // Fill, Text 업데이트
    {
        hpFillImage.fillAmount = currentHP / maxHP;
        damageText.text = $"{Mathf.FloorToInt(accumulatedDamage)}";
    }

    private IEnumerator ResetDamageText() // 데미지 리셋
    {
        yield return new WaitForSeconds(resetDamageTime);
        accumulatedDamage = 0f;
        damageText.text = "";
    }
}