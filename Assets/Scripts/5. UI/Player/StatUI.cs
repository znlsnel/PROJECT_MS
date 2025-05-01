using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider steminaSlider;

    public void Start()
    {
        Managers.onChangePlayer += OnChangePlayer;
    }

    private void OnChangePlayer(AlivePlayer player)
    {
        player.Health.onResourceChanged += UpdateHealthUI;
        player.Stamina.onResourceChanged += UpdateStaminaUI;
    }

    private void UpdateHealthUI(float current, float max)
    {
        healthSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }

    private void UpdateStaminaUI(float current, float max)
    {
        steminaSlider.DOValue(current / max, 0.3f).SetEase(Ease.OutQuad);
    }
}
