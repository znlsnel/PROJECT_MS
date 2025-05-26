using UnityEngine;
using UnityEngine.UI;

public class ColorUI : MonoBehaviour
{
    [SerializeField] private GameObject _selectedImage;
    [SerializeField] private ColorType _colorType;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        NetworkRoomSystem.Instance.ChangeColor(_colorType);
    }
}
