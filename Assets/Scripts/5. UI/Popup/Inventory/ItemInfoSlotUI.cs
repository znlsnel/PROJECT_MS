using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoSlotUI : MonoBehaviour
{
    [Header("Slot Images")]
    [SerializeField] private Image background;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite heartImage;
    [SerializeField] private Sprite StaminaImage;
    [SerializeField] private Sprite DamageImage;
    [SerializeField] private Sprite DurabilityImage;

    [Header("Slot Text")]
    [SerializeField] private TextMeshProUGUI valueText;


    public void Setup(EItemEffectType effectType, int value)
    {
        background.color = effectType == EItemEffectType.Damage ? MyColor.Brown : 
            effectType == EItemEffectType.Heal ? MyColor.Red : 
            effectType == EItemEffectType.Stamina ? MyColor.Yellow : MyColor.Blue; 

        icon.sprite = effectType == EItemEffectType.Damage ? DamageImage :
            effectType == EItemEffectType.Heal ? heartImage : 
            effectType == EItemEffectType.Stamina ? StaminaImage : DurabilityImage;

        valueText.text = value > 0 ? $"+{value}" : value.ToString();
    }

}
