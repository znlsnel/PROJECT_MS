using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : UIBase, IBillboardable
{
    public TextMeshProUGUI hpText;
    public Slider hpBar;

    public void SetHp(int current, int max)
    {
        hpText.text = $"{current}/{max}";
        hpBar.value = (float)current / max;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void UpdatePosition(Vector3 screenPos)
    {
        transform.position = screenPos;
    }
}
