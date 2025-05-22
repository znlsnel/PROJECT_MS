
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleTypeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roleTypeText;
    [SerializeField] private Image image;
    
    void Start()
    {
        
    }

    private void SetRoleType(string roleType)
    {
        roleTypeText.text = "마피아";
        image.color = MyColor.Red; 
    }
}
