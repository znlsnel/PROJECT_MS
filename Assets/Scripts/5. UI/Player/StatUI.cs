using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider steminaSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider waterSlider;
    public void Start()
    {
        Managers.onChangePlayer += OnChangePlayer;
    }

    private void OnChangePlayer(AlivePlayer player)
    {
        player.Health.onResourceChanged += UpdateHealthUI;
        player.Stamina.onResourceChanged += UpdateStaminaUI;
        player.HungerPoint.onResourceChanged += UpdateHungerUI;
        player.WaterPoint.onResourceChanged += UpdateWaterUI;
    }

    private void UpdateHealthUI(float current, float max)
    {
        healthSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }

    private void UpdateStaminaUI(float current, float max)
    {
        steminaSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }

    private void UpdateHungerUI(float current, float max)
    {
        hungerSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }

    private void UpdateWaterUI(float current, float max)
    {
        waterSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }
}
