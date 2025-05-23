using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
   [SerializeField] private GameObject _settingUIPrefab;
   [SerializeField] private Button _settingButton;

    private SettingUI _settingUI;

    private void Awake()
    {
        _settingButton.onClick.AddListener(OpenSettingUI);
    }
 

    private void OpenSettingUI()
    {
        
        if (_settingUI == null)
            _settingUI = Instantiate(_settingUIPrefab).GetComponent<SettingUI>(); 
        
 
            
        Managers.UI.ShowPopupUI(_settingUI); 
    }
}
