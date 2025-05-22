using UnityEngine;
using UnityEngine.UI;

public class OnOffToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject target;
    [SerializeField] private Image background;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        target.SetActive(isOn); 
        background.color = isOn ? MyColor.On : MyColor.Off; 
    }

}
